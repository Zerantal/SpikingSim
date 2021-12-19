using System;
using System.Windows.Forms;

namespace SpikingLibTest
{
    public partial class NumberInput : Form
    {
        public NumberInput()
        {
            InitializeComponent();

            Priority = 0;
        }

        public int Priority { get; private set; }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Priority = Convert.ToInt32(PriorityUpDown.Value);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
