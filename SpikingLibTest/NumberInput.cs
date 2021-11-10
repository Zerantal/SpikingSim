using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpikingLibTest
{
    public partial class NumberInput : Form
    {
        private int p;

        public NumberInput()
        {
            InitializeComponent();

            p = 0;
        }

        public int Priority
        {
            get { return p; }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            p = Convert.ToInt32(PriorityUpDown.Value);

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
