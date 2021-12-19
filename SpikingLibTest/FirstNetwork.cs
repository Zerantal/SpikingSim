using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using SpikingLibrary;
using MathLib.Statistics;
using System.Diagnostics;

namespace SpikingLibTest
{

    public partial class FirstNetwork : Form
    {
        
        private const int Ne = 800;  // Number of excitatory neurons
        private const int Ni = 200;  // Number of inhibitory neurons

        private NeuronCollection _inhibitoryNeurons;
        private NeuronCollection _excitatoryNeurons;
        private NeuronProbe _inhibitoryProbe;
        private NeuronProbe _excitatoryProbe;
        private PeriodicInputSource _inhibitoryInput;
        private PeriodicInputSource _excitatoryInput;

        private RollingPointPairList _inhibitoryFiringsTrace;
        private RollingPointPairList _excitatoryFiringsTrace;
        private RollingPointPairList _inhibatoryFiringPlotTrace;     // data for scatter plot of firings
        private RollingPointPairList _excitatoryFiringPlotTrace;     // data fir scatter plot of firings
        private RollingPointListWithStaticX _inhibitoryNeuronTrace;
        private RollingPointListWithStaticX _excitatoryNeuronTrace;
        private GraphPane _firingPane, _inhibitoryPane, _excitatoryPane, _firingPlotPane;

        private int _updateCounter;
        private const int UpdateInterval = 100; // 8 ms
        private long _currentTime;
        private int _currentInhibitoryCount;
        private int _currentExcitatoryCount;

        private const int TimeBinSize = 20; // 1 ms

        private MasterPane _master;

        public FirstNetwork()
        {
            _currentExcitatoryCount = 0;
            InitializeComponent();

            SetupGraph();
            SetupNetwork();            
        }

        private void SetupGraph()
        {
            Color inhibitoryColour = Color.Blue;
            Color excitatoryColour = Color.Red;

            _master = zgc.MasterPane;
            _master.PaneList.Clear();
            _master.Margin.All = 10;
            _inhibitoryNeuronTrace = new RollingPointListWithStaticX(10000, 0, 1000);
            _excitatoryNeuronTrace = new RollingPointListWithStaticX(10000, 0, 1000);
            _inhibitoryFiringsTrace = new RollingPointPairList(1000);
            _excitatoryFiringsTrace = new RollingPointPairList(1000);
            _inhibatoryFiringPlotTrace = new RollingPointPairList(Ni * 20);  // enough points to hold 1000 ms of data (assuming max firing rate of 20Hz)
            _excitatoryFiringPlotTrace = new RollingPointPairList(Ne * 20);
            
            _inhibitoryPane = new GraphPane();
            _excitatoryPane = new GraphPane();
            _firingPane = new GraphPane();
            _firingPlotPane = new GraphPane();

            _firingPane.Title.Text = "Plot of neuron firings as a function of time";
            _firingPane.XAxis.Title.Text = "time (ms)";
            _firingPane.YAxis.Title.Text = "Number of neural firings";
            _firingPane.YAxis.Scale.Max = 100;

            _firingPlotPane.Title.Text = "Scatter plot of neuron firings";
            _firingPlotPane.XAxis.Title.Text = "time (ms)";
            _firingPlotPane.YAxis.Title.Text = "Neuron number";
            _firingPlotPane.YAxis.Scale.Max = Ni + Ne;

            _inhibitoryPane.Title.Text = "Random inhibitory neuron";
            _inhibitoryPane.XAxis.Title.Text = "time (ms)";
            _inhibitoryPane.YAxis.Title.Text = "Membrane potential (v)";
            _inhibitoryPane.XAxis.Scale.Min = 0;
            _inhibitoryPane.XAxis.Scale.Max = 1000;
            _inhibitoryPane.YAxis.Scale.Min = -100;
            _inhibitoryPane.YAxis.Scale.Max = 40;
            _inhibitoryPane.YAxis.MajorGrid.IsZeroLine = false;

            _excitatoryPane.Title.Text = "Random excitatory neuron";
            _excitatoryPane.XAxis.Title.Text = "time (ms)";
            _excitatoryPane.YAxis.Title.Text = "Membrane potential (v)";
            _excitatoryPane.XAxis.Scale.Min = 0;
            _excitatoryPane.XAxis.Scale.Max = 1000;
            _excitatoryPane.YAxis.Scale.Min = -100;
            _excitatoryPane.YAxis.Scale.Max = 40;
            _excitatoryPane.YAxis.MajorGrid.IsZeroLine = false;

            _master.Add(_firingPane);
            _master.Add(_inhibitoryPane);
            _master.Add(_firingPlotPane);
            _master.Add(_excitatoryPane);

            // Reconfigure the axis ranges for the GraphPanes
            zgc.AxisChange();

            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = CreateGraphics())
            {
                _master.SetLayout(g, PaneLayout.ForceSquare);
            }

            LineItem inhibitoryCurve = _firingPane.AddCurve("Inhibitory neurons", _inhibitoryFiringsTrace, inhibitoryColour, SymbolType.None);
            inhibitoryCurve.Symbol.IsVisible = false;

            LineItem excitatoryCurve = _firingPane.AddCurve("Excitatory neurons", _excitatoryFiringsTrace, excitatoryColour, SymbolType.None);
            excitatoryCurve.Symbol.IsVisible = false;

            LineItem inhibitoryNeuronCurve = _inhibitoryPane.AddCurve("", _inhibitoryNeuronTrace, inhibitoryColour, SymbolType.None);
            inhibitoryNeuronCurve.Symbol.IsVisible = false;

            LineItem excitatoryNeuronCurve = _excitatoryPane.AddCurve("", _excitatoryNeuronTrace, excitatoryColour, SymbolType.None);
            excitatoryNeuronCurve.Symbol.IsVisible = false;

            LineItem inhibitoryNeuronPlot = _firingPlotPane.AddCurve("", _inhibatoryFiringPlotTrace, inhibitoryColour, SymbolType.Circle);
            inhibitoryNeuronPlot.Line.IsVisible = false;
            inhibitoryNeuronPlot.Symbol.Size = 2.0F;
            inhibitoryNeuronPlot.Symbol.Fill = new Fill(inhibitoryColour);
            LineItem excitatoryNeuronPlot = _firingPlotPane.AddCurve("", _excitatoryFiringPlotTrace, excitatoryColour, SymbolType.Circle);
            excitatoryNeuronPlot.Line.IsVisible = false;
            excitatoryNeuronPlot.Symbol.Size = 2.0F;
            excitatoryNeuronPlot.Symbol.Fill = new Fill(excitatoryColour);
        }

        private void SetupNetwork()
        {
            SpikingNetEngine.Start();
            
            int i;
            Random r = new Random();
            double rNum;
            // normally distributed random thalamic input
            _inhibitoryInput = new PeriodicInputSource(10);            
            _excitatoryInput = new PeriodicInputSource(10);
            _inhibitoryNeurons = new NeuronCollection("Inhibitory Neurons");
            _excitatoryNeurons = new NeuronCollection("Excitatory Neurons");
            NeuronCollection allNeurons = new NeuronCollection("All Neurons");            
            Neuron n;        
            
            // Create excitatory neurons            
            for (i = 0; i < Ne; i++)
            {
                rNum = r.NextDouble();
                n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, 0.2, -65 + 15 * Math.Pow(rNum, 2),
                    8 - 6 * Math.Pow(rNum, 2), 0));
                _excitatoryNeurons.Add(n);
                if (i == 0)   // probe first excitatory neuron
                    _excitatoryProbe = new NeuronProbe(n, 1);  // probe random excitatory neuron
            }            

            // Create inhibitory neurons           
            for (i = 0; i < Ni; i++)
            {
                rNum = r.NextDouble();
                n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02 + 0.08 * rNum, 0.25 - 0.05 * rNum, -65, 2, 0));
                _inhibitoryNeurons.Add(n);
                if (i == 0)         // probe first inhibitory neuron
                    _inhibitoryProbe = new NeuronProbe(n, 1); // probe random inhibitory neuron
            }

            allNeurons.Add(_inhibitoryNeurons);
            allNeurons.Add(_excitatoryNeurons);
            _excitatoryNeurons.ConnectTo(allNeurons, new SynapseFactory(
                new NormalRandomGenerator(50, 20), new UniformRandomGenerator(0, 0.5)));
            _inhibitoryNeurons.ConnectTo(allNeurons, new SynapseFactory(
                new NormalRandomGenerator(50, 20), new UniformRandomGenerator(-1, 0)));
            _inhibitoryInput.ConnectTo(_inhibitoryNeurons, new NonLearningNoisySynapseFactory(
                new ConstantGenerator(10), new ConstantGenerator(1), new NormalRandomGenerator(0, 2)));
            _excitatoryInput.ConnectTo(_excitatoryNeurons, new NonLearningNoisySynapseFactory(
                new ConstantGenerator(10), new ConstantGenerator(1), new NormalRandomGenerator(0, 5)));            
            

            // attach event handlers
            _inhibitoryNeurons.NeuronFired += UpdatePlot;
           _excitatoryNeurons.NeuronFired += UpdatePlot;

            // setup neuron probes                      
            _inhibitoryProbe.NeuronProbed += ProbeUpdated; 
            _inhibitoryProbe.Start();
            _excitatoryProbe.NeuronProbed += ProbeUpdated;
            _excitatoryProbe.Start();

            // start network and random inputs
            SpikingNetEngine.SlowNeuralEngine(0);
            SpikingNetEngine.Start();            
            _inhibitoryInput.Start();
            _excitatoryInput.Start();
        }

        private void ProbeUpdated(object sender, NeuronProbeUpdateEventArgs ev)
        {
            if (sender == _inhibitoryProbe)
            {
                _inhibitoryNeuronTrace.Add( ev.V);
            }
            else
            {
                _excitatoryNeuronTrace.Add(ev.V);
            }
            if (_updateCounter == UpdateInterval)
            {
                RefreshGraphs();               
                _updateCounter = 0;
            }

            _updateCounter++;            
        }
        
        private void UpdatePlot(object sender, NeuronCollectionFiringEventArgs e)
        {           
            if (e.Time - _currentTime > TimeBinSize)
            {
                _inhibitoryFiringsTrace.Add((double)e.Time / 10, _currentInhibitoryCount);
                _excitatoryFiringsTrace.Add((double)e.Time / 10, _currentExcitatoryCount);
                _currentTime = e.Time;
                Debug.WriteLine(_currentExcitatoryCount+_currentInhibitoryCount);
                _currentInhibitoryCount = 0;
                _currentExcitatoryCount = 0;                
            }

            if (sender == _inhibitoryNeurons)
            {
                _currentInhibitoryCount++;
                _inhibatoryFiringPlotTrace.Add((double)e.Time/10, e.FiringNeuron.Id);
            }
            else
            {
                _currentExcitatoryCount++;
                _excitatoryFiringPlotTrace.Add((double)e.Time/10, e.FiringNeuron.Id);
            }
            
           

            //if (e.Time > 100000)
                //NeuralNetwork.Pause();

                if (_updateCounter != UpdateInterval) return;

                RefreshGraphs();
            _updateCounter = 0;
        }
        
        private void RefreshGraphs()
        {
            if (WindowState == FormWindowState.Minimized) return;

            _firingPane.XAxis.Scale.Max = (double) _currentTime/10;
            _firingPane.XAxis.Scale.Min = (double) _currentTime/10 - 1000;
            _firingPlotPane.XAxis.Scale.Max = (double) _currentTime/10;
            _firingPlotPane.XAxis.Scale.Min = (double) _currentTime/10 - 1000;
            zgc.AxisChange();
            zgc.Invalidate();
        }

        private void FirstNetwork_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PauseBtn.Text == @"Resume")
                SpikingNetEngine.Resume();

            SpikingNetEngine.Stop(true);
            while (SpikingNetEngine.IsRunning)
                Application.DoEvents();     // Process any events that the neural network thread has marshalled

            Debug.Flush();
        }

        private static void PauseBtn_Click(object sender, EventArgs e)
        {
            if (!(sender is Button b)) return;

            if (b.Text == @"Pause")
            {
                SpikingNetEngine.Pause();
                b.Text = @"Resume";
            }
            else
            {
                SpikingNetEngine.Resume();
                b.Text = @"Pause";
            }
        }

        private void ThalamicNoiseCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ThalamicNoiseCB.Checked)
            {
                _inhibitoryInput.Start();
                _excitatoryInput.Start();
            }
            else
            {
                _inhibitoryInput.Stop();
                _excitatoryInput.Stop();
            }

        }

    }
}
