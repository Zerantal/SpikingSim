#region

using System;
using System.Diagnostics.Contracts;
using System.Diagnostics;

#endregion

namespace SpikingLibrary
{
    // uses the triplet learning rule (Gerstner06)
    public class Synapse
    {
        private static int _idCounter;
        private readonly int _synapseId;

        private int _axonalDelay; // in units of 0.1 ms
        private Neuron _postSynapticNeuron; // post synaptic neuron    

        private long _lastStdpUpdateTime = int.MinValue;

        private StdpParameters _stdpParams;

        // stdp variables
        private double _r1, _r2, _o1, _o2;        

        public Synapse(int delay, double efficacy, StdpParameters stdpParameters)
        {            
            // Contract.Requires(delay >= 1);
            // Contract.Requires(stdpParameters != null);

            _axonalDelay = delay;         
            _synapseId = _idCounter++;
            Weight = efficacy;

            _stdpParams = stdpParameters;            
        }
        
        public Neuron PostsynapticNeuron
        {
            get
            {
                return _postSynapticNeuron;
            }
            internal set
            {
                // Contract.Requires(value != null);
                _postSynapticNeuron = value;
            }
        }

        public int AxonalDelay
        {
            get
            {
                //Contract.Ensures(Contract.Result<int>() >= 1);
                return _axonalDelay;
            }  
            internal set
            {
                // Contract.Requires(value >= 1);
                _axonalDelay = value;
            }
        }

        public double Weight { get; internal set; }

        internal int Id
        {
            get { return _synapseId; }
        }

        
        internal virtual void ActivateSynapse(long time)
        {
            // Contract.Requires(PostsynapticNeuron != null);

            double t = time - _lastStdpUpdateTime;            

            // resolve r1, r2, and o1;
            _r1 = _r1*Math.Exp(-t/_stdpParams.TauPositive);
            _r2 = _r2*Math.Exp(-t/_stdpParams.TauX);
            _o1 = _o1*Math.Exp(-t/_stdpParams.TauNegative);
            _o2 = _o2*Math.Exp(-t/_stdpParams.TauY);

//            Debug.WriteLine("LTD: " + t + ", " + _r1 + ", " + _r2 + ", " + _o1 + ", " + _o2 + ", " +
//                Weight + ", " + -_o1 * (_stdpParams.A2Negative + _stdpParams.A3Negative * _r2), "Synapse");

            Weight -= _o1*(_stdpParams.A2Negative + _stdpParams.A3Negative*_r2);
            _r1++;
            _r2++;

            _lastStdpUpdateTime = time;

            _postSynapticNeuron.V += Weight;            
        }

        internal virtual void Bap(long time)
        {
            double t = time - _lastStdpUpdateTime;

            // resolve r1, r2, and o1;
            _r1 = _r1 * Math.Exp(-t / _stdpParams.TauPositive);
            _r2 = _r2 * Math.Exp(-t / _stdpParams.TauX);
            _o1 = _o1 * Math.Exp(-t / _stdpParams.TauNegative);
            _o2 = _o2 * Math.Exp(-t / _stdpParams.TauY);

//            Debug.WriteLine("LTP: " + t + ", " + _r1 + ", " + _r2 + ", " + _o1 + ", " + _o2 + ", " +
//                Weight + ", " + _r1 * (_stdpParams.A2Positive + _stdpParams.A3Positive * _o2), "Synapse");

            Weight += _r1 * (_stdpParams.A2Positive + _stdpParams.A3Positive * _o2);
            _o1++;            
            _o2++;

            _lastStdpUpdateTime = time;                   
        }        

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_axonalDelay >= 1);
        }
    }
}