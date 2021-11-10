using System;
using ZedGraph;
using SpikingLibrary;
using System.Diagnostics.Contracts;

namespace SpikingLibTest
{
    [ContractVerification(false)]
    class QuadrupletProtocol
    {
        public static double InitWeight = 0.25;
        public static int NumPairs = 60;
        public static event EventHandler UpdateEvent;        
        public static double TInc = 3;
        public static double Dt1 = -5;
        public static double Dt2 = 5;
        public static double Frequency = 1;

        private readonly PointPairList _ptList;
        private readonly SynapseProbe _preProbe;
        private readonly PeriodicInputSource _preInput;
        private readonly PeriodicInputSource _postInput;
        private readonly TimeDelayNotification _delayNotification;
        private double _T;
        private readonly double _TMin = -100;
        private readonly double _TMax = 100;

        public QuadrupletProtocol(PointPairList ptList, double TMin, double TMax)
        {            
            _ptList = ptList;
            _TMax = TMax;
            _TMin = TMin;

            Neuron n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, -0.1, -55, 0, 0));  // create neuron to use in experiments                  

            Synapse s1 = new Synapse(1, InitWeight, StdpParameters.HippocampalCulture);
            Synapse s2 = new NonLearningSynapse(1, 80);
            _preProbe = new SynapseProbe(s1);

            SpikingNetEngine.Start();
            _delayNotification = new TimeDelayNotification();
            _delayNotification.Notification += RepeatQuadrupletProtocol;

            _T = _TMin;

            _preInput = new PeriodicInputSource(maxInputCycles: NumPairs);
            _postInput = new PeriodicInputSource(maxInputCycles: NumPairs);
            _preInput.ConnectTo(n, s1);
            _postInput.ConnectTo(n, s2);
            _preInput.PeriodicInputFinished += preInput_PeriodicInputFinished;

            // run Pair protocol
            ExecuteQuadrupletProtocol();
        }

        void preInput_PeriodicInputFinished(object sender, EventArgs e)
        {
            // delay for 5s to allow input to finish propogating to synapse
            _delayNotification.CreateNotification(50000);
        }

        private void RepeatQuadrupletProtocol(object sender, EventArgs e)
        {
            _ptList.Add(_T, _preProbe.GetWeight() - InitWeight);
            OnUpdateEvent(EventArgs.Empty);

            _T += TInc;

            if (_T <= _TMax)
                ExecuteQuadrupletProtocol();
        }

        private void ExecuteQuadrupletProtocol()
        {
            _preProbe.SetWeight(InitWeight);

            double preSpikeInterval = _T + Dt1/2 - Dt2/2;
            double postSpikeInterval = _T - Dt1/2 + Dt2/2;

            int preInterval = (int) Math.Abs(preSpikeInterval*10);
            int postInterval = (int) Math.Abs(postSpikeInterval*10);

            if (preInterval == 0) preInterval++;
            if (postInterval == 0) postInterval++;

            _preInput.SetSpikeTimings(new[]
                                          {
                                              preInterval,
                                              (int) (1/Frequency*10000 - preInterval)
                                          });
            _postInput.SetSpikeTimings(new[]
                                           {
                                               postInterval,
                                               (int) (1/Frequency*10000 - postInterval)
                                           });
            double preInputStartTime;
            double postInputStartTime;

            FindInputStartTimes(out preInputStartTime, out postInputStartTime);

            InputSource.SynchronizeInputSignals(new[]
                                                     {
                                                         new Tuple<InputSource, int>(_preInput,
                                                                                      (int) (preInputStartTime*10)),
                                                         new Tuple<InputSource, int>(_postInput,
                                                                                      (int) (postInputStartTime*10))
                                                     });

        }


        private void FindInputStartTimes(out double preInputStartTime, out double postInputStartTime)
        {
            double preSpikeInterval = _T + Dt1 / 2 - Dt2 / 2;
            double postSpikeInterval = _T - Dt1 / 2 + Dt2 / 2;

                        // Evaluate all relative timings of spike
            const double t1Post = 1;
            double t2Post = t1Post + postSpikeInterval;
            double t1Pre = t1Post - Dt1;
            double t2Pre = t1Pre + preSpikeInterval;
            

            // find start time for input sources
            if (t1Pre < t2Pre)
                preInputStartTime = t1Pre;
            else            
                preInputStartTime = t2Pre;
            if (t1Post < t2Post)
                postInputStartTime = t1Post;
            else
                postInputStartTime = t2Post;

            double minStartTime;
            if (preInputStartTime < postInputStartTime)
                minStartTime = preInputStartTime;
            else
            {
                minStartTime = postInputStartTime;
            }

            if (minStartTime < 1)
            {
                preInputStartTime += -minStartTime + 1;
                postInputStartTime += -minStartTime + 1;
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
