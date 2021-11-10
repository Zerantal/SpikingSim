using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    partial class NeuralEvolutionForm
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
        [ContractVerification(false)]
        private void InitializeComponent()
        {
            this.mathPictureBox1 = new Wolfram.NETLink.UI.MathPictureBox();
            this.outputTB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mathPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mathPictureBox1
            // 
            this.mathPictureBox1.Link = null;
            this.mathPictureBox1.Location = new System.Drawing.Point(12, 244);
            this.mathPictureBox1.MathCommand = null;
            this.mathPictureBox1.Name = "mathPictureBox1";
            this.mathPictureBox1.PictureType = "Automatic";
            this.mathPictureBox1.Size = new System.Drawing.Size(709, 255);
            this.mathPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mathPictureBox1.TabIndex = 0;
            this.mathPictureBox1.TabStop = false;
            this.mathPictureBox1.UseFrontEnd = true;
            // 
            // outputTB
            // 
            this.outputTB.Location = new System.Drawing.Point(12, 12);
            this.outputTB.Multiline = true;
            this.outputTB.Name = "outputTB";
            this.outputTB.ReadOnly = true;
            this.outputTB.Size = new System.Drawing.Size(709, 226);
            this.outputTB.TabIndex = 1;
            // 
            // NeuralEvolutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 511);
            this.Controls.Add(this.outputTB);
            this.Controls.Add(this.mathPictureBox1);
            this.Name = "NeuralEvolutionForm";
            this.Text = "NeuralEvolutionForm";
            ((System.ComponentModel.ISupportInitialize)(this.mathPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wolfram.NETLink.UI.MathPictureBox mathPictureBox1;
        private System.Windows.Forms.TextBox outputTB;
    }
}