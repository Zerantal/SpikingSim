namespace SpikingLibrary
{
    public struct ScheduledEventItem
    {
        public ScheduledEventItem(ScheduledEvent callback, int time)
        {
            // Contract.Requires(callback != null);
            // Contract.Requires(time >= 1);

            ScheduledEvent = callback;
            TimeInterval = time;            
        }

        public ScheduledEvent ScheduledEvent { get; set; }

        public int TimeInterval { get; set; }
    }
}
