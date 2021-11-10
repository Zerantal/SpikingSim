using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using SpikingLibrary;
using ZedGraph;

namespace SpikingLibTest
{
    public partial class StdpLearningWindow : Form
    {        
        private const double Freq = 1; // in Hz
        private const int NumPairs = 6;
        private const double InitWeight = 0.25;
        
        private GraphPane _pane;
        private MasterPane _master; 
        private PointPairList _learningWindowTrace;

        private double _dt;
        private const double DtMin = -50;
        private const double DtMax = 50;
        private const double DtInc = 3;
        private Synapse _s1;
        private NonLearningSynapse _s2;
        private SynapseProbe _p1;
        private SynapseProbe _p2;
        private PeriodicInputSource _input;
        private readonly TextWriterTraceListener _stdpListener;
        private readonly TimeDelayNotification _timeDelayDelayNotification = new TimeDelayNotification();

        public StdpLearningWindow()
        {
            InitializeComponent();

            // setup debug listener
            _stdpListener = new TextWriterTraceListener("stdpLog.txt");
            Debug.Listeners.Add(_stdpListener);

            SetupGraph();

            SetupNetwork();
        }

        private void SetupNetwork()
        {
            _timeDelayDelayNotification.Notification += new EventHandler(startPairProtocolAgain);
            // create network
            Neuron n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, -0.1, -55, 0, 0));
            int t1, t2;
            _dt = DtMin;
            CalcAxonalDelays(_dt, out t1, out t2);              
            _s1 = new Synapse(t1, InitWeight, StdpParameters.HippocampalCulture);
            _s2 = new NonLearningSynapse(t2, 80);
            _p1 = new SynapseProbe(_s1);
            _p2 = new SynapseProbe(_s2);           
            SpikingNetEngine.SlowNeuralEngine(0);            
            SpikingNetEngine.Start();

            _input = new PeriodicInputSource((int)(10000.0/Freq));
            _input.ConnectTo(n, _s1);
            _input.ConnectTo(n, _s2);
            _input.MaxInputCycles = NumPairs;
            
            // start pair protocol           
            _input.PeriodicInputFinished += _input_PeriodicInputFinished;
            Debug.WriteLine("Δt = " + _dt);
            _input.Start();
                        
        }

        void startPairProtocolAgain(object sender, EventArgs e)
        {
            _learningWindowTrace.Add(_dt, _p1.GetWeight() - InitWeight);

            _dt += DtInc;

            zgc.AxisChange();
            zgc.Refresh();

            if (_dt > DtMax) return; //finish gathering data

            int t1, t2;
            CalcAxonalDelays(_dt, out t1, out t2);
            _p1.SetAxonalDelay(t1);
            _p2.SetAxonalDelay(t2);
            _p1.SetWeight(InitWeight);
            Debug.WriteLine("Δt = " + _dt);
            _input.Start(100000); // start input in 10 seconds            
        }

        private static void CalcAxonalDelays(double dt, out int t1, out int t2)
        {           
            if (dt < 0)
            {
                t1 = (int)(-dt * 10) + 1;
                t2 = 1;
            }
            else
            {
                t1 = 1;
                t2 = (int)(dt * 10) + 1;
            }           
        }

        void _input_PeriodicInputFinished(object sender, EventArgs e)
        {
            // delay for .5 seconds to allow for input signal to finish propogating along axon
            _timeDelayDelayNotification.CreateNotification(5000); 
        }

        private void SetupGraph()
        {
            // Setup zedgraph control
            _pane = new GraphPane();
            _learningWindowTrace = new PointPairList();

            _master = zgc.MasterPane;
            _master.PaneList.Clear();
            _master.Margin.All = 10;

            _pane.Title.Text = "STDP Learning Window";
            _pane.XAxis.Title.Text = "Δt";
            _pane.YAxis.Title.Text = "Δw";
            _pane.YAxis.MajorGrid.IsZeroLine = true;
            _pane.XAxis.Scale.Max = DtMax;
            _pane.XAxis.Scale.Min = DtMin;

            _master.Add(_pane);
            zgc.AxisChange();

            LineItem stdpCurve = _pane.AddCurve("", _learningWindowTrace, Color.Black, SymbolType.None);
            stdpCurve.Symbol.IsVisible = false;     
        }

        private void StdpLearningWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpikingNetEngine.Stop();
            _stdpListener.Flush();
           _stdpListener.Close();
        }
    }
}
