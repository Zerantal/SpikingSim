using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    partial class NeuronDisplayTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.neuronDisplay = new SpikingLibrary.NeuronDisplayControl();
            this.SuspendLayout();
            // 
            // neuronDisplay
            // 
            this.neuronDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neuronDisplay.Location = new System.Drawing.Point(0, 0);
            this.neuronDisplay.Name = "neuronDisplay";
            this.neuronDisplay.Size = new System.Drawing.Size(349, 303);
            this.neuronDisplay.TabIndex = 0;
            this.neuronDisplay.Target = null;
            // 
            // NeuronDisplayTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 303);
            this.Controls.Add(this.neuronDisplay);
            this.Name = "NeuronDisplayTest";
            this.Text = "NeuronDisplayTest";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NeuronDisplayTest_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private SpikingLibrary.NeuronDisplayControl neuronDisplay;
    }
}