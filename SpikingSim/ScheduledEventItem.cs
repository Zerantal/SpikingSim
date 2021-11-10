using System.Diagnostics.Contracts;

namespace SpikingLibrary
{
    public struct ScheduledEventItem
    {
        ScheduledEvent _schedEvent;
        int _timeInterval;

        public ScheduledEventItem(ScheduledEvent callback, int time)
        {
            // Contract.Requires(callback != null);
            // Contract.Requires(time >= 1);

            _schedEvent = callback;
            _timeInterval = time;            
        }

        public ScheduledEvent ScheduledEvent 
        {
            get
            {
                //Contract.Ensures(Contract.Result<ScheduledEvent>() != null);
                return _schedEvent;
            }
            set
            {
                // Contract.Requires(value != null);

                _schedEvent = value;
            }
        }

        public int TimeInterval 
        {
            get
            {
                //Contract.Ensures(Contract.Result<int>() >= 1);
                return _timeInterval;
            }
            set
            {
                // Contract.Requires(value >= 1);
                _timeInterval = value;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_schedEvent != null);
            Contract.Invariant(_timeInterval >= 1);
        }

    }
}
