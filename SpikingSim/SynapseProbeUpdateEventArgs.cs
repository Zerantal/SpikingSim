using System;

namespace SpikingLibrary
{
    public class SynapseProbeUpdateEventArgs : EventArgs
    {
        public SynapseProbeUpdateEventArgs(long time, double weight)
        {
            Weight = weight;
            Time = time;
        }

        public double Weight { get; }

        public long Time { get; }
    }
}
