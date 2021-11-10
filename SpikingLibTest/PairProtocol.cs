using System;
using ZedGraph;
using SpikingLibrary;
using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    class PairProtocol
    {
        public static double InitWeight = 0.25;
        public static int NumPairs = 60;
        public static double RhoMin = 1;
        public static double RhoMax = 50;
        public static double RhoInc = 1;        
        public static event EventHandler UpdateEvent;

        private double _rho;
        private readonly double _timeInterval;
        private readonly PointPairList _ptList;
        private readonly SynapseProbe _preProbe;
        private readonly PeriodicInputSource _preInput;
        private readonly PeriodicInputSource _postInput;
        private readonly TimeDelayNotification _delayNotification;

        

        public PairProtocol(PointPairList ptList, double timeInterval)
        {
            _timeInterval = timeInterval;
            _ptList = ptList;

            Neuron n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, -0.1, -55, 0, 0));  // create neuron to use in experiments                  

            Synapse s1 = new Synapse(1, InitWeight, StdpParameters.RatVisualCortexL23);
            Synapse s2 = new NonLearningSynapse(1, 80);
            _preProbe = new SynapseProbe(s1);            

            SpikingNetEngine.Start();
            _delayNotification = new TimeDelayNotification();
            _delayNotification.Notification += RepeatPairExperiment;

            _rho = RhoMin;
            _preInput = new PeriodicInputSource(maxInputCycles: NumPairs);
            _postInput = new PeriodicInputSource(maxInputCycles: NumPairs);
            _preInput.ConnectTo(n, s1);        
            _postInput.ConnectTo(n, s2);
            _preInput.PeriodicInputFinished += preInput_PeriodicInputFinished;           

            // run Pair protocol
            ExecutePairProtocol();
        }

        void preInput_PeriodicInputFinished(object sender, EventArgs e)
        {
            // delay for 10s to prevent possible race condition
            _delayNotification.CreateNotification(100000); 
        }

        private void RepeatPairExperiment(object sender, EventArgs e)
        {
            _ptList.Add(_rho, _preProbe.GetWeight() - InitWeight);
            OnUpdateEvent(EventArgs.Empty);

            _rho += RhoInc;

            if (_rho <= RhoMax)            
                ExecutePairProtocol();                                      
        }

        private void ExecutePairProtocol()
        {            
            _preProbe.SetWeight(InitWeight);

            _preInput.SetSpikeTimings(new[] {(int) (1/_rho*10000)});
            _postInput.SetSpikeTimings(new[] {(int) (1/_rho*10000)});

            if (_timeInterval < 0)
            {
                InputSource.SynchronizeInputSignals(new[]
                                                         {
                                                             new Tuple<InputSource, int>(_preInput,
                                                                                          (int) (_timeInterval*-10) + 1),
                                                             new Tuple<InputSource, int>(_postInput, 1)
                                                         });
            }
            else
            {
                InputSource.SynchronizeInputSignals(new[]
                                                         {
                                                             new Tuple<InputSource, int>(_postInput,
                                                                                          (int) (_timeInterval*10) + 1),
                                                             new Tuple<InputSource, int>(_preInput, 1)
                                                         });                
            }            
        }

        protected virtual void OnUpdateEvent(EventArgs e)
        {            
            EventHandler handler = UpdateEvent;
            
            if (handler != null)
            {             
                handler(this, e);
            }
        }
    }
}
