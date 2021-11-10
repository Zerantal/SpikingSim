using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpikingLibrary;
using MathLib.Statistics;

namespace SpikingLibTest
{
    public partial class STDPTestForm : Form
    {
        private PeriodicInputSource _input;        
        private Neuron n;        

        public STDPTestForm()
        {
            InitializeComponent();

            SetupNetwork();
        }

        private void SetupNetwork()
        {
            n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, -0.1, -55, 0, 0));

            SpikingNetEngine.Start();
            SpikingNetEngine.SlowNeuralEngine(0);                   
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            int deltaT = decimal.ToInt32(spikeIntervalUD.Value) * 10;
            _input = new PeriodicInputSource(10000/decimal.ToInt32(freqUD.Value));

            int t1, t2; // synaptic delay for s1 and s2 respectively

            if (deltaT < 0)
            {
                t1 = -deltaT + 1;
                t2 = 1;
            }
            else
            {
                t1 = 1;
                t2 = deltaT + 1;
            }
           

            Synapse s1 = new Synapse(t1, 0.5, StdpParameters.RatVisualCortexL23);
            NonLearningSynapse s2 = new NonLearningSynapse(t2, 80);
            _input.ConnectTo(n, s1);
            _input.ConnectTo(n, s2);

            synapseDisplayControl1.Target = s1;
            synapseDisplayControl1.ClearTrace();

            stopBtn.Enabled = true;
            spikeIntervalUD.Enabled = false;
            startBtn.Enabled = false;
            freqUD.Enabled = false;

            SpikingNetEngine.Start();            
            _input.Start();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            _input.Stop();
            stopBtn.Enabled = false;
            spikeIntervalUD.Enabled = true;
            startBtn.Enabled = true;
            freqUD.Enabled = true;

            SpikingNetEngine.Stop();

        }

        private void STDPTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpikingNetEngine.Stop();
        }
    }
}
