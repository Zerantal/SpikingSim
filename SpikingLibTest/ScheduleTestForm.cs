using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using SpikingLibrary;

namespace SpikingLibTest
{
    public partial class ScheduleTestForm : Form
    {
        private Scheduler s;

        public ScheduleTestForm()
        {
            InitializeComponent();

            s = new Scheduler(10000);                                 
        }

        private void PrintPriority(long time)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ScheduledEvent(PrintPriority), time);
            }
            else
            {

                testOutput.AppendText(time + " ");
                Console.Write(time + " ");
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int p;
            int tmin = Convert.ToInt32(minTimeUpDown.Value);
            int priorityRange = Convert.ToInt32(maxTimeUpDown.Value - minTimeUpDown.Value);            
            List<ScheduledEventItem> eventList = new List<ScheduledEventItem>(Convert.ToInt32(numEventsUpDown.Value));

            testOutput.AppendText(Environment.NewLine);
            testOutput.AppendText(numEventsUpDown.Value + " random events have been added to the scheduler " +
                "with the following priorities: ");
            for (int i = 0; i < numEventsUpDown.Value; i++)
            {
                p = r.Next(priorityRange) + Convert.ToInt32(minTimeUpDown.Value);
                eventList.Add(new ScheduledEventItem(new ScheduledEvent(PrintPriority), p));                
                testOutput.AppendText(p + " ");
            }
            s.ScheduleEvent(new Collection<ScheduledEventItem>(eventList));       
            testOutput.AppendText(Environment.NewLine);
        }

        private void AddEventBtn_Click(object sender, EventArgs e)
        {
            NumberInput priorityDlgBox = new NumberInput();

            testOutput.AppendText(Environment.NewLine);
            if (priorityDlgBox.ShowDialog() == DialogResult.OK)
            {
                testOutput.AppendText("Event added to schedule with priority: " + priorityDlgBox.Priority);
                s.ScheduleEvent(new ScheduledEvent(PrintPriority), priorityDlgBox.Priority);
            }
            testOutput.AppendText(Environment.NewLine);
                        
        }        

        private void ExecuteAsyncBtn_Click(object sender, EventArgs e)
        {            
            StopAsyncBtn.Enabled = true;
            StartAsyncBtn.Enabled = false;
            s.StartAsync("Scheduler Test");
        }

        private void StopAsyncBtn_Click(object sender, EventArgs e)
        {            
            s.Stop();
            while (s.IsRunning) ;            
            StopAsyncBtn.Enabled = false;
            StartAsyncBtn.Enabled = true;
        }

        private void ScheduleTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            s.Stop();
            while (s.IsRunning) ;
        }


    }
}
