using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using ZedGraph;

namespace SpikingLibrary
{
    public partial class NeuronDisplayControl : UserControl
    {
        private  Neuron _target;
        private readonly int _sampleInterval;
        private NeuronProbe _probe;

        private readonly GraphPane _voltagePane;
        private RollingPointListWithStaticX _neuronTrace;

        private int _maxTime = 100;
        private const int UpdateFreq = 20;
        private int _updateCounter;

        public NeuronDisplayControl()
        {
            InitializeComponent();

            _sampleInterval = 1;
            _target = null;

            // Setup zedgraph control
            _voltagePane = new GraphPane();
            _neuronTrace = new RollingPointListWithStaticX(_maxTime*10, 0, _maxTime);

            var master = zgc.MasterPane;
            master.PaneList.Clear();
            master.Margin.All = 10;

            _voltagePane.Title.Text = "Neuron Trace";
            _voltagePane.XAxis.Title.Text = "time (ms)";
            _voltagePane.YAxis.Title.Text = "Membrane potential (v)";
            _voltagePane.XAxis.Scale.Min = 0;
            _voltagePane.XAxis.Scale.Max = _maxTime;

            _voltagePane.YAxis.Scale.Min = -100;
            _voltagePane.YAxis.Scale.Max = 40;
            _voltagePane.YAxis.MajorGrid.IsZeroLine = false;

            master.Add(_voltagePane);
            zgc.AxisChange();

            LineItem voltageCurve = _voltagePane.AddCurve("", _neuronTrace, Color.Black, SymbolType.None);
            voltageCurve.Symbol.IsVisible = false;
            //voltageCurve.Line.IsSmooth = true;           

        }

        [Category("Appearance")]
        [Description("Maximum time period fo neuron trace (in ms)")]
        [DefaultValue(100)]
        // ReSharper disable once UnusedMember.Global
        public int TracePeriod
        {
            get => _maxTime;
            set
            {
                // Contract.Requires(value >= 10);
                // Contract.Requires(value <= 1000);
                _maxTime = value;               

                // redraw graph
                _neuronTrace = new RollingPointListWithStaticX(_maxTime * 10, 0, _maxTime);
                _voltagePane.XAxis.Scale.Max = _maxTime;
                _voltagePane.CurveList.Clear();
                LineItem voltageCurve = _voltagePane.AddCurve("", _neuronTrace, Color.Black, SymbolType.None);
                voltageCurve.Symbol.IsVisible = false;
                zgc.AxisChange();
                zgc.Refresh();
            }
        }

        [Browsable(false)]
        public Neuron Target
        {
            get => _target;
            set
            {
                if (_probe != null)
                {
                    _probe.Stop();
                    _probe = null;
                }

                _target = value;
                if (Target == null) return;

                _probe = new NeuronProbe(_target, _sampleInterval);
                _probe.NeuronProbed +=  _probe_NeuronProbed;
                _probe.Start();
            }
        }        

        private void _probe_NeuronProbed(object sender, NeuronProbeUpdateEventArgs e)
        {
            _neuronTrace.Add(e.V);

            if (_updateCounter == UpdateFreq)
            {
                _updateCounter = 0;
                RefreshGraph();
            }
            _updateCounter++;
          
        }

        private void RefreshGraph()
        {            
             zgc.AxisChange();
             zgc.Invalidate();            
        }
    }
}
