using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    partial class ScheduleTestForm
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
            this.testOutput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Button();
            this.AddEventBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maxTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.minTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.numEventsUpDown = new System.Windows.Forms.NumericUpDown();
            this.StopAsyncBtn = new System.Windows.Forms.Button();
            this.StartAsyncBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxTimeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minTimeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEventsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // testOutput
            // 
            this.testOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.testOutput.Location = new System.Drawing.Point(0, 0);
            this.testOutput.Multiline = true;
            this.testOutput.Name = "testOutput";
            this.testOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.testOutput.Size = new System.Drawing.Size(709, 321);
            this.testOutput.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Numbers of events:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Time Range:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "To";
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(9, 74);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 7;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // AddEventBtn
            // 
            this.AddEventBtn.Location = new System.Drawing.Point(12, 440);
            this.AddEventBtn.Name = "AddEventBtn";
            this.AddEventBtn.Size = new System.Drawing.Size(99, 23);
            this.AddEventBtn.TabIndex = 8;
            this.AddEventBtn.Text = "Add Event...";
            this.AddEventBtn.UseVisualStyleBackColor = true;
            this.AddEventBtn.Click += new System.EventHandler(this.AddEventBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maxTimeUpDown);
            this.groupBox1.Controls.Add(this.minTimeUpDown);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numEventsUpDown);
            this.groupBox1.Controls.Add(this.AddBtn);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 327);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 107);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add random events to scheduler";
            // 
            // maxTimeUpDown
            // 
            this.maxTimeUpDown.Location = new System.Drawing.Point(186, 48);
            this.maxTimeUpDown.Maximum = new decimal(new int[] {
            705032704,
            1,
            0,
            0});
            this.maxTimeUpDown.Name = "maxTimeUpDown";
            this.maxTimeUpDown.Size = new System.Drawing.Size(75, 20);
            this.maxTimeUpDown.TabIndex = 11;
            this.maxTimeUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // minTimeUpDown
            // 
            this.minTimeUpDown.Location = new System.Drawing.Point(80, 48);
            this.minTimeUpDown.Maximum = new decimal(new int[] {
            705032704,
            1,
            0,
            0});
            this.minTimeUpDown.Name = "minTimeUpDown";
            this.minTimeUpDown.Size = new System.Drawing.Size(70, 20);
            this.minTimeUpDown.TabIndex = 12;
            this.minTimeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numEventsUpDown
            // 
            this.numEventsUpDown.Location = new System.Drawing.Point(111, 19);
            this.numEventsUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numEventsUpDown.Name = "numEventsUpDown";
            this.numEventsUpDown.Size = new System.Drawing.Size(69, 20);
            this.numEventsUpDown.TabIndex = 10;
            this.numEventsUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // StopAsyncBtn
            // 
            this.StopAsyncBtn.Enabled = false;
            this.StopAsyncBtn.Location = new System.Drawing.Point(108, 564);
            this.StopAsyncBtn.Name = "StopAsyncBtn";
            this.StopAsyncBtn.Size = new System.Drawing.Size(99, 23);
            this.StopAsyncBtn.TabIndex = 10;
            this.StopAsyncBtn.Text = "Stop Scheduler";
            this.StopAsyncBtn.UseVisualStyleBackColor = true;
            this.StopAsyncBtn.Click += new System.EventHandler(this.StopAsyncBtn_Click);
            // 
            // StartAsyncBtn
            // 
            this.StartAsyncBtn.Location = new System.Drawing.Point(12, 564);
            this.StartAsyncBtn.Name = "StartAsyncBtn";
            this.StartAsyncBtn.Size = new System.Drawing.Size(90, 23);
            this.StartAsyncBtn.TabIndex = 2;
            this.StartAsyncBtn.Text = "Start Scheduler";
            this.StartAsyncBtn.UseVisualStyleBackColor = true;
            this.StartAsyncBtn.Click += new System.EventHandler(this.ExecuteAsyncBtn_Click);
            // 
            // ScheduleTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 599);
            this.Controls.Add(this.StopAsyncBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AddEventBtn);
            this.Controls.Add(this.StartAsyncBtn);
            this.Controls.Add(this.testOutput);
            this.Name = "ScheduleTestForm";
            this.Text = "Scheduler Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScheduleTestForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxTimeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minTimeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEventsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox testOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button AddEventBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown maxTimeUpDown;
        private System.Windows.Forms.NumericUpDown minTimeUpDown;
        private System.Windows.Forms.NumericUpDown numEventsUpDown;
        private System.Windows.Forms.Button StopAsyncBtn;
        private System.Windows.Forms.Button StartAsyncBtn;

    }
}

