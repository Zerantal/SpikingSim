using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SpikingLibrary;

namespace SpikingLibTest
{
    public partial class NeuronDisplayTest : Form
    {
        public NeuronDisplayTest()
        {
            InitializeComponent();

            setupSNN();
        }

        private void setupSNN()
        {           
            Neuron n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, 0.2, -50, 2, 15));            
            neuronDisplay.Target = n;
            SpikingNetEngine.SlowNeuralEngine(5000);            

            SpikingNetEngine.Start();            
        }

        private void NeuronDisplayTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            SpikingNetEngine.Stop();
        }
    }
}
