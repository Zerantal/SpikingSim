using System;

namespace SpikingLibrary
{
    public class NeuronProbeUpdateEventArgs : EventArgs
    {
        public double U { get; }
        public double V { get; }
        public long Time { get; }

        public NeuronProbeUpdateEventArgs(long time, double u, double v)
        {
            U = u;
            V = v;
            Time = time;
        }
    }
}
