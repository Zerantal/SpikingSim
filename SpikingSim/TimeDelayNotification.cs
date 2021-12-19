using System;
using System.Threading;
using System.ComponentModel;

namespace SpikingLibrary
{
    public class TimeDelayNotification
    {
        public event EventHandler Notification;
        private readonly SendOrPostCallback _onNotificationDelegate;

        private readonly AsyncOperation _asyncOp;        

        public TimeDelayNotification()
        {
            _onNotificationDelegate = ReportNotification;
            _asyncOp = AsyncOperationManager.CreateOperation(null);
        }

        public void CreateNotification(int time)
        {
            // Contract.Requires(time > 0);
            SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_NotifyClient, time);
        } 

        private void Scheduler_NotifyClient(long time)
        {
            _asyncOp.Post(_onNotificationDelegate, EventArgs.Empty);
        }

        private void ReportNotification(object state)
        {           
            OnNotification(EventArgs.Empty);
        }

        protected void OnNotification(EventArgs e)
        {
            Notification?.Invoke(this, e);
        }
    }
}
