using System;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using System.Globalization;
using SpikingLibrary;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    public partial class XorTest : Form
    {
        private Neuron _m1, _m2, _m3, _o;
        private Synapse _xm1, _xm2, _ym2, _ym3;
        private Synapse _m1O, _m2O, _m3O;
        private NonLearningSynapse _trainingSynapse;
        private TemporalEncodedAfferent _x;
        private TemporalEncodedAfferent _y;
        private TemporalEncodedAfferent _trainingAfferent;

        private SynapseProbe _xm1Probe, _xm2Probe, _ym2Probe, _ym3Probe;
        private SynapseProbe _m1OProbe, _m2OProbe, _m3OProbe;
        private NeuronProbe _m1Probe, _m2Probe, _m3Probe, _outputProbe;

        private const int TrainingDelay = 200; // 20 ms

        private readonly int[,] _trainingData = {
                                          {1, 1, 0},
                                          {1, 0, 1},
                                          {0, 1, 1},
                                          {0, 0, 0}
                                      };

        private readonly TimeDelayNotification _notifier = new TimeDelayNotification();

        public XorTest()
        {
            InitializeComponent();
        }

        private void XorTest_Load(object sender, EventArgs e)
        {
            NeuronParameters middleClass = NeuronParameters.PhasicSpiking;
            NeuronParameters outputClass = NeuronParameters.PhasicSpiking;
            _m1 = SpikingNetEngine.CreateNeuron(middleClass);
            _m2 = SpikingNetEngine.CreateNeuron(middleClass);
            _m3 = SpikingNetEngine.CreateNeuron(middleClass);
            _o = SpikingNetEngine.CreateNeuron(outputClass);
            _xm1 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            _xm2 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            _ym2 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            _ym3 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            _m1O = new Synapse(10, 0.5, StdpParameters.HippocampalCulture);
            _m2O = new Synapse(10, 0.5, StdpParameters.HippocampalCulture);
            _m3O = new Synapse(10, 0.5, StdpParameters.HippocampalCulture);

            _x = new TemporalEncodedAfferent(0, 1, 100);
            _y = new TemporalEncodedAfferent(0, 1, 100);
            _trainingAfferent = new TemporalEncodedAfferent(0, 1, 100);
            _trainingSynapse = new NonLearningSynapse(TrainingDelay, 50);
            _trainingAfferent.ConnectTo(_o, _trainingSynapse);

            _x.ConnectTo(_m1, _xm1);
            _x.ConnectTo(_m2, _xm2);
            _y.ConnectTo(_m2, _ym2);
            _y.ConnectTo(_m3, _ym3);
            _m1.ConnectTo(_o, _m1O);
            _m2.ConnectTo(_o, _m2O);
            _m3.ConnectTo(_o, _m3O);            

            m1NeuronDisplay.Target = _m1;
            m2NeuronDisplay.Target = _m2;
            m3NeuronDisplay.Target = _m3;
            outputNeuronDisplay.Target = _o;
            
            SpikingNetEngine.SlowNeuralEngine(500);
            SpikingNetEngine.Start();
            
            // setup probes
            _xm1Probe = new SynapseProbe(_xm1, 10);
            _xm2Probe = new SynapseProbe(_xm2, 10);
            _ym2Probe = new SynapseProbe(_ym2, 10);
            _ym3Probe = new SynapseProbe(_ym3, 10);
            _m1OProbe = new SynapseProbe(_m1O, 10);
            _m2OProbe = new SynapseProbe(_m2O, 10);
            _m3OProbe = new SynapseProbe(_m3O, 10);
            _m1Probe = new NeuronProbe(_m1);
            _m2Probe = new NeuronProbe(_m2);
            _m3Probe = new NeuronProbe(_m3);
            _outputProbe = new NeuronProbe(_o);

            _xm1Probe.SynapseProbed += SynapseProbed;
            _xm2Probe.SynapseProbed +=SynapseProbed;
            _ym2Probe.SynapseProbed += SynapseProbed;
            _ym3Probe.SynapseProbed += SynapseProbed;
            _m1OProbe.SynapseProbed += SynapseProbed;
            _m2OProbe.SynapseProbed += SynapseProbed;
            _m3OProbe.SynapseProbed += SynapseProbed;
            _xm1Probe.Start();
            _xm2Probe.Start();
            _ym2Probe.Start();
            _ym3Probe.Start();
            _m1OProbe.Start();
            _m2OProbe.Start();
            _m3OProbe.Start();

            //  display neuron parameters in respective controls
            aMiddleUD.Value = Convert.ToDecimal(middleClass.A);
            bMiddleUD.Value = Convert.ToDecimal(middleClass.B);
            cMiddleUD.Value = Convert.ToDecimal(middleClass.C);
            dMiddleUD.Value = Convert.ToDecimal(middleClass.D);
            IMiddleUD.Value = Convert.ToDecimal(middleClass.I);
            aOutputUD.Value = Convert.ToDecimal(outputClass.A);
            bOutputUD.Value = Convert.ToDecimal(outputClass.B);
            cOutputUD.Value = Convert.ToDecimal(outputClass.C);
            dOutputUD.Value = Convert.ToDecimal(outputClass.D);
            IOutputUD.Value = Convert.ToDecimal(outputClass.I);
           
        }

        private void SynapseProbed(object sender, SynapseProbeUpdateEventArgs e)
        {
            if (ReferenceEquals(_xm1Probe, sender))
                synapseXm1TB.Text = e.Weight.ToString(CultureInfo.InvariantCulture);
            else if (ReferenceEquals(_xm2Probe, sender))
                synapseXm2TB.Text = e.Weight.ToString(CultureInfo.CurrentCulture);
            else if (ReferenceEquals(_ym2Probe, sender))
                synapseYm2TB.Text = e.Weight.ToString(CultureInfo.InvariantCulture);
            else if (ReferenceEquals(_ym3Probe, sender))
                synapseYm3TB.Text = e.Weight.ToString(CultureInfo.InvariantCulture);
            else if (ReferenceEquals(_m1OProbe, sender))
                synapsem1oTB.Text = e.Weight.ToString(CultureInfo.InvariantCulture);
            else if (ReferenceEquals(_m2OProbe, sender))
                synapsem2oTB.Text = e.Weight.ToString(CultureInfo.InvariantCulture);
            else if (ReferenceEquals(_m3OProbe, sender))
                synapsem3oTB.Text = e.Weight.ToString(CultureInfo.InvariantCulture);            
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (pauseBtn.Text == @"Pause")
            {
                SpikingNetEngine.Pause();
                pauseBtn.Text = @"Resume";
            }
            else
            {
                SpikingNetEngine.Resume();
                pauseBtn.Text = @"Pause";
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            SpikingNetEngine.Stop();
        }

        private void XorTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpikingNetEngine.Stop();
        }

        private void MiddleUD_ValueChanged(object sender, EventArgs e)
        {
            double a = decimal.ToDouble(aMiddleUD.Value);
            double b = decimal.ToDouble(bMiddleUD.Value);
            double c = decimal.ToDouble(cMiddleUD.Value);
            double d = decimal.ToDouble(dMiddleUD.Value);
            double I = decimal.ToDouble(IMiddleUD.Value);

            NeuronParameters newClass = new NeuronParameters(a, b, c, d, I);
            _m1Probe.ChangeNeuronType(newClass);
            _m2Probe.ChangeNeuronType(newClass);
            _m3Probe.ChangeNeuronType(newClass);
        }

        private void OutputUD_ValueChanged(object sender, EventArgs e)
        {
            double a = decimal.ToDouble(aOutputUD.Value);
            double b = decimal.ToDouble(bOutputUD.Value);
            double c = decimal.ToDouble(cOutputUD.Value);
            double d = decimal.ToDouble(dOutputUD.Value);
            double I = decimal.ToDouble(IOutputUD.Value);

            NeuronParameters newClass = new NeuronParameters(a, b, c, d, I);
            _outputProbe.ChangeNeuronType(newClass);            
        }

        private void synapseTB_TextChanged(object sender, EventArgs e)
        {
            try
            {


                if (ReferenceEquals(synapseXm1TB, sender))
                    _xm1Probe.SetWeight(double.Parse(synapseXm1TB.Text));
                else if (ReferenceEquals(synapseXm2TB, sender))
                    _xm2Probe.SetWeight(double.Parse(synapseXm2TB.Text));
                else if (ReferenceEquals(synapseYm2TB, sender))
                    _ym2Probe.SetWeight(double.Parse(synapseYm2TB.Text));
                else if (ReferenceEquals(synapseYm3TB, sender))
                    _ym3Probe.SetWeight(double.Parse(synapseYm3TB.Text));
                else if (ReferenceEquals(synapsem1oTB, sender))
                    _m1OProbe.SetWeight(double.Parse(synapsem1oTB.Text));
                else if (ReferenceEquals(synapsem2oTB, sender))
                    _m2OProbe.SetWeight(double.Parse(synapsem2oTB.Text));
                else if (ReferenceEquals(synapsem3oTB, sender))
                    _m3OProbe.SetWeight(double.Parse(synapsem3oTB.Text));
            }
            catch (FormatException)
            {
            }
        }

        private void trainBtn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            // ReSharper disable once UnusedVariable
            int idx = r.Next(_trainingData.GetLength(0));

            //x.SendInput(trainingData[idx, 0]);
            //y.SendInput(trainingData[idx, 1]);
            //_trainingAfferent.SendInput(trainingData[idx, 2]);

        }

        private void automaticTrainingBtn_Click(object sender, EventArgs e)
        {
            _notifier.Notification += notifier_Notification;
            _notifier.CreateNotification(1000); // every 100 ms
        }

        private void notifier_Notification(object sender, EventArgs e)
        {
            Random r = new Random();
            // ReSharper disable once UnusedVariable
            int idx = r.Next(_trainingData.GetLength(0));

            //x.SendInput(trainingData[idx, 0]);
            //y.SendInput(trainingData[idx, 1]);
            //_trainingAfferent.SendInput(trainingData[idx, 2]);

            _notifier.CreateNotification(1000);
        }
    }
}
