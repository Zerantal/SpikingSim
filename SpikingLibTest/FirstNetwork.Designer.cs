using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    partial class FirstNetwork
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
            this.components = new System.ComponentModel.Container();
            this.zgc = new ZedGraph.ZedGraphControl();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.ThalamicNoiseCB = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zgc
            // 
            this.zgc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zgc.Location = new System.Drawing.Point(0, 0);
            this.zgc.Name = "zgc";
            this.zgc.ScrollGrace = 0;
            this.zgc.ScrollMaxX = 0;
            this.zgc.ScrollMaxY = 0;
            this.zgc.ScrollMaxY2 = 0;
            this.zgc.ScrollMinX = 0;
            this.zgc.ScrollMinY = 0;
            this.zgc.ScrollMinY2 = 0;
            this.zgc.Size = new System.Drawing.Size(941, 681);
            this.zgc.TabIndex = 0;
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(12, 21);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(75, 23);
            this.PauseBtn.TabIndex = 1;
            this.PauseBtn.Text = "Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(PauseBtn_Click);
            // 
            // ThalamicNoiseCB
            // 
            this.ThalamicNoiseCB.AutoSize = true;
            this.ThalamicNoiseCB.Checked = true;
            this.ThalamicNoiseCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThalamicNoiseCB.Location = new System.Drawing.Point(141, 25);
            this.ThalamicNoiseCB.Name = "ThalamicNoiseCB";
            this.ThalamicNoiseCB.Size = new System.Drawing.Size(122, 17);
            this.ThalamicNoiseCB.TabIndex = 2;
            this.ThalamicNoiseCB.Text = "Apply thalamic noise";
            this.ThalamicNoiseCB.UseVisualStyleBackColor = true;
            this.ThalamicNoiseCB.CheckedChanged += new System.EventHandler(this.ThalamicNoiseCB_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ThalamicNoiseCB);
            this.panel1.Controls.Add(this.PauseBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 687);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(941, 56);
            this.panel1.TabIndex = 3;
            // 
            // FirstNetwork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 743);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.zgc);
            this.Name = "FirstNetwork";
            this.Text = "FirstNetwork";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FirstNetwork_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zgc;
        private System.Windows.Forms.Button PauseBtn;
        private System.Windows.Forms.CheckBox ThalamicNoiseCB;
        private System.Windows.Forms.Panel panel1;
    }
}