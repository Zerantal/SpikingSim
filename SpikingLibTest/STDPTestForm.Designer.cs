using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    partial class StdpTestForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.freqUD = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.spikeIntervalUD = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.stopBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.synapseDisplayControl1 = new SpikingLibrary.SynapseDisplayControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.freqUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spikeIntervalUD)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.synapseDisplayControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.14484F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.85515F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(407, 359);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.freqUD);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.spikeIntervalUD);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.stopBtn);
            this.panel1.Controls.Add(this.startBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 262);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(401, 94);
            this.panel1.TabIndex = 1;
            // 
            // freqUD
            // 
            this.freqUD.Location = new System.Drawing.Point(306, 31);
            this.freqUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.freqUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.freqUD.Name = "freqUD";
            this.freqUD.Size = new System.Drawing.Size(86, 20);
            this.freqUD.TabIndex = 9;
            this.freqUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Frequency";
            // 
            // spikeIntervalUD
            // 
            this.spikeIntervalUD.Location = new System.Drawing.Point(306, 5);
            this.spikeIntervalUD.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.spikeIntervalUD.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.spikeIntervalUD.Name = "spikeIntervalUD";
            this.spikeIntervalUD.Size = new System.Drawing.Size(86, 20);
            this.spikeIntervalUD.TabIndex = 7;
            this.spikeIntervalUD.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Δt";
            // 
            // stopBtn
            // 
            this.stopBtn.Enabled = false;
            this.stopBtn.Location = new System.Drawing.Point(90, 63);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(75, 23);
            this.stopBtn.TabIndex = 1;
            this.stopBtn.Text = "stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(9, 63);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // synapseDisplayControl1
            // 
            this.synapseDisplayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.synapseDisplayControl1.Location = new System.Drawing.Point(3, 3);
            this.synapseDisplayControl1.Name = "synapseDisplayControl1";
            this.synapseDisplayControl1.Size = new System.Drawing.Size(401, 253);
            this.synapseDisplayControl1.TabIndex = 0;
            this.synapseDisplayControl1.Target = null;
            // 
            // STDPTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 359);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StdpTestForm";
            this.Text = "STDPTestForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.STDPTestForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.freqUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spikeIntervalUD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private SpikingLibrary.SynapseDisplayControl synapseDisplayControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.NumericUpDown spikeIntervalUD;
        private System.Windows.Forms.NumericUpDown freqUD;
        private System.Windows.Forms.Label label1;
    }
}