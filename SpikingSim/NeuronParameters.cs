#region

using System;

using Util;
#endregion

namespace SpikingLibrary
{
    public class NeuronParameters : IDeepCloneable<NeuronParameters>
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly double _d;
        private readonly double _I; // bias current

        private readonly string _label;

        internal const double ResetThreshold = 30;   //mV

        public NeuronParameters() : this(0.02, 0.25, -65, 6, 0.5, "Phasic Spiking")
        {
            // set parameters for basic phasic spiking neuron            
        }

        public NeuronParameters(double a, double b, double c, double d, double I)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _I = I;

            double tmp = (5 - b) * (5 - b) - 4 * 0.04 * (140 + I);

            if (tmp > 0)
            {
                if (a < 0)
                    DefaultV = (-5 + b + Math.Sqrt(tmp)) / 2 / 0.04;
                else
                    DefaultV = (-5 + b - Math.Sqrt(tmp)) / 2 / 0.04;


                DefaultU = DefaultV * b;
            }
            else // trajectory doesn't converge: Use minimum v such that v' = 0
            {
                DefaultV = -62.5;
                DefaultU = 0.04 * DefaultV * DefaultV + 5 * DefaultV + 140 + I;
            }  
        }

        public NeuronParameters(double a, double b, double c, double d, double I, string className)
            : this(a, b, c, d, I)
        {
            _label = className;
        }

        public string Label
        {
            get { return _label; }
        }

        public double A { get { return _a; } }

        public double B { get { return _b; } }

        public double C { get { return _c; } }

        public double D { get { return _d; } }

        public double I { get { return _I; } }

        internal double DefaultU { get; private set; }

        internal double DefaultV { get; private set; }
        
        internal double Du(double u, double v)
        {
            return (_a*(_b*v - u));
        }

        internal double Dv(double u, double v)
        {
            return (0.04*v*v + 5*v + 140 - u + _I);
        }
                     
        // return true if spike is triggered, false otherwise
        internal bool CalcNextState(ref double u, ref double v)
        {
            double uNew = u + Du(u, v)*SpikingNetEngine.Dt;
            double vNew = v + Dv(u, v)*SpikingNetEngine.Dt;

            u = uNew;
            v = vNew;

            if (v >= ResetThreshold)
            {
                v = _c;
                u += _d;
                return true;
            }
            return false;
        }

        public static NeuronParameters TonicSpiking { get { return new NeuronParameters(0.02, 0.2, -65, 6, 14, "Tonic Spiking"); }}
        public static NeuronParameters PhasicSpiking { get { return new NeuronParameters(0.02, 0.25, -65, 6, 0.5, "Phasic Spiking"); }}
        public static NeuronParameters TonicBursting { get { return new NeuronParameters(0.02, 0.2, -50, 2, 15, "Tonic Bursting"); }}
        public static NeuronParameters PhasicBursting { get { return new NeuronParameters(0.02, 0.25, -55, 0.05, 0.6, "Phasic Bursting"); }}
        public static NeuronParameters MixedMode { get { return new NeuronParameters(0.02, 0.2, -55, 4, 10, "Mixed Mode"); }}
        public static NeuronParameters SpikeFreqAdaptation { get { return new NeuronParameters(0.01, 0.2, -65, 8, 30, "Spike Frequency Adaptation"); }}
        public static NeuronParameters SubthresholdOscillation { get { return new NeuronParameters(0.05, 0.26, -60, 0, 0, "Subthreshold Oscillations"); }}
        public static NeuronParameters Class1 { get { return new NeuronParameters(0.02, -0.1, -55, 6, 0, "Class 1"); }}
        public static NeuronParameters Class2 { get { return new NeuronParameters(0.2, 0.26, -65, 0, 0, "Class 2"); }}
        public static NeuronParameters SpikeLatency { get { return new NeuronParameters(0.02, 0.2, -65, 6, 7, "Spike Latency"); }}
        public static NeuronParameters Resonator { get { return new NeuronParameters(0.1, 0.26, -60, -1, 0, "Resonator"); }}
        public static NeuronParameters ReboundSpike { get { return new NeuronParameters(0.03, 0.25, -60, 4, 0, "ReboundSpike"); }}
        public static NeuronParameters ReboundBurst { get { return new NeuronParameters(0.03, 0.25, -52, 0, 0, "ReboundBurst"); }}
        public static NeuronParameters ThresholdVariability { get { return new NeuronParameters(0.03, 0.25, -60, 4, 0, "Threshold Variability"); }}
        public static NeuronParameters Bistability { get { return new NeuronParameters(1, 1.5, -60, 0, -67, "Bistability"); }}
        public static NeuronParameters DAP { get { return new NeuronParameters(1, 0.2, -60, -21, 0, "DAP"); }}
        public static NeuronParameters Accommodation { get { return new NeuronParameters(0.02, 1, -55, 4, 0, "Accommodation"); }}
        public static NeuronParameters InhibitionInducedSpiking { get { return new NeuronParameters(-0.02, -1, -60, 8, 80, "Inhibition-Induced Spiking"); }}
        public static NeuronParameters InhibitionInducedBursting { get { return new NeuronParameters(-0.026, -1, -45, 0, 80, "Inhibition-Induced Bursting"); }}
        public static NeuronParameters Integrator { get { return new NeuronParameters(0.02, -0.1, -55, 6, 0, "Integrator"); }}

        #region Implementation of IDeepCloneable<out NeuronParameters>

        public NeuronParameters DeepClone()
        {
            return new NeuronParameters(_a, _b, _c, _d, _I, _label);            
        }

        #endregion
    }
}