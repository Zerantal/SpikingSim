using System;

namespace SpikingLibrary
{
    public class NeuronFiredEventArgs : EventArgs
    {
        public NeuronFiredEventArgs(long time)
        {
            Time = time;
        }

        public long Time { get; }
    }
}
