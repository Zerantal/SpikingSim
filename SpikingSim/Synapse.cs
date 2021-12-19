#region

using System;

#endregion

namespace SpikingLibrary
{
    // uses the triplet learning rule (Gerstner06)
    public class Synapse
    {
        private static int _idCounter;

        private long _lastStdpUpdateTime = int.MinValue;

        private readonly StdpParameters _stdpParams;

        // stdp variables
        private double _r1, _r2, _o1, _o2;        

        public Synapse(int delay, double efficacy, StdpParameters stdpParameters)
        {            
            // Contract.Requires(delay >= 1);
            // Contract.Requires(stdpParameters != null);

            AxonalDelay = delay;         
            Id = _idCounter++;
            Weight = efficacy;

            _stdpParams = stdpParameters;            
        }
        
        public Neuron PostsynapticNeuron { get; internal set; }

        public int AxonalDelay { get; internal set; }

        public double Weight { get; internal set; }

        internal int Id { get; }


        internal virtual void ActivateSynapse(long time)
        {
            // Contract.Requires(PostsynapticNeuron != null);

            double t = time - _lastStdpUpdateTime;            

            // resolve r1, r2, and o1;
            _r1 *= Math.Exp(-t/_stdpParams.TauPositive);
            _r2 *= Math.Exp(-t/_stdpParams.TauX);
            _o1 *= Math.Exp(-t/_stdpParams.TauNegative);
            _o2 *= Math.Exp(-t/_stdpParams.TauY);

//            Debug.WriteLine("LTD: " + t + ", " + _r1 + ", " + _r2 + ", " + _o1 + ", " + _o2 + ", " +
//                Weight + ", " + -_o1 * (_stdpParams.A2Negative + _stdpParams.A3Negative * _r2), "Synapse");

            Weight -= _o1*(_stdpParams.A2Negative + _stdpParams.A3Negative*_r2);
            _r1++;
            _r2++;

            _lastStdpUpdateTime = time;

            PostsynapticNeuron.V += Weight;            
        }

        internal virtual void Bap(long time)
        {
            double t = time - _lastStdpUpdateTime;

            // resolve r1, r2, and o1;
            _r1 *= Math.Exp(-t / _stdpParams.TauPositive);
            _r2 *= Math.Exp(-t / _stdpParams.TauX);
            _o1 *= Math.Exp(-t / _stdpParams.TauNegative);
            _o2 *= Math.Exp(-t / _stdpParams.TauY);

//            Debug.WriteLine("LTP: " + t + ", " + _r1 + ", " + _r2 + ", " + _o1 + ", " + _o2 + ", " +
//                Weight + ", " + _r1 * (_stdpParams.A2Positive + _stdpParams.A3Positive * _o2), "Synapse");

            Weight += _r1 * (_stdpParams.A2Positive + _stdpParams.A3Positive * _o2);
            _o1++;            
            _o2++;

            _lastStdpUpdateTime = time;                   
        }
    }
}