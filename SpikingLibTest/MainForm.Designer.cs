using System.Diagnostics.Contracts;
namespace SpikingLibTest
{
    [ContractVerification(false)]
    partial class MainForm
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
            this.schedTestBtn = new System.Windows.Forms.Button();
            this.neuronSimBtn = new System.Windows.Forms.Button();
            this.firstNetBtn = new System.Windows.Forms.Button();
            this.neuronDisplayBtn = new System.Windows.Forms.Button();
            this.displaySTDPTestBtn = new System.Windows.Forms.Button();
            this.learningWindowBtn = new System.Windows.Forms.Button();
            this.StdpPairProtocolBtn = new System.Windows.Forms.Button();
            this.xorBtn = new System.Windows.Forms.Button();
            this.NeuralEvolBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.schedTestBtn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.neuronSimBtn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.firstNetBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.neuronDisplayBtn, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.displaySTDPTestBtn, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.learningWindowBtn, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.StdpPairProtocolBtn, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.xorBtn, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.NeuralEvolBtn, 0, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(158, 276);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // schedTestBtn
            // 
            this.schedTestBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedTestBtn.Location = new System.Drawing.Point(3, 3);
            this.schedTestBtn.Name = "schedTestBtn";
            this.schedTestBtn.Size = new System.Drawing.Size(152, 23);
            this.schedTestBtn.TabIndex = 0;
            this.schedTestBtn.Text = "Scheduler Test";
            this.schedTestBtn.UseVisualStyleBackColor = true;
            this.schedTestBtn.Click += new System.EventHandler(this.schedTestBtn_Click);
            // 
            // neuronSimBtn
            // 
            this.neuronSimBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neuronSimBtn.Location = new System.Drawing.Point(3, 32);
            this.neuronSimBtn.Name = "neuronSimBtn";
            this.neuronSimBtn.Size = new System.Drawing.Size(152, 23);
            this.neuronSimBtn.TabIndex = 2;
            this.neuronSimBtn.Text = "Neuron Simulator";
            this.neuronSimBtn.UseVisualStyleBackColor = true;
            this.neuronSimBtn.Click += new System.EventHandler(this.neuronSimBtn_Click);
            // 
            // firstNetBtn
            // 
            this.firstNetBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.firstNetBtn.Location = new System.Drawing.Point(3, 61);
            this.firstNetBtn.Name = "firstNetBtn";
            this.firstNetBtn.Size = new System.Drawing.Size(152, 23);
            this.firstNetBtn.TabIndex = 3;
            this.firstNetBtn.Text = "First Network";
            this.firstNetBtn.UseVisualStyleBackColor = true;
            this.firstNetBtn.Click += new System.EventHandler(this.firstNetBtn_Click);
            // 
            // neuronDisplayBtn
            // 
            this.neuronDisplayBtn.AutoSize = true;
            this.neuronDisplayBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.neuronDisplayBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neuronDisplayBtn.Location = new System.Drawing.Point(3, 119);
            this.neuronDisplayBtn.Name = "neuronDisplayBtn";
            this.neuronDisplayBtn.Size = new System.Drawing.Size(152, 23);
            this.neuronDisplayBtn.TabIndex = 6;
            this.neuronDisplayBtn.Text = "DisplayNeuronControl Test";
            this.neuronDisplayBtn.UseVisualStyleBackColor = true;
            this.neuronDisplayBtn.Click += new System.EventHandler(this.neuronDisplayBtn_Click);
            // 
            // displaySTDPTestBtn
            // 
            this.displaySTDPTestBtn.AutoSize = true;
            this.displaySTDPTestBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.displaySTDPTestBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displaySTDPTestBtn.Location = new System.Drawing.Point(3, 148);
            this.displaySTDPTestBtn.Name = "displaySTDPTestBtn";
            this.displaySTDPTestBtn.Size = new System.Drawing.Size(152, 23);
            this.displaySTDPTestBtn.TabIndex = 7;
            this.displaySTDPTestBtn.Text = "STDP Test";
            this.displaySTDPTestBtn.UseVisualStyleBackColor = true;
            this.displaySTDPTestBtn.Click += new System.EventHandler(this.displaySTDPTestBtn_Click);
            // 
            // learningWindowBtn
            // 
            this.learningWindowBtn.AutoSize = true;
            this.learningWindowBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.learningWindowBtn.Location = new System.Drawing.Point(3, 90);
            this.learningWindowBtn.Name = "learningWindowBtn";
            this.learningWindowBtn.Size = new System.Drawing.Size(152, 23);
            this.learningWindowBtn.TabIndex = 8;
            this.learningWindowBtn.Text = "STDP Learning Window";
            this.learningWindowBtn.UseVisualStyleBackColor = true;
            this.learningWindowBtn.Click += new System.EventHandler(this.learningWindowBtn_Click);
            // 
            // StdpPairProtocolBtn
            // 
            this.StdpPairProtocolBtn.AutoSize = true;
            this.StdpPairProtocolBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.StdpPairProtocolBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StdpPairProtocolBtn.Location = new System.Drawing.Point(3, 177);
            this.StdpPairProtocolBtn.Name = "StdpPairProtocolBtn";
            this.StdpPairProtocolBtn.Size = new System.Drawing.Size(152, 23);
            this.StdpPairProtocolBtn.TabIndex = 9;
            this.StdpPairProtocolBtn.Text = "STDP Pair Protocol Test";
            this.StdpPairProtocolBtn.UseVisualStyleBackColor = true;
            this.StdpPairProtocolBtn.Click += new System.EventHandler(this.StdpTripletTestBtn_Click);
            // 
            // xorBtn
            // 
            this.xorBtn.AutoSize = true;
            this.xorBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.xorBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xorBtn.Location = new System.Drawing.Point(3, 206);
            this.xorBtn.Name = "xorBtn";
            this.xorBtn.Size = new System.Drawing.Size(152, 23);
            this.xorBtn.TabIndex = 10;
            this.xorBtn.Text = "Xor Test (Not completed)";
            this.xorBtn.UseVisualStyleBackColor = true;
            this.xorBtn.Click += new System.EventHandler(this.xorBtn_Click);
            // 
            // NeuralEvolBtn
            // 
            this.NeuralEvolBtn.AutoSize = true;
            this.NeuralEvolBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.NeuralEvolBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NeuralEvolBtn.Location = new System.Drawing.Point(3, 235);
            this.NeuralEvolBtn.Name = "NeuralEvolBtn";
            this.NeuralEvolBtn.Size = new System.Drawing.Size(152, 38);
            this.NeuralEvolBtn.TabIndex = 11;
            this.NeuralEvolBtn.Text = "Neural Evolution Test";
            this.NeuralEvolBtn.UseVisualStyleBackColor = true;
            this.NeuralEvolBtn.Click += new System.EventHandler(this.NeuralEvolBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(158, 276);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button schedTestBtn;
        private System.Windows.Forms.Button neuronSimBtn;
        private System.Windows.Forms.Button firstNetBtn;
        private System.Windows.Forms.Button neuronDisplayBtn;
        private System.Windows.Forms.Button displaySTDPTestBtn;
        private System.Windows.Forms.Button learningWindowBtn;
        private System.Windows.Forms.Button StdpPairProtocolBtn;
        private System.Windows.Forms.Button xorBtn;
        private System.Windows.Forms.Button NeuralEvolBtn;
    }
}