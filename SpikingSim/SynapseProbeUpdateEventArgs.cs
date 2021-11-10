using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingLibrary
{
    public class SynapseProbeUpdateEventArgs : EventArgs
    {
        public SynapseProbeUpdateEventArgs(long time, double weight)
        {
            Weight = weight;
            Time = time;
        }

        public double Weight { get; private set; }

        public long Time { get; private set; }
    }
}
