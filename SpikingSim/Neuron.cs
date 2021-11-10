#region

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Threading;

#endregion

namespace SpikingLibrary
{
    /// <remarks>
    ///   Implements a Izhikevich neuron
    /// </remarks>
    public class Neuron
    {
        private static int _identifierCounter;
        
        private readonly List<Synapse> _axon;
        private readonly List<Synapse> _dendrite;

        private readonly int _id;
         
        private NeuronParameters _neuronType;
        private double _u;
        private double _v;

        public event EventHandler<NeuronFiredEventArgs> NeuronFired;
        private readonly SendOrPostCallback _onNeuronFiredDelegate;

        private readonly AsyncOperation _asyncOp;
        
        internal Neuron(NeuronParameters type)
        {
            // // Contract.Requires(type != null);
            
            _neuronType = type;
            _v = _neuronType.DefaultV;
            _u = _neuronType.DefaultU;
            _axon = new List<Synapse>();
            _dendrite = new List<Synapse>();

            // set neuron identifier
            _id = _identifierCounter;
            _identifierCounter++;
            
            _onNeuronFiredDelegate = ReportNeuronFiring;
            _asyncOp = AsyncOperationManager.CreateOperation(null);

        }

        internal NeuronParameters NeuronType
        {
            get
            {
                return _neuronType;
            }            
            set
            {
                // // Contract.Requires(value != null);
                _neuronType = value;
            }
        }

        internal double U
        {
            get { return _u; }

            set { _u = value; }
        }

        internal double V
        {
            get { return _v; }
            set { _v = value; }
        }

        public int Id
        {
            get { return _id; }
        }        

        internal void AddAxonalSynapse(Synapse synapse)
        {
            // // Contract.Requires(synapse != null);

            _axon.Add(synapse);
        }

        internal void AddDendriticSynapse(Synapse synapse)
        {
            // // Contract.Requires(synapse != null);

            _dendrite.Add(synapse);
        }

        public void ConnectTo(Neuron postsynapticNeuron, Synapse synapse)
        {
            // // Contract.Requires(postsynapticNeuron != null);
            // // Contract.Requires(synapse != null);

            SpikingNetEngine.ConnectNeurons(this, postsynapticNeuron, synapse);
        }

        // This method is invoked via the AsyncOperation object,
        // so it is guaranteed to be executed on the correct thread.
        private void ReportNeuronFiring(object state)
        {
            NeuronFiredEventArgs e = state as NeuronFiredEventArgs;

            OnNeuronFired(e);
        }

        protected void OnNeuronFired(NeuronFiredEventArgs e)
        {
            if (NeuronFired != null)
            {
                NeuronFired(this, e);
            }
        }

        internal void TriggerNeuronSpike(long time)
        {        
            // Backpropogated action potential
            foreach (Synapse s in _dendrite)            
                s.Bap(time); // performs weight updating on dendritic synapses            
            
            foreach (Synapse a in _axon)   // fan out action potential & perform weight updating on axonal synapses
                SpikingNetEngine.Scheduler.ScheduleEvent(a.ActivateSynapse, a.AxonalDelay);

            NeuronFiredEventArgs e = new NeuronFiredEventArgs(time);            

            _asyncOp.Post(_onNeuronFiredDelegate, e);
        }                

        /// <summary>
        /// Incrementally update neuron state to value at next time step. return true on neuron firing
        /// </summary>
        internal bool IncrementNeuronState()
        {
            return _neuronType.CalcNextState(ref _u, ref _v);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_axon != null);
            Contract.Invariant(_dendrite != null);
            Contract.Invariant(_neuronType != null);         
        }
    }
}