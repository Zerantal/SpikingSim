using System.Windows.Forms;

using SpikingLibrary;

namespace SpikingLibTest
{
    public partial class NeuronDisplayTest : Form
    {
        public NeuronDisplayTest()
        {
            InitializeComponent();

            SetupSnn();
        }

        private void SetupSnn()
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
