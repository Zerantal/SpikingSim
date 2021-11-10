using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using System.ComponentModel;

using ZedGraph;

namespace SpikingLibrary
{
    public partial class NeuronDisplayControl : UserControl
    {
        private  Neuron _target;
        private readonly int _sampleInterval = 1;
        private NeuronProbe _probe;

        readonly GraphPane _voltagePane;
        RollingPointListWithStaticX _neuronTrace;
        private readonly MasterPane _master;

        private int _maxTime = 100;
        private const int UpdateFreq = 20;
        private int _updateCounter;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "ZedGraph.Label.set_Text(System.String)")]
        public NeuronDisplayControl()
        {
            InitializeComponent();

            _sampleInterval = 1;
            _target = null;

            // Setup zedgraph control
            _voltagePane = new GraphPane();
            _neuronTrace = new RollingPointListWithStaticX(_maxTime*10, 0, _maxTime);

            _master =  zgc.MasterPane;
            _master.PaneList.Clear();
            _master.Margin.All = 10;

            _voltagePane.Title.Text = "Neuron Trace";
            _voltagePane.XAxis.Title.Text = "time (ms)";
            _voltagePane.YAxis.Title.Text = "Membrane potential (v)";
            _voltagePane.XAxis.Scale.Min = 0;
            _voltagePane.XAxis.Scale.Max = _maxTime;

            _voltagePane.YAxis.Scale.Min = -100;
            _voltagePane.YAxis.Scale.Max = 40;
            _voltagePane.YAxis.MajorGrid.IsZeroLine = false;

            _master.Add(_voltagePane);
            zgc.AxisChange();

            LineItem voltageCurve = _voltagePane.AddCurve("", _neuronTrace, Color.Black, SymbolType.None);
            voltageCurve.Symbol.IsVisible = false;
            //voltageCurve.Line.IsSmooth = true;           

        }

        [Category("Appearance")]
        [Description("Maximum time period fo neuron trace (in ms)")]
        [DefaultValue(100)]
        public int TracePeriod
        {
            get { return _maxTime; }
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
            get { return _target; }
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

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(zgc != null);
            Contract.Invariant(_neuronTrace != null);
            Contract.Invariant(_sampleInterval > 0);
            Contract.Invariant(_maxTime >= 10 && _maxTime <= 1000);
            Contract.Invariant(_voltagePane != null);
        }
    }
}
