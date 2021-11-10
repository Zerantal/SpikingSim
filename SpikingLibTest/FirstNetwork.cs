using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using SpikingLibrary;
using MathLib.Statistics;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace SpikingLibTest
{

    public partial class FirstNetwork : Form
    {
        
        private const int Ne = 800;  // Number of excitatory neurons
        private const int Ni = 200;  // Number of inhibitory neurons
        
        NeuronCollection _inhibitoryNeurons;
        NeuronCollection _excitatoryNeurons;
        NeuronProbe _inhibProbe;
        NeuronProbe _excitProbe;
        PeriodicInputSource _inhibitoryInput;
        PeriodicInputSource _excitatoryInput;

        RollingPointPairList _inhibitoryFiringsTrace;        
        RollingPointPairList _excitatoryFiringsTrace;
        RollingPointPairList _inhibatoryFiringPlotTrace;     // data for scatter plot of firings
        RollingPointPairList _excitatoryFiringPlotTrace;     // data fir scatter plot of firings
        RollingPointListWithStaticX _inhibNeuronTrace;
        RollingPointListWithStaticX _excitNeuronTrace;
        GraphPane _firingPane, _inhibPane, _excitPane, _firingPlotPane;

        private int _updateCounter;
        private const int UpdateInterval = 100; // 8 ms
        private long _currentTime;
        private int _currentInhibCount;
        private int _currentExcitCount;

        private const int TimeBinSize = 20; // 1 ms

        private MasterPane _master;

        public FirstNetwork()
        {
            _currentExcitCount = 0;
            InitializeComponent();

            SetupGraph();
            SetupNetwork();            
        }

        private void SetupGraph()
        {
            Color inhibColour = Color.Blue;
            Color excitColour = Color.Red;

            _master = zgc.MasterPane;
            _master.PaneList.Clear();
            _master.Margin.All = 10;
            _inhibNeuronTrace = new RollingPointListWithStaticX(10000, 0, 1000);
            _excitNeuronTrace = new RollingPointListWithStaticX(10000, 0, 1000);
            _inhibitoryFiringsTrace = new RollingPointPairList(1000);
            _excitatoryFiringsTrace = new RollingPointPairList(1000);
            _inhibatoryFiringPlotTrace = new RollingPointPairList(Ni * 20);  // enough points to hold 1000 ms of data (assuming max firing rate of 20Hz)
            _excitatoryFiringPlotTrace = new RollingPointPairList(Ne * 20);
            
            _inhibPane = new GraphPane();
            _excitPane = new GraphPane();
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

            _inhibPane.Title.Text = "Random inhibitory neuron";
            _inhibPane.XAxis.Title.Text = "time (ms)";
            _inhibPane.YAxis.Title.Text = "Membrane potential (v)";
            _inhibPane.XAxis.Scale.Min = 0;
            _inhibPane.XAxis.Scale.Max = 1000;
            _inhibPane.YAxis.Scale.Min = -100;
            _inhibPane.YAxis.Scale.Max = 40;
            _inhibPane.YAxis.MajorGrid.IsZeroLine = false;

            _excitPane.Title.Text = "Random excitatory neuron";
            _excitPane.XAxis.Title.Text = "time (ms)";
            _excitPane.YAxis.Title.Text = "Membrane potential (v)";
            _excitPane.XAxis.Scale.Min = 0;
            _excitPane.XAxis.Scale.Max = 1000;
            _excitPane.YAxis.Scale.Min = -100;
            _excitPane.YAxis.Scale.Max = 40;
            _excitPane.YAxis.MajorGrid.IsZeroLine = false;

            _master.Add(_firingPane);
            _master.Add(_inhibPane);
            _master.Add(_firingPlotPane);
            _master.Add(_excitPane);

            // Refigure the axis ranges for the GraphPanes
            zgc.AxisChange();

            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = CreateGraphics())
            {
                _master.SetLayout(g, PaneLayout.ForceSquare);
            }

            LineItem inhibCurve = _firingPane.AddCurve("Inhibitory neurons", _inhibitoryFiringsTrace, inhibColour, SymbolType.None);
            inhibCurve.Symbol.IsVisible = false;

            LineItem excitCurve = _firingPane.AddCurve("Excitatory neurons", _excitatoryFiringsTrace, excitColour, SymbolType.None);
            excitCurve.Symbol.IsVisible = false;

            LineItem inhibNeuronCurve = _inhibPane.AddCurve("", _inhibNeuronTrace, inhibColour, SymbolType.None);
            inhibNeuronCurve.Symbol.IsVisible = false;

            LineItem excitNeuronCurve = _excitPane.AddCurve("", _excitNeuronTrace, excitColour, SymbolType.None);
            excitNeuronCurve.Symbol.IsVisible = false;

            LineItem inhibNeuronPlot = _firingPlotPane.AddCurve("", _inhibatoryFiringPlotTrace, inhibColour, SymbolType.Circle);
            inhibNeuronPlot.Line.IsVisible = false;
            inhibNeuronPlot.Symbol.Size = 2.0F;
            inhibNeuronPlot.Symbol.Fill = new Fill(inhibColour);
            LineItem excitNeuronPlot = _firingPlotPane.AddCurve("", _excitatoryFiringPlotTrace, excitColour, SymbolType.Circle);
            excitNeuronPlot.Line.IsVisible = false;
            excitNeuronPlot.Symbol.Size = 2.0F;
            excitNeuronPlot.Symbol.Fill = new Fill(excitColour);
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
                    _excitProbe = new NeuronProbe(n, 1);  // probe random excitatory neuron
            }            

            // Create inhibitory neurons           
            for (i = 0; i < Ni; i++)
            {
                rNum = r.NextDouble();
                n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02 + 0.08 * rNum, 0.25 - 0.05 * rNum, -65, 2, 0));
                _inhibitoryNeurons.Add(n);
                if (i == 0)         // probe first inhibitory neuron
                    _inhibProbe = new NeuronProbe(n, 1); // probe random inhibitory neuron
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
            _inhibProbe.NeuronProbed += ProbeUpdated; 
            _inhibProbe.Start();
            _excitProbe.NeuronProbed += ProbeUpdated;
            _excitProbe.Start();

            // start network and random inputs
            SpikingNetEngine.SlowNeuralEngine(0);
            SpikingNetEngine.Start();            
            _inhibitoryInput.Start();
            _excitatoryInput.Start();
        }

        private void ProbeUpdated(object sender, NeuronProbeUpdateEventArgs ev)
        {
            if (sender == _inhibProbe)
            {
                _inhibNeuronTrace.Add( ev.V);
            }
            else
            {
                _excitNeuronTrace.Add(ev.V);
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
            if ((e.Time - _currentTime) > TimeBinSize)
            {
                _inhibitoryFiringsTrace.Add((double)e.Time / 10, _currentInhibCount);
                _excitatoryFiringsTrace.Add((double)e.Time / 10, _currentExcitCount);
                _currentTime = e.Time;
                Debug.WriteLine(_currentExcitCount+_currentInhibCount);
                _currentInhibCount = 0;
                _currentExcitCount = 0;                
            }

            if (sender == _inhibitoryNeurons)
            {
                _currentInhibCount++;
                _inhibatoryFiringPlotTrace.Add((double)e.Time/10, e.FiringNeuron.Id);
            }
            else
            {
                _currentExcitCount++;
                _excitatoryFiringPlotTrace.Add((double)e.Time/10, e.FiringNeuron.Id);
            }
            
           

            //if (e.Time > 100000)
                //NeuralNetwork.Pause();

            if (_updateCounter == UpdateInterval)
            {
                RefreshGraphs();
                _updateCounter = 0;
            }
        }
        
        private void RefreshGraphs()
        {
            if (WindowState == FormWindowState.Minimized) return;

            _firingPane.XAxis.Scale.Max = ((double) _currentTime/10);
            _firingPane.XAxis.Scale.Min = ((double) _currentTime/10) - 1000;
            _firingPlotPane.XAxis.Scale.Max = ((double) _currentTime/10);
            _firingPlotPane.XAxis.Scale.Min = ((double) _currentTime/10) - 1000;
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
            Button b = sender as Button;
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

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.zgc != null);
        }
         
    }
}
