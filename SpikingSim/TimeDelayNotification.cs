using System;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace SpikingLibrary
{
    public class TimeDelayNotification
    {
        public event EventHandler Notification;
        private readonly SendOrPostCallback _onNotificationDelegate;

        private readonly AsyncOperation _asyncOp;        

        public TimeDelayNotification()
        {
            _onNotificationDelegate = new SendOrPostCallback(ReportNotification);
            _asyncOp = AsyncOperationManager.CreateOperation(null);
        }

        public void CreateNotification(int time)
        {
            // Contract.Requires(time > 0);
            SpikingNetEngine.Scheduler.ScheduleEvent(Sched_NotifyClient, time);
        } 

        private void Sched_NotifyClient(long time)
        {
            _asyncOp.Post(_onNotificationDelegate, new EventArgs());
        }

        private void ReportNotification(object state)
        {           
            OnNotification(new EventArgs());
        }

        protected void OnNotification(EventArgs e)
        {
            if (Notification != null)
            {
                Notification(this, e);                
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_asyncOp != null);
        }
    }
}
