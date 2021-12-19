#region

using System;

using Util;
#endregion

namespace SpikingLibrary
{
    public class NeuronParameters : IDeepCloneable<NeuronParameters>
    {
        // ReSharper disable once InconsistentNaming

        internal const double ResetThreshold = 30;   //mV

        public NeuronParameters() : this(0.02, 0.25, -65, 6, 0.5, "Phasic Spiking")
        {
            // set parameters for basic phasic spiking neuron            
        }

        public NeuronParameters(double a, double b, double c, double d, double I)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            this.I = I;

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
            Label = className;
        }

        public string Label { get; }

        public double A { get; }

        public double B { get; }

        public double C { get; }

        public double D { get; }

        public double I { get; }

        internal double DefaultU { get; }

        internal double DefaultV { get; }
        
        internal double Du(double u, double v)
        {
            return A*(B*v - u);
        }

        internal double Dv(double u, double v)
        {
            return 0.04*v*v + 5*v + 140 - u + I;
        }
                     
        // return true if spike is triggered, false otherwise
        internal bool CalcNextState(ref double u, ref double v)
        {
            double uNew = u + Du(u, v)*SpikingNetEngine.Dt;
            double vNew = v + Dv(u, v)*SpikingNetEngine.Dt;

            u = uNew;
            v = vNew;

            if (!(v >= ResetThreshold)) return false;
            v = C;
            u += D;
            return true;
        }

        public static NeuronParameters TonicSpiking => new NeuronParameters(0.02, 0.2, -65, 6, 14, "Tonic Spiking");
        public static NeuronParameters PhasicSpiking => new NeuronParameters(0.02, 0.25, -65, 6, 0.5, "Phasic Spiking");
        public static NeuronParameters TonicBursting => new NeuronParameters(0.02, 0.2, -50, 2, 15, "Tonic Bursting");
        public static NeuronParameters PhasicBursting => new NeuronParameters(0.02, 0.25, -55, 0.05, 0.6, "Phasic Bursting");
        public static NeuronParameters MixedMode => new NeuronParameters(0.02, 0.2, -55, 4, 10, "Mixed Mode");
        public static NeuronParameters SpikeFrequencyAdaptation => new NeuronParameters(0.01, 0.2, -65, 8, 30, "Spike Frequency Adaptation");
        public static NeuronParameters SubthresholdOscillation => new NeuronParameters(0.05, 0.26, -60, 0, 0, "Subthreshold Oscillations");
        public static NeuronParameters Class1 => new NeuronParameters(0.02, -0.1, -55, 6, 0, "Class 1");
        public static NeuronParameters Class2 => new NeuronParameters(0.2, 0.26, -65, 0, 0, "Class 2");
        public static NeuronParameters SpikeLatency => new NeuronParameters(0.02, 0.2, -65, 6, 7, "Spike Latency");
        public static NeuronParameters Resonator => new NeuronParameters(0.1, 0.26, -60, -1, 0, "Resonator");
        public static NeuronParameters ReboundSpike => new NeuronParameters(0.03, 0.25, -60, 4, 0, "ReboundSpike");
        public static NeuronParameters ReboundBurst => new NeuronParameters(0.03, 0.25, -52, 0, 0, "ReboundBurst");
        public static NeuronParameters ThresholdVariability => new NeuronParameters(0.03, 0.25, -60, 4, 0, "Threshold Variability");
        public static NeuronParameters Bistability => new NeuronParameters(1, 1.5, -60, 0, -67, "Bistability");
        // ReSharper disable once InconsistentNaming
        public static NeuronParameters DAP => new NeuronParameters(1, 0.2, -60, -21, 0, "DAP");
        public static NeuronParameters Accommodation => new NeuronParameters(0.02, 1, -55, 4, 0, "Accommodation");
        public static NeuronParameters InhibitionInducedSpiking => new NeuronParameters(-0.02, -1, -60, 8, 80, "Inhibition-Induced Spiking");
        public static NeuronParameters InhibitionInducedBursting => new NeuronParameters(-0.026, -1, -45, 0, 80, "Inhibition-Induced Bursting");
        public static NeuronParameters Integrator => new NeuronParameters(0.02, -0.1, -55, 6, 0, "Integrator");

        #region Implementation of IDeepCloneable<out NeuronParameters>

        public NeuronParameters DeepClone()
        {
            return new NeuronParameters(A, B, C, D, I, Label);            
        }

        #endregion
    }
}