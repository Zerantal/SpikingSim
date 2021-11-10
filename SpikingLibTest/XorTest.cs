using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

using SpikingLibrary;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    public partial class XorTest : Form
    {
        private Neuron m1, m2, m3, o;
        private Synapse Xm1, Xm2, Ym2, Ym3;
        private Synapse m1o, m2o, m3o;
        private NonLearningSynapse trainingSynapse;
        private TemporalEncodedAfferent x;
        private TemporalEncodedAfferent y;
        private TemporalEncodedAfferent _trainingAfferent;

        private SynapseProbe Xm1Probe, Xm2Probe, Ym2Probe, Ym3Probe;
        private SynapseProbe m1oProbe, m2oProbe, m3oProbe;
        private NeuronProbe m1Probe, m2Probe, m3Probe, outputProbe;

        private int trainingDelay = 200; // 20 ms

        private int[,] trainingData = {
                                          {1, 1, 0},
                                          {1, 0, 1},
                                          {0, 1, 1},
                                          {0, 0, 0}
                                      };

        private TimeDelayNotification notifier = new TimeDelayNotification();

        public XorTest()
        {
            InitializeComponent();
        }

        private void XorTest_Load(object sender, EventArgs e)
        {
            NeuronParameters middleClass = NeuronParameters.PhasicSpiking;
            NeuronParameters outputClass = NeuronParameters.PhasicSpiking;
            m1 = SpikingNetEngine.CreateNeuron(middleClass);
            m2 = SpikingNetEngine.CreateNeuron(middleClass);
            m3 = SpikingNetEngine.CreateNeuron(middleClass);
            o = SpikingNetEngine.CreateNeuron(outputClass);
            Xm1 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            Xm2 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            Ym2 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            Ym3 = new Synapse(10, 5, StdpParameters.HippocampalCulture);
            m1o = new Synapse(10, 0.5, StdpParameters.HippocampalCulture);
            m2o = new Synapse(10, 0.5, StdpParameters.HippocampalCulture);
            m3o = new Synapse(10, 0.5, StdpParameters.HippocampalCulture);

            x = new TemporalEncodedAfferent(0, 1, 100);
            y = new TemporalEncodedAfferent(0, 1, 100);
            _trainingAfferent = new TemporalEncodedAfferent(0, 1, 100);
            trainingSynapse = new NonLearningSynapse(trainingDelay, 50);
            _trainingAfferent.ConnectTo(o, trainingSynapse);

            x.ConnectTo(m1, Xm1);
            x.ConnectTo(m2, Xm2);
            y.ConnectTo(m2, Ym2);
            y.ConnectTo(m3, Ym3);
            m1.ConnectTo(o, m1o);
            m2.ConnectTo(o, m2o);
            m3.ConnectTo(o, m3o);            

            m1NeuronDisplay.Target = m1;
            m2NeuronDisplay.Target = m2;
            m3NeuronDisplay.Target = m3;
            outputNeuronDisplay.Target = o;
            
            SpikingNetEngine.SlowNeuralEngine(500);
            SpikingNetEngine.Start();
            
            // setup probes
            Xm1Probe = new SynapseProbe(Xm1, 10);
            Xm2Probe = new SynapseProbe(Xm2, 10);
            Ym2Probe = new SynapseProbe(Ym2, 10);
            Ym3Probe = new SynapseProbe(Ym3, 10);
            m1oProbe = new SynapseProbe(m1o, 10);
            m2oProbe = new SynapseProbe(m2o, 10);
            m3oProbe = new SynapseProbe(m3o, 10);
            m1Probe = new NeuronProbe(m1);
            m2Probe = new NeuronProbe(m2);
            m3Probe = new NeuronProbe(m3);
            outputProbe = new NeuronProbe(o);

            Xm1Probe.SynapseProbed += SynapseProbed;
            Xm2Probe.SynapseProbed +=SynapseProbed;
            Ym2Probe.SynapseProbed += SynapseProbed;
            Ym3Probe.SynapseProbed += SynapseProbed;
            m1oProbe.SynapseProbed += SynapseProbed;
            m2oProbe.SynapseProbed += SynapseProbed;
            m3oProbe.SynapseProbed += SynapseProbed;
            Xm1Probe.Start();
            Xm2Probe.Start();
            Ym2Probe.Start();
            Ym3Probe.Start();
            m1oProbe.Start();
            m2oProbe.Start();
            m3oProbe.Start();

            //  display neuron paramters in respective controls
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

        void SynapseProbed(object sender, SynapseProbeUpdateEventArgs e)
        {
            if (ReferenceEquals(Xm1Probe, sender))
                synapseXm1TB.Text = e.Weight.ToString();
            else if (ReferenceEquals(Xm2Probe, sender))
                synapseXm2TB.Text = e.Weight.ToString();
            else if (ReferenceEquals(Ym2Probe, sender))
                synapseYm2TB.Text = e.Weight.ToString();
            else if (ReferenceEquals(Ym3Probe, sender))
                synapseYm3TB.Text = e.Weight.ToString();
            else if (ReferenceEquals(m1oProbe, sender))
                synapsem1oTB.Text = e.Weight.ToString();
            else if (ReferenceEquals(m2oProbe, sender))
                synapsem2oTB.Text = e.Weight.ToString();
            else if (ReferenceEquals(m3oProbe, sender))
                synapsem3oTB.Text = e.Weight.ToString();            
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (pauseBtn.Text == "Pause")
            {
                SpikingNetEngine.Pause();
                pauseBtn.Text = "Resume";
            }
            else
            {
                SpikingNetEngine.Resume();
                pauseBtn.Text = "Pause";
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
            double a = Decimal.ToDouble(aMiddleUD.Value);
            double b = Decimal.ToDouble(bMiddleUD.Value);
            double c = Decimal.ToDouble(cMiddleUD.Value);
            double d = Decimal.ToDouble(dMiddleUD.Value);
            double I = Decimal.ToDouble(IMiddleUD.Value);

            NeuronParameters newClass = new NeuronParameters(a, b, c, d, I);
            m1Probe.ChangeNeuronType(newClass);
            m2Probe.ChangeNeuronType(newClass);
            m3Probe.ChangeNeuronType(newClass);
        }

        private void OutputUD_ValueChanged(object sender, EventArgs e)
        {
            double a = Decimal.ToDouble(aOutputUD.Value);
            double b = Decimal.ToDouble(bOutputUD.Value);
            double c = Decimal.ToDouble(cOutputUD.Value);
            double d = Decimal.ToDouble(dOutputUD.Value);
            double I = Decimal.ToDouble(IOutputUD.Value);

            NeuronParameters newClass = new NeuronParameters(a, b, c, d, I);
            outputProbe.ChangeNeuronType(newClass);            
        }

        private void synapseTB_TextChanged(object sender, EventArgs e)
        {
            try
            {


                if (ReferenceEquals(synapseXm1TB, sender))
                    Xm1Probe.SetWeight(double.Parse(synapseXm1TB.Text));
                else if (ReferenceEquals(synapseXm2TB, sender))
                    Xm2Probe.SetWeight(double.Parse(synapseXm2TB.Text));
                else if (ReferenceEquals(synapseYm2TB, sender))
                    Ym2Probe.SetWeight(double.Parse(synapseYm2TB.Text));
                else if (ReferenceEquals(synapseYm3TB, sender))
                    Ym3Probe.SetWeight(double.Parse(synapseYm3TB.Text));
                else if (ReferenceEquals(synapsem1oTB, sender))
                    m1oProbe.SetWeight(double.Parse(synapsem1oTB.Text));
                else if (ReferenceEquals(synapsem2oTB, sender))
                    m2oProbe.SetWeight(double.Parse(synapsem2oTB.Text));
                else if (ReferenceEquals(synapsem3oTB, sender))
                    m3oProbe.SetWeight(double.Parse(synapsem3oTB.Text));
            }
            catch (FormatException)
            {
                return;
            }
        }

        private void trainBtn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int idx = r.Next(trainingData.GetLength(0));

            //x.SendInput(trainingData[idx, 0]);
            //y.SendInput(trainingData[idx, 1]);
            //_trainingAfferent.SendInput(trainingData[idx, 2]);

        }

        private void automaticTrainingBtn_Click(object sender, EventArgs e)
        {
            notifier.Notification += new EventHandler(notifier_Notification);
            notifier.CreateNotification(1000); // every 100 ms
        }

        void notifier_Notification(object sender, EventArgs e)
        {
            Random r = new Random();
            int idx = r.Next(trainingData.GetLength(0));

            //x.SendInput(trainingData[idx, 0]);
            //y.SendInput(trainingData[idx, 1]);
            //_trainingAfferent.SendInput(trainingData[idx, 2]);

            notifier.CreateNotification(1000);
        }
    }
}
