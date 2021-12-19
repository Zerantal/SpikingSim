#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SpikingLibrary;
using ZedGraph;

#endregion

namespace SpikingLibTest
{
    public partial class NeuronSimulator : Form
    {
        private double _a, _b, _c, _d, _biasCurrent;

        private readonly object[,] _presetParams =
            {
                {"Tonic spiking", 0.02, 0.2, -65, 6, 14},
                {"Phasic spiking", 0.02, 0.25, -65, 6, 0.5},
                {"Tonic bursting", 0.02, 0.2, -50, 2, 15},
                {"Phasic bursting", 0.02, 0.25, -55, 0.05, 0.6},
                {"Mixed mode", 0.02, 0.2, -55, 4, 10},
                {"Spike frequency adaptation", 0.01, 0.2, -65, 8, 30},
                {"Class 1", 0.02, -0.1, -55, 6, 0},
                {"Class 2", 0.2, 0.26, -65, 0, 0},
                {"Spike latency", 0.02, 0.2, -65, 6, 7},
                {"Subthreshold Oscillations", 0.05, 0.26, -60, 0, 0},
                {"Resonator", 0.1, 0.26, -60, -1, 0},
                {"Integrator", 0.02, -0.1, -55, 6, 0},
                {"Rebound spike", 0.03, 0.25, -60, 4, 0},
                {"Rebound burst", 0.03, 0.25, -52, 0, 0},
                {"Threshold variability", 0.03, 0.25, -60, 4, 0},
                {"Bistability", 1, 1.5, -60, 0, -67},
                {"DAP", 1, 0.2, -60, -21, 0},
                {"Accommodation", 0.02, 1, -55, 4, 0},
                {"Inhibition-induced spiking", -0.02, -1, -60, 8, 80},
                {"Inhibition-induced bursting", -0.026, -1, -45, 0, 80}
            };

        private readonly NeuronProbe _probe;
        private bool _settingPoint;
        private LineItem _du;
        private LineItem _dv;
        private MasterPane _master;
        private GraphPane _phasePane;
        private LineItem _phasePortrait;
        private LineItem _phasePosition;

        private RollingPointPairList _phaseTrace;
        private LineItem _resetPot;

        private LineItem _u;
        private RollingPointListWithStaticX _uBuffer;
        private GraphPane _uPane;
        private LineItem _v;
        private RollingPointListWithStaticX _vBuffer;

        private const int UpdateFreq = 20;
        private int _updateCounter;

        public NeuronSimulator()
        {
            InitializeComponent();

            InitialiseGraphDisplay();
            
            // Create network            
            var neuron = SpikingNetEngine.CreateNeuron(new NeuronParameters());            
            SpikingNetEngine.Start();
            SpikingNetEngine.SlowNeuralEngine(10000); // otherwise gui will hang
            _probe = new NeuronProbe(neuron, 1);
            _probe.NeuronProbed += ProbeCallBack;            
            _probe.Start();

            for (int i = 0; i < _presetParams.GetLength(0); i++)
            {
                paramCB.Items.Add(_presetParams[i, 0]);
            }

            paramCB.SelectedIndex = 0; // triggers execution of neural net scheduler        
        }

        private void InitialiseGraphDisplay()
        {
            _master = zg1.MasterPane;
            _master.PaneList.Clear();
            _master.Margin.All = 10;
            _vBuffer = new RollingPointListWithStaticX(900, 0, 100);
            _uBuffer = new RollingPointListWithStaticX(900, 0, 100);
            _phaseTrace = new RollingPointPairList(100);

            _vPane = new GraphPane();
            _uPane = new GraphPane();
            _phasePane = new GraphPane();

            _vPane.Title.Text = "Membrane potential, v";
            _vPane.XAxis.Title.Text = "time (ms)";
            _vPane.YAxis.MajorGrid.IsZeroLine = false;
            _vPane.YAxis.MajorGrid.IsVisible = true;
            _vPane.XAxis.MajorGrid.IsVisible = true;
            _vPane.XAxis.Scale.Min = 0;
            _vPane.XAxis.Scale.Max = 100;
            _vPane.YAxis.Scale.Min = -100;
            _vPane.YAxis.Scale.Max = 40;

            _uPane.Title.Text = "Recovery variable, u";
            _uPane.XAxis.Title.Text = "time (ms)";
            _uPane.YAxis.MajorGrid.IsZeroLine = false;
            _uPane.YAxis.MajorGrid.IsVisible = true;
            _uPane.XAxis.MajorGrid.IsVisible = true;
            _uPane.XAxis.Scale.Min = 0;
            _uPane.XAxis.Scale.Max = 100;
            _uPane.YAxis.Scale.Min = -50;
            _uPane.YAxis.Scale.Max = 50;

            _phasePane.Title.Text = "Phase Portrait";
            _phasePane.XAxis.Title.Text = "v";
            _phasePane.YAxis.Title.Text = "u";
            _phasePane.YAxis.MajorGrid.IsZeroLine = false;
            _phasePane.XAxis.Scale.Min = -100;
            _phasePane.XAxis.Scale.Max = 0;
            _phasePane.YAxis.Scale.Min = -50;
            _phasePane.YAxis.Scale.Max = 50;

            _master.Add(_vPane);
            _master.Add(_uPane);
            _master.Add(_phasePane);

            // Reconfigure the axis ranges for the GraphPanes
            zg1.AxisChange();

            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = CreateGraphics())
            {
                _master.SetLayout(g, PaneLayout.ExplicitRow21);
            }

            _dv = new LineItem("v'=0") {Color = Color.Blue, Symbol = {Type = SymbolType.None}};
            _du = new LineItem("u'=0") {Color = Color.Violet, Symbol = {Type = SymbolType.None}};
            _resetPot = new LineItem("reset potential")
                           {
                               Color = Color.Green,
                               Symbol = {Type = SymbolType.None},
                               Line = {Style = DashStyle.Dot}
                           };
            _u = new LineItem("", _uBuffer, Color.Green, SymbolType.None);
            _v = new LineItem("", _vBuffer, Color.Blue, SymbolType.None);
            _phasePortrait = new LineItem("Phase Portrait", _phaseTrace, Color.Black, SymbolType.None);
            _phasePosition = new LineItem("")
                                {
                                    Symbol = new Symbol(SymbolType.Circle, Color.Red)
                                                 {Fill = new Fill(Color.Red)}
                                };
            _phasePane.CurveList.Add(_dv);
            _phasePane.CurveList.Add(_du);
            _phasePane.CurveList.Add(_resetPot);
            _phasePane.CurveList.Add(_phasePortrait);
            _phasePane.CurveList.Add(_phasePosition);

            _vPane.CurveList.Add(_v);
            _uPane.CurveList.Add(_u);
        }

        private void BiasCurrentUpDownValueChanged(object sender, EventArgs e)
        {
            PlotDv();
            zg1.Refresh();
            _biasCurrent = decimal.ToDouble(biasCurrentUpDown.Value);
            UpdateNeuronClass();
        }

        private void UpdateNeuronClass()
        {
            _probe.ChangeNeuronType(new NeuronParameters(_a, _b, _c, _d, _biasCurrent));
        }

        private void PlotDv()
        {
            double vMin = _phasePane.XAxis.Scale.Min;
            double vMax = _phasePane.XAxis.Scale.Max;
            double vStep = (vMax - vMin)/100.0;
            double v;

            _dv.Clear();

            double I = decimal.ToDouble(biasCurrentUpDown.Value);

            for (v = vMin; v <= vMax; v += vStep)
            {
                double u = 0.04*v*v + 5*v + 140 + I;
                _dv.AddPoint(new PointPair(v, u));
            }
        }

        private void CParamUpDownValueChanged(object sender, EventArgs e)
        {
            PlotResetPotential();
            zg1.Refresh();
            _c = Convert.ToDouble(cParamUpDown.Value);
            UpdateNeuronClass();
        }

        private void PlotResetPotential()
        {
            _resetPot.Clear();
            // plot reset potential on phase portrait
            _resetPot.AddPoint(new PointPair(decimal.ToDouble(cParamUpDown.Value), _phasePane.YAxis.Scale.Min));
            _resetPot.AddPoint(new PointPair(decimal.ToDouble(cParamUpDown.Value), _phasePane.YAxis.Scale.Max));
        }

        private void BParamUpDownValueChanged(object sender, EventArgs e)
        {
            PlotDu();
            _b = Convert.ToDouble(bParamUpDown.Value);
            zg1.Refresh();

            UpdateNeuronClass();
        }

        private void PlotDu()
        {
            double vMin = _phasePane.XAxis.Scale.Min;
            double vMax = _phasePane.XAxis.Scale.Max;
            double vStep = (vMax - vMin)/100.0;
            double v;
            double b = decimal.ToDouble(bParamUpDown.Value);

            _du.Clear();

            for (v = vMin; v <= vMax; v += vStep)
            {
                double u = b*v;
                _du.AddPoint(new PointPair(v, u));
            }
        }

        private void QuitBtnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ParamCbSelectedIndexChanged(object sender, EventArgs e)
        {           
            var ctl = sender as ComboBox;

            int i = 0;

            if (ctl == null) return;

            while (i < _presetParams.GetLength(0) && ctl.SelectedItem != _presetParams[i, 0])
            {
                i++;
            }

            if (i == _presetParams.GetLength(0)) return;

            double a = Convert.ToDouble(_presetParams[i, 1]);
            double b = Convert.ToDouble(_presetParams[i, 2]);
            double c = Convert.ToDouble(_presetParams[i, 3]);
            double d = Convert.ToDouble(_presetParams[i, 4]);
            double I = Convert.ToDouble(_presetParams[i, 5]);

            aParamUpDown.Value = Convert.ToDecimal(a);
            bParamUpDown.Value = Convert.ToDecimal(b);
            cParamUpDown.Value = Convert.ToDecimal(c);
            dParamUpDown.Value = Convert.ToDecimal(d);
            biasCurrentUpDown.Value = Convert.ToDecimal(I);

            _probe.ChangeNeuronType(new NeuronParameters(a, b, c, d, I, Convert.ToString(_presetParams[i, 0])));
            RescaleGraphs();
        }

        private void RescaleGraphs()
        {
            double I = decimal.ToDouble(biasCurrentUpDown.Value);
            // rescale phase plane
            _phasePane.XAxis.Scale.Min = -100;
            _phasePane.XAxis.Scale.Max = 0;
            _phasePane.YAxis.Scale.Min = I - 50;
            _phasePane.YAxis.Scale.Max = I + 50;

            // u plot
            _uPane.XAxis.Scale.Min = 0;
            _uPane.XAxis.Scale.Max = 100;
            _uPane.YAxis.Scale.Min = I - 50;
            _uPane.YAxis.Scale.Max = I + 50;

            // v plot
            _vPane.XAxis.Scale.Min = 0;
            _vPane.XAxis.Scale.Max = 100;
            _vPane.YAxis.Scale.Min = -100;
            _vPane.YAxis.Scale.Max = 40;

            // Re-plot graphs
            PlotDv();
            PlotResetPotential();
        }

        private void ProbeCallBack(object sender, NeuronProbeUpdateEventArgs e)
        {

            _vBuffer.Add(e.V);
            _uBuffer.Add(e.U);
            _phaseTrace.Add(new PointPair(e.V, e.U));
            _phasePosition.Clear();
            _phasePosition.AddPoint(new PointPair(e.V, e.U));

            if (_updateCounter == UpdateFreq)
            {
                _updateCounter = 0;
                UpdateGraphs();
            }
            _updateCounter++;

        }

        private void UpdateGraphs()
        {
           zg1.Refresh();            
        }


        private void NeuronSimulatorFormClosed(object sender, FormClosedEventArgs e)
        {
            SpikingNetEngine.Stop();

            while (SpikingNetEngine.IsRunning)
                Application.DoEvents(); // Process any events that the neural network thread has marshalled            
        }

        private void ExcPulseBtnClick(object sender, EventArgs e)
        {
            _probe.IncrementState(0, 10);            
        }

        private void InhibitoryPulseBtnClick(object sender, EventArgs e)
        {
            _probe.IncrementState(0,-10);            
        }

        private void SpeedSliderScroll(object sender, EventArgs e)
        {
            SpikingNetEngine.SlowNeuralEngine(SpeedSlider.Maximum + SpeedSlider.Minimum - SpeedSlider.Value);
        }

        private void PauseBtnClick(object sender, EventArgs e)
        {
            if (pauseBtn.Text == @"Pause")
            {
                SpikingNetEngine.Pause();
                pauseBtn.Text = @"Resume";
            }
            else
            {
                if (_settingPoint)
                {
                    _settingPoint = false;
                    SetPointBtn.ForeColor = Color.Black;
                }
                SpikingNetEngine.Resume();
                pauseBtn.Text = @"Pause";
            }
        }

        private void SetPointBtnClick(object sender, EventArgs e)
        {
            _settingPoint = true;
            SetPointBtn.ForeColor = Color.Red;
            if (pauseBtn.Text == @"Pause")
                PauseBtnClick(sender, e);

            // look at zg1.MouseDown event handler for continuation of this process
        }

        private bool Zg1MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (!_settingPoint) return default;

            Scale uScale = _phasePane.YAxis.Scale;
            Scale vScale = _phasePane.XAxis.Scale;
            _phasePane.ReverseTransform(new PointF(e.X, e.Y), out var v, out var u);
            if (u < uScale.Min || u > uScale.Max || v < vScale.Min || v > vScale.Max)
                return false; // if user clicks doesn't click on phase pane
            _probe.ChangeU(u);
            _probe.ChangeV(v);  
            _phasePortrait.Clear();
            PauseBtnClick(sender, EventArgs.Empty);
            _settingPoint = false;
            return true;
        }

        private void AParamUpDownValueChanged(object sender, EventArgs e)
        {
            _a = Convert.ToDouble(aParamUpDown.Value);
            UpdateNeuronClass();
        }

        private void DParamUpDownValueChanged(object sender, EventArgs e)
        {
            _d = Convert.ToDouble(dParamUpDown.Value);

            UpdateNeuronClass();
        }

        private void Zg1ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            PlotDv();
            PlotResetPotential();
            PlotDu();
            zg1.Refresh();
        }
    }
}