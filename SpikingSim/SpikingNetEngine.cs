using System.Diagnostics;
using System;
using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace SpikingLibrary
{
    public static class SpikingNetEngine
    {
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
            get => _isStopping;
            private set => _isStopping = value;
        }

        public static bool IsRunning => Scheduler.IsRunning;

        internal static Scheduler Scheduler { get; } = new Scheduler(10000);

        public static void Stop(bool unloadNeurons = false)
        {
            if (!Scheduler.IsRunning) return;

            // schedule a future stop
            // (prevent race conditions when the structure of the network is changed)
            Scheduler.ScheduleEvent(Scheduler_StopEngine, 3);
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
                Scheduler.ScheduleEvent(Scheduler_DelayExecution, 1);
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

                    Scheduler.ScheduleEvent(Scheduler_AddNeurons, 1);
                    _neuronAdditionScheduled = true;
                }
            }         

            return neuron;
        }        

        private static void Scheduler_AddNeurons(long time)
        {
            lock (SyncObj)
            {
                NeuronRegister.Add(PendingNeuronAdditions);
                PendingNeuronAdditions.Clear();
                _neuronAdditionScheduled = false;
            }
        }

        public static void RemoveNeuron(Neuron neuron)
        {
            // Contract.Requires(neuron != null);

            if (!Scheduler.IsRunning)
                NeuronRegister.Remove(neuron);
            else
            {
                lock (SyncObj)
                {
                    PendingNeuronRemovals.Add(neuron);
                    if (_neuronRemovalScheduled) return;

                    Scheduler.ScheduleEvent(Scheduler_RemoveNeurons, 1);
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

                    Scheduler.ScheduleEvent(Scheduler_RemoveNeurons, 1);
                    _neuronRemovalScheduled = true;
                }
            }
        }

        private static void Scheduler_RemoveNeurons(long time)
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
                
            Scheduler.ScheduleEvent(Scheduler_UpdateNeuronStates, 2);  
                
            if (_slowdownMethodRunning)
                Scheduler.ScheduleEvent(Scheduler_DelayExecution, 3);
        }

        // called on the SNN engine thread
        private static void Scheduler_UpdateNeuronStates(long time)
        {
            foreach (Neuron n in NeuronRegister)
            {
                if (n.IncrementNeuronState()) // i.e., if neuron is firing
                    n.TriggerNeuronSpike(time);
            }

            Scheduler.ScheduleEvent(Scheduler_UpdateNeuronStates, 1);
        }

        private static void Scheduler_DelayExecution(long time)
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
            Scheduler.ScheduleEvent(Scheduler_DelayExecution, 1);
        }

        private static void Scheduler_StopEngine(long time)
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

                    Scheduler.ScheduleEvent(Scheduler_CreateConnections, 1);
                    _createConnectionsScheduled = true;
                }
            }            
        }

        private static void Scheduler_CreateConnections(long time)
        {
            lock (SyncObj)
            {
                foreach (var (presynapticNeuron, postsynapticNeuron, synapse) in PendingConnections)
                {/*
                    Contract.Assume(connData != null);
                    Contract.Assume(connData.Item1 != null);
                    Contract.Assume(connData.Item2 != null);
                    Contract.Assume(connData.Item3 != null);*/
                    synapse.PostsynapticNeuron = postsynapticNeuron; // set synapse.post
                    presynapticNeuron.AddAxonalSynapse(synapse);    // pre.AddAxonalSynapse
      //              Contract.Assume(connData.Item3 != null);
                    postsynapticNeuron.AddDendriticSynapse(synapse); // post.AddDendriticSynapse
                }

                PendingConnections.Clear();
                _createConnectionsScheduled = false;                
            }            
        }
    }
}
