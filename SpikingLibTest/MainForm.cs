using System;
using System.Windows.Forms;

namespace SpikingLibTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void schedulerTestBtn_Click(object sender, EventArgs e)
        {
            Form schedulerTestForm = new ScheduleTestForm();            
            schedulerTestForm.ShowDialog();
        }

        private void neuronSimBtn_Click(object sender, EventArgs e)
        {
            Form neuronSimForm = new NeuronSimulator();
            neuronSimForm.ShowDialog();
        }

        private void firstNetBtn_Click(object sender, EventArgs e)
        {
            Form firstNetForm = new FirstNetwork();
            firstNetForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void neuronDisplayBtn_Click(object sender, EventArgs e)
        {
            Form showNeuronDisplayForm = new NeuronDisplayTest();
            showNeuronDisplayForm.ShowDialog();
        }

        private void displaySTDPTestBtn_Click(object sender, EventArgs e)
        {
            Form showStdpTestForm = new StdpTestForm();
            showStdpTestForm.ShowDialog();
        }

        private void learningWindowBtn_Click(object sender, EventArgs e)
        {
            Form showStdpLearningWindowForm = new StdpLearningWindow();
            showStdpLearningWindowForm.ShowDialog();
        }

        private void StdpTripletTestBtn_Click(object sender, EventArgs e)
        {
            Form showStdpTripletForm = new StdpPairProtocolForm();
            showStdpTripletForm.ShowDialog();
        }

        private void xorBtn_Click(object sender, EventArgs e)
        {
            Form showXortTestForm = new XorTest();
            showXortTestForm.ShowDialog();

        }

        private void NeuralEvolutionBtn_Click(object sender, EventArgs e)
        {
            Form evolutionTestForm = new NeuralEvolutionForm();
            evolutionTestForm.ShowDialog();
        }
    }
}
