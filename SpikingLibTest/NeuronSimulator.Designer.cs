using ZedGraph;
using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    partial class NeuronSimulator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <parameters neuronName="disposing">true if managed resources should be disposed; otherwise, false.</parameters>
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
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.biasCurrentUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.bParamUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.dParamUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cParamUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.aParamUpDown = new System.Windows.Forms.NumericUpDown();
            this.ExcPulseBtn = new System.Windows.Forms.Button();
            this.InhibPulseBtn = new System.Windows.Forms.Button();
            this.SetPointBtn = new System.Windows.Forms.Button();
            this.SpeedSlider = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.quitBtn = new System.Windows.Forms.Button();
            this.paramCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.biasCurrentUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bParamUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dParamUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cParamUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aParamUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // zg1
            // 
            this.zg1.Dock = System.Windows.Forms.DockStyle.Top;
            this.zg1.Location = new System.Drawing.Point(0, 0);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(1066, 521);
            this.zg1.TabIndex = 0;
            this.zg1.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(this.Zg1ZoomEvent);
            this.zg1.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.Zg1MouseDownEvent);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.biasCurrentUpDown);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.bParamUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dParamUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cParamUpDown);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.aParamUpDown);
            this.groupBox1.Location = new System.Drawing.Point(12, 527);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 187);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // biasCurrentUpDown
            // 
            this.biasCurrentUpDown.DecimalPlaces = 3;
            this.biasCurrentUpDown.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.biasCurrentUpDown.Location = new System.Drawing.Point(38, 124);
            this.biasCurrentUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.biasCurrentUpDown.Name = "biasCurrentUpDown";
            this.biasCurrentUpDown.Size = new System.Drawing.Size(86, 20);
            this.biasCurrentUpDown.TabIndex = 9;
            this.biasCurrentUpDown.ValueChanged += new System.EventHandler(this.BiasCurrentUpDownValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "I = ";
            // 
            // bParamUpDown
            // 
            this.bParamUpDown.DecimalPlaces = 3;
            this.bParamUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.bParamUpDown.Location = new System.Drawing.Point(38, 46);
            this.bParamUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.bParamUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.bParamUpDown.Name = "bParamUpDown";
            this.bParamUpDown.Size = new System.Drawing.Size(86, 20);
            this.bParamUpDown.TabIndex = 10;
            this.bParamUpDown.ValueChanged += new System.EventHandler(this.BParamUpDownValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "d = ";
            // 
            // dParamUpDown
            // 
            this.dParamUpDown.DecimalPlaces = 3;
            this.dParamUpDown.Location = new System.Drawing.Point(38, 98);
            this.dParamUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.dParamUpDown.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.dParamUpDown.Name = "dParamUpDown";
            this.dParamUpDown.Size = new System.Drawing.Size(86, 20);
            this.dParamUpDown.TabIndex = 8;
            this.dParamUpDown.ValueChanged += new System.EventHandler(this.DParamUpDownValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "c = ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "b = ";
            // 
            // cParamUpDown
            // 
            this.cParamUpDown.DecimalPlaces = 3;
            this.cParamUpDown.Location = new System.Drawing.Point(38, 72);
            this.cParamUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.cParamUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.cParamUpDown.Name = "cParamUpDown";
            this.cParamUpDown.Size = new System.Drawing.Size(86, 20);
            this.cParamUpDown.TabIndex = 7;
            this.cParamUpDown.ValueChanged += new System.EventHandler(this.CParamUpDownValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "a = ";
            // 
            // aParamUpDown
            // 
            this.aParamUpDown.DecimalPlaces = 3;
            this.aParamUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.aParamUpDown.Location = new System.Drawing.Point(38, 20);
            this.aParamUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.aParamUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.aParamUpDown.Name = "aParamUpDown";
            this.aParamUpDown.Size = new System.Drawing.Size(86, 20);
            this.aParamUpDown.TabIndex = 6;
            this.aParamUpDown.ValueChanged += new System.EventHandler(this.AParamUpDownValueChanged);
            // 
            // ExcPulseBtn
            // 
            this.ExcPulseBtn.Location = new System.Drawing.Point(206, 541);
            this.ExcPulseBtn.Name = "ExcPulseBtn";
            this.ExcPulseBtn.Size = new System.Drawing.Size(101, 23);
            this.ExcPulseBtn.TabIndex = 3;
            this.ExcPulseBtn.Text = "Excitatory pulse";
            this.ExcPulseBtn.UseVisualStyleBackColor = true;
            this.ExcPulseBtn.Click += new System.EventHandler(this.ExcPulseBtnClick);
            // 
            // InhibPulseBtn
            // 
            this.InhibPulseBtn.Location = new System.Drawing.Point(206, 569);
            this.InhibPulseBtn.Name = "InhibPulseBtn";
            this.InhibPulseBtn.Size = new System.Drawing.Size(101, 23);
            this.InhibPulseBtn.TabIndex = 4;
            this.InhibPulseBtn.Text = "Inhibitory Pulse";
            this.InhibPulseBtn.UseVisualStyleBackColor = true;
            this.InhibPulseBtn.Click += new System.EventHandler(this.InhibitoryPulseBtnClick);
            // 
            // SetPointBtn
            // 
            this.SetPointBtn.Location = new System.Drawing.Point(206, 612);
            this.SetPointBtn.Name = "SetPointBtn";
            this.SetPointBtn.Size = new System.Drawing.Size(101, 23);
            this.SetPointBtn.TabIndex = 5;
            this.SetPointBtn.Text = "Set Initial Point";
            this.SetPointBtn.UseVisualStyleBackColor = true;
            this.SetPointBtn.Click += new System.EventHandler(this.SetPointBtnClick);
            // 
            // SpeedSlider
            // 
            this.SpeedSlider.LargeChange = 5000;
            this.SpeedSlider.Location = new System.Drawing.Point(206, 669);
            this.SpeedSlider.Maximum = 20000;
            this.SpeedSlider.Minimum = 2000;
            this.SpeedSlider.Name = "SpeedSlider";
            this.SpeedSlider.Size = new System.Drawing.Size(288, 45);
            this.SpeedSlider.SmallChange = 1000;
            this.SpeedSlider.TabIndex = 6;
            this.SpeedSlider.TickFrequency = 1000;
            this.SpeedSlider.Value = 10000;
            this.SpeedSlider.Scroll += new System.EventHandler(this.SpeedSliderScroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(288, 653);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Simulation Speed";
            // 
            // pauseBtn
            // 
            this.pauseBtn.Location = new System.Drawing.Point(379, 541);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(75, 23);
            this.pauseBtn.TabIndex = 8;
            this.pauseBtn.Text = "Pause";
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Click += new System.EventHandler(this.PauseBtnClick);
            // 
            // quitBtn
            // 
            this.quitBtn.Location = new System.Drawing.Point(379, 570);
            this.quitBtn.Name = "quitBtn";
            this.quitBtn.Size = new System.Drawing.Size(75, 23);
            this.quitBtn.TabIndex = 9;
            this.quitBtn.Text = "Quit";
            this.quitBtn.UseVisualStyleBackColor = true;
            this.quitBtn.Click += new System.EventHandler(this.QuitBtnClick);
            // 
            // paramCB
            // 
            this.paramCB.FormattingEnabled = true;
            this.paramCB.Location = new System.Drawing.Point(546, 559);
            this.paramCB.Name = "paramCB";
            this.paramCB.Size = new System.Drawing.Size(180, 21);
            this.paramCB.TabIndex = 10;
            this.paramCB.SelectedIndexChanged += new System.EventHandler(this.ParamCbSelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(543, 543);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Load parameter set";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(546, 604);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(179, 110);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Neuron Model:\r\n\r\nv\' = 0.04v^2 + 5v +140 - u + I\r\nu\' = a(bv - u)\r\n\r\nif (v > 30 mV)" +
                "\r\nthen v = c, u = u + d";
            // 
            // NeuronSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 737);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.paramCB);
            this.Controls.Add(this.quitBtn);
            this.Controls.Add(this.pauseBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SpeedSlider);
            this.Controls.Add(this.SetPointBtn);
            this.Controls.Add(this.InhibPulseBtn);
            this.Controls.Add(this.ExcPulseBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.zg1);
            this.Name = "NeuronSimulator";
            this.Text = "NeuronSimulator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NeuronSimulatorFormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.biasCurrentUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bParamUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dParamUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cParamUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aParamUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown bParamUpDown;
        private System.Windows.Forms.NumericUpDown biasCurrentUpDown;
        private System.Windows.Forms.NumericUpDown dParamUpDown;
        private System.Windows.Forms.NumericUpDown cParamUpDown;
        private System.Windows.Forms.NumericUpDown aParamUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExcPulseBtn;
        private System.Windows.Forms.Button InhibPulseBtn;
        private System.Windows.Forms.Button SetPointBtn;
        private System.Windows.Forms.TrackBar SpeedSlider;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.Button quitBtn;
        private System.Windows.Forms.ComboBox paramCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private GraphPane _vPane;
    }
}