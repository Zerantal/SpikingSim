using System.Diagnostics.Contracts;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace SpikingLibrary
{
    public static class SpikingNetEngine
    {
        private static readonly Scheduler Sched = new Scheduler(10000); // 1000 ms in 0.1ms increment
        internal const double Dt = 0.1; // Time granularity of simulator
        private static readonly Stopwatch Timer = new Stopwatch();
        private static readonly NeuronCollection NeuronRegister = new NeuronCollection();

        // needs to be locked;
        private static int _slowdown; // insert pause between each time step (in μs)
        private static bool _slowdownMethodRunning; // true if slowdown is in effect
        private static readonly object SyncObj = new object(); // locking object

        private static readonly NeuronCollection PendingNeuronAdditions = new NeuronCollection();
                                                 // needs to be locked with _syncObj

        private static bool _neuronAdditionScheduled;
        private static readonly NeuronCollection PendingNeuronRemovals = new NeuronCollection();
        private static bool _neuronRemovalScheduled;
        private static volatile bool _isStopping;
        private static volatile bool _unloadNeurons;
        private static readonly List<Tuple<Neuron, Neuron, Synapse>> PendingConnections = new List<Tuple<Neuron, Neuron, Synapse>>();
        private static bool _createConnectionsScheduled;

        internal static bool IsStopping
        {
            get { return _isStopping; }
            private set { _isStopping = value; }
        }

        public static bool IsRunning
        {
            get { return Scheduler.IsRunning; }
        }

        internal static Scheduler Scheduler
        {
            get { return Sched; }
        }

        public static void Stop(bool unloadNeurons = false)
        {
            if (!Scheduler.IsRunning) return;

            // schedule a future stop
            // (prevent race conditions when the structure of the network is changed)
            Scheduler.ScheduleEvent(Sched_StopEngine, 3);
            IsStopping = true;
            _unloadNeurons = unloadNeurons;
        }

        public static void Pause()
        {
            Scheduler.Pause();
        }

        public static void Resume()
        {
            Scheduler.Resume();
        }

        public static void SlowNeuralEngine(int slowdown)
        {
            // Contract.Requires(slowdown >= 0);

            lock (SyncObj)
            {
                _slowdown = slowdown;
                if (slowdown == 0) return;

                if (_slowdownMethodRunning) return; // don't schedule a new one
                _slowdownMethodRunning = true;
                Scheduler.ScheduleEvent(Sched_DelayExecution, 1);
            }
        }

        public static Neuron CreateNeuron(NeuronParameters parameters)
        {
            // Contract.Requires(parameters != null);

            Neuron neuron = new Neuron(parameters.DeepClone());

            if (!Scheduler.IsRunning)
                NeuronRegister.Add(neuron);
            else
            {
                lock (SyncObj)
                {
                    PendingNeuronAdditions.Add(neuron);
                    if (_neuronAdditionScheduled) return neuron;

                    Scheduler.ScheduleEvent(Sched_AddNeurons, 1);
                    _neuronAdditionScheduled = true;
                }
            }         

            return neuron;
        }        

        private static void Sched_AddNeurons(long time)
        {
            lock (SyncObj)
            {
                NeuronRegister.Add(PendingNeuronAdditions);
                PendingNeuronAdditions.Clear();
                _neuronAdditionScheduled = false;
            }
        }

        public static void RemoveNeuron(Neuron n)
        {
            // Contract.Requires(n != null);

            if (!Scheduler.IsRunning)
                NeuronRegister.Remove(n);
            else
            {
                lock (SyncObj)
                {
                    PendingNeuronRemovals.Add(n);
                    if (_neuronRemovalScheduled) return;

                    Scheduler.ScheduleEvent(Sched_RemoveNeurons, 1);
                    _neuronRemovalScheduled = true;
                }
            }
        }

    public static void RemoveNeuronCollection(NeuronCollection neuronGroup)
        {
            // Contract.Requires(neuronGroup != null);

            if (!Scheduler.IsRunning)
                NeuronRegister.Remove(neuronGroup);
            else
            {
                lock (SyncObj)
                {
                    PendingNeuronRemovals.Add(neuronGroup);
                    if (_neuronRemovalScheduled) return;

                    Scheduler.ScheduleEvent(Sched_RemoveNeurons, 1);
                    _neuronRemovalScheduled = true;
                }
            }
        }

        private static void Sched_RemoveNeurons(long time)
        {
            lock (SyncObj)
            {
                NeuronRegister.Remove(PendingNeuronRemovals);
                PendingNeuronRemovals.Clear();
                _neuronRemovalScheduled = false;
            }
        }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public static void Start(string name = "Neural network Engine")
        {
            if (Scheduler.IsRunning) return;

            Scheduler.StartAsync(name);
                
            Scheduler.ScheduleEvent(Sched_UpdateNeuronStates, 2);  
                
            if (_slowdownMethodRunning)
                Scheduler.ScheduleEvent(Sched_DelayExecution, 3);
        }

        // called on the SNN engine thread
        private static void Sched_UpdateNeuronStates(long time)
        {
            foreach (Neuron n in NeuronRegister)
            {
                if (n.IncrementNeuronState()) // i.e., if neuron is firing
                    n.TriggerNeuronSpike(time);
            }

            Scheduler.ScheduleEvent(Sched_UpdateNeuronStates, 1);
        }

        private static void Sched_DelayExecution(long time)
        {
            Timer.Reset();
            Timer.Start();

            if (_slowdown == 0)
            {
                _slowdownMethodRunning = false;
                return;
            }
            while (Timer.ElapsedTicks < Stopwatch.Frequency/1000000*_slowdown)
            {
            }
            Scheduler.ScheduleEvent(Sched_DelayExecution, 1);
        }

        private static void Sched_StopEngine(long time)
        {
            Scheduler.Stop();
            IsStopping = false;
            if (_unloadNeurons)
                NeuronRegister.Clear();
        }

        internal static void ConnectNeurons(Neuron presynapticNeuron, Neuron postsynapticNeuron, Synapse synapse)
        {
            // Contract.Requires(presynapticNeuron != null);
            // Contract.Requires(postsynapticNeuron != null);
            // Contract.Requires(synapse != null);

            if (!Scheduler.IsRunning)
            {
                synapse.PostsynapticNeuron = postsynapticNeuron;
                presynapticNeuron.AddAxonalSynapse(synapse);
                postsynapticNeuron.AddDendriticSynapse(synapse);
            }                
            else
            {
                lock (SyncObj)
                {
                    PendingConnections.Add(new Tuple<Neuron, Neuron, Synapse>(presynapticNeuron, postsynapticNeuron,
                                                                              synapse));                    
                    if (_createConnectionsScheduled) return;

                    Scheduler.ScheduleEvent(Sched_CreateConnections, 1);
                    _createConnectionsScheduled = true;
                }
            }            
        }

        private static void Sched_CreateConnections(long time)
        {
            lock (SyncObj)
            {
                foreach (Tuple<Neuron, Neuron, Synapse> connData in PendingConnections)
                {/*
                    Contract.Assume(connData != null);
                    Contract.Assume(connData.Item1 != null);
                    Contract.Assume(connData.Item2 != null);
                    Contract.Assume(connData.Item3 != null);*/
                    connData.Item3.PostsynapticNeuron = connData.Item2; // set synapse.post
                    connData.Item1.AddAxonalSynapse(connData.Item3);    // pre.AddAxonalSynapse
      //              Contract.Assume(connData.Item3 != null);
                    connData.Item2.AddDendriticSynapse(connData.Item3); // post.AddDendriticSynapse
                }

                PendingConnections.Clear();
                _createConnectionsScheduled = false;                
            }            
        }

        [ContractInvariantMethod]
        private static void ObjectInvariant()
        {
            Contract.Invariant(Contract.ForAll(PendingConnections, t => t != null));
            Contract.Invariant(Contract.ForAll(PendingConnections,
                                               t => t.Item1 != null && t.Item2 != null && t.Item3 != null));
        }
    }
}
