using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Util;

namespace SpikingLibrary
{
  
    [Serializable]
    public class NeuronCollection : IEnumerable<Neuron>, IShallowCloneable<NeuronCollection>
    {
        private string _label;
        private readonly HashSet<Neuron> _neuronSet;

        public event EventHandler<NeuronCollectionFiringEventArgs> NeuronFired;

        private readonly object _syncObj = new object();
        private readonly List<Tuple<NeuronCollection, ISynapseFactory>> _pendingConnection;
        private bool _connectionEventScheduled;

        public NeuronCollection()
        {
            _label = "";
            _neuronSet = new HashSet<Neuron>();
            _pendingConnection = new List<Tuple<NeuronCollection, ISynapseFactory>>();
        }

        public NeuronCollection(string name)
        {
            // Contract.Requires(name != null);            

            _label = name;
            _neuronSet = new HashSet<Neuron>();
            _pendingConnection = new List<Tuple<NeuronCollection, ISynapseFactory>>();
        }

        public void Add(Neuron neuron)
        {
            // Contract.Requires(neuron != null);

            _neuronSet.Add(neuron);
            neuron.NeuronFired += HandleNeuronFiredEvent;
        }        

        public void Add(NeuronCollection neuronGroup)
        {
            // Contract.Requires(neuronGroup != null);

            foreach (Neuron n in neuronGroup)
                n.NeuronFired += HandleNeuronFiredEvent;                
            _neuronSet.UnionWith(neuronGroup._neuronSet);            
        }

        public void Remove(Neuron neuron)
        {
            // Contract.Requires(neuron != null);

            neuron.NeuronFired -= HandleNeuronFiredEvent;
            _neuronSet.Remove(neuron);
        }

        public void Remove(NeuronCollection neuronGroup)
        {
            // Contract.Requires(neuronGroup != null);

            foreach (Neuron n in neuronGroup)
            {
                n.NeuronFired -= HandleNeuronFiredEvent;
                _neuronSet.Remove(n);
            }
        }

        public void Clear()
        {
            foreach (Neuron n in _neuronSet)
                n.NeuronFired -= HandleNeuronFiredEvent;

            _neuronSet.Clear();
        }

        private void HandleNeuronFiredEvent(object sender, NeuronFiredEventArgs e)
        {
            OnNeuronFired(new NeuronCollectionFiringEventArgs((Neuron)sender, e.Time));
        }

        protected virtual void OnNeuronFired(NeuronCollectionFiringEventArgs e)
        {
            NeuronFired?.Invoke(this, e);
        }

        public void ConnectTo(NeuronCollection neuronGroup, ISynapseFactory synapseFactory)
        {
            // Contract.Requires(neuronGroup != null);
            // Contract.Requires(synapseFactory != null);
            // Contract.Requires(Contract.ForAll(neuronGroup, n => n != null));

            // prevent possible race condition
            while (SpikingNetEngine.IsStopping && SpikingNetEngine.IsRunning)
            {
            }

            if (!SpikingNetEngine.IsRunning)
            {
                foreach (Neuron preSynapticNeuron in this)
                {
                    foreach (Neuron postSynapticNeuron in neuronGroup)
                    {
                        Contract.Assume(postSynapticNeuron != null);
                        Synapse syn = synapseFactory.CreateSynapse();
                        syn.PostsynapticNeuron = postSynapticNeuron;
                        preSynapticNeuron.AddAxonalSynapse(syn);
                        postSynapticNeuron.AddDendriticSynapse(syn);
                    }
                }
            }
            else
            {
                lock (_syncObj)
                {                    
                    _pendingConnection.Add(new Tuple<NeuronCollection, ISynapseFactory>(
                                               neuronGroup.ShallowClone(), synapseFactory.DeepClone()));
                    if (_connectionEventScheduled) return;

                    _connectionEventScheduled = true;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ConnectionEvent, 1);
                }
            }
        }    
        
        private void Scheduler_ConnectionEvent(long time)
        {
            lock (_syncObj)
            {
                foreach (var (neuralNetwork, synapseFactory) in _pendingConnection)
                {
                    foreach (Neuron preSynapticNeuron in this)
                    {
                        foreach (Neuron postSynapticNeuron in neuralNetwork)
                        {
                            Contract.Assume(postSynapticNeuron != null);
                            Synapse syn = synapseFactory.CreateSynapse();
                            syn.PostsynapticNeuron = postSynapticNeuron;
                            preSynapticNeuron.AddAxonalSynapse(syn);
                            postSynapticNeuron.AddDendriticSynapse(syn);
                        }
                    }
                }
                _pendingConnection.Clear();
                _connectionEventScheduled = false;                
            }           
        }

        public string Name
        {
            get => _label;
            set => _label = value;
        }        

        #region IEnumerable<Neuron> Members

        public IEnumerator<Neuron> GetEnumerator()
        {
            return _neuronSet.GetEnumerator();            
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _neuronSet.GetEnumerator();            
        }

        #endregion

        #region IShallowCloneable<NeuronCollection> Members

        public NeuronCollection ShallowClone()
        {
            NeuronCollection retCollection = new NeuronCollection(_label) {this};

            return retCollection;
        }

        #endregion

    }
}
