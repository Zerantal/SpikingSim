using System;

namespace SpikingLibrary
{
    public class NeuronProbeUpdateEventArgs : EventArgs
    {
        public double U { get; private set; }
        public double V { get; private set; }
        public long Time { get; private set; }

        public NeuronProbeUpdateEventArgs(long time, double u, double v)
        {
            U = u;
            V = v;
            Time = time;
        }
    }
}
