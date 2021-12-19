using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using System.ComponentModel;

using ZedGraph;

namespace SpikingLibrary
{
    public partial class SynapseDisplayControl : UserControl
    {
        private Synapse _target;
        private  int _sampleInterval;
        private SynapseProbe _probe;

        private readonly GraphPane _weightPane;
        private readonly RollingPointPairList _synapseTrace;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly MasterPane _master;        

        private int _maxTime = 1800; // time period of trace in seconds. 
        private const int NumTracePoints = 10000;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "ZedGraph.Label.set_Text(System.String)")]
        public SynapseDisplayControl()
        {
            InitializeComponent();

            _sampleInterval = (int) Math.Ceiling(10000.0 / NumTracePoints * _maxTime);                         
            _target = null;

            // Setup zedgraph control
            _weightPane = new GraphPane();
            _synapseTrace = new RollingPointPairList(NumTracePoints);

            _master = zgc.MasterPane;
            _master.PaneList.Clear();
            _master.Margin.All = 10;

            _weightPane.Title.Text = "Synaptic Weight Trace";
            _weightPane.XAxis.Title.Text = "time (seconds)";
            _weightPane.YAxis.Title.Text = "Synaptic Weight (mV)";                      
            _weightPane.YAxis.MajorGrid.IsZeroLine = true;      

            _master.Add(_weightPane);
            zgc.AxisChange();

            LineItem weightCurve = _weightPane.AddCurve("", _synapseTrace, Color.Black, SymbolType.None);
            weightCurve.Symbol.IsVisible = false;           
        }

        [Category("Appearance")]
        [Description("Time period of synaptic trace (in seconds)")]
        [DefaultValue(1800)]
        // ReSharper disable once UnusedMember.Global
        public int TracePeriod
        {
            get => _maxTime;
            set
            {
                // Contract.Requires(value >= 60);
                // Contract.Requires(value <= 86400);  // 24 hrs

                _maxTime = value;
                _sampleInterval = (int)Math.Ceiling(10000.0 / NumTracePoints * _maxTime);

                if (_sampleInterval <= 0)
                    _sampleInterval = 1;

                // redraw graph                             
                _weightPane.CurveList.Clear();
                LineItem weightCurve = _weightPane.AddCurve("", _synapseTrace, Color.Black, SymbolType.None);
                weightCurve.Symbol.IsVisible = false;
                zgc.AxisChange();
                zgc.Refresh();
                if (_probe != null)
                    _probe.UpdateInterval = _sampleInterval;
            }
        }

        [Browsable(false)]
        public Synapse Target
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

                _probe = new SynapseProbe(_target, _sampleInterval);
                _probe.SynapseProbed += _probe_SynapseProbed;
                _probe.Start();
            }
        }

        private void _probe_SynapseProbed(object sender, SynapseProbeUpdateEventArgs e)
        {
            _synapseTrace.Add((double)e.Time / 10000, e.Weight);

            RefreshGraph();
        }

        private void RefreshGraph()
        {
            zgc.AxisChange();
            zgc.Invalidate();
        }

        public void ClearTrace()
        {
            Contract.Assume(_weightPane.CurveList[0] != null);

            _weightPane.CurveList[0].Clear();                            
        }

    }
}
