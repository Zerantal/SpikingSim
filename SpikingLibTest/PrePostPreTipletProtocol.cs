using System;
using System.Diagnostics.Contracts;

using ZedGraph;
using SpikingLibrary;
// ReSharper disable IdentifierTypo

namespace SpikingLibTest
{
    [ContractVerification(false)]
    internal class PrePostPreTipletProtocol
    {
        public static double InitWeight = 0.25;
        public static int NumPairs = 60;
        public static event EventHandler UpdateEvent;
        public static double Frequency = 1;

        private readonly PointPairList _ptList;
        private readonly double[,] _prepostpreTriplets;
        private readonly SynapseProbe _preProbe;
        private readonly PeriodicInputSource _preInput;
        private readonly PeriodicInputSource _postInput;
        private readonly TimeDelayNotification _delayNotification;        
        private int _tripletIdx;

        public PrePostPreTipletProtocol(PointPairList prepostprePtList, double[,] prepostpreTriplets)
        {
            _ptList = prepostprePtList;
            _prepostpreTriplets = prepostpreTriplets;
            _tripletIdx = 0;

            Neuron n = SpikingNetEngine.CreateNeuron(new NeuronParameters(0.02, -0.1, -55, 0, 0));  // create neuron to use in experiments                  

            Synapse s1 = new Synapse(1, InitWeight, StdpParameters.HippocampalCulture);
            Synapse s2 = new NonLearningSynapse(1, 80);
            _preProbe = new SynapseProbe(s1);

            SpikingNetEngine.Start();
            _delayNotification = new TimeDelayNotification();
            _delayNotification.Notification += RepeatTripletProtocol;
            
            _preInput = new PeriodicInputSource(maxInputCycles: NumPairs);
            _postInput = new PeriodicInputSource(maxInputCycles: NumPairs);
            _preInput.ConnectTo(n, s1);
            _postInput.ConnectTo(n, s2);
            _preInput.PeriodicInputFinished += preInput_PeriodicInputFinished;

            // run Pair protocol
            ExecuteTripletProtocol();
        }

        private void preInput_PeriodicInputFinished(object sender, EventArgs e)
        {
            // delay for 10s to prevent possible race condition
            _delayNotification.CreateNotification(100000);
        }

        private void RepeatTripletProtocol(object sender, EventArgs e)
        {
            _ptList.Add(_tripletIdx, _preProbe.GetWeight() - InitWeight);
            OnUpdateEvent(EventArgs.Empty);

            _tripletIdx++;                        

            if (_tripletIdx < _prepostpreTriplets.GetLength(0))
                ExecuteTripletProtocol();
        }

        private void ExecuteTripletProtocol()
        {
            _preProbe.SetWeight(InitWeight);

            double dt1 = _prepostpreTriplets[_tripletIdx, 0];
            double dt2 = _prepostpreTriplets[_tripletIdx, 1];

            Contract.Assert(dt1 > 0 && dt2 < 0);

            double preSpikeInterval = dt1 - dt2;

            int preInterval = (int) (preSpikeInterval*10);
            if (preInterval == 0) preInterval++;    // ensure non zero interval

            _preInput.SetSpikeTimings(new[] {preInterval, (int)(1/Frequency*10000 - preInterval)});                
            _postInput.SetSpikeTimings(new[] { (int)(1 / Frequency * 10000) });

            InputSource.SynchronizeInputSignals(new[]
                                                     {
                                                         new Tuple<InputSource, int>(_preInput, 1),
                                                         new Tuple<InputSource, int>(_postInput, (int) (dt1*10) + 1)
                                                     });           
        }

        protected virtual void OnUpdateEvent(EventArgs e)
        {
            EventHandler handler = UpdateEvent;

            handler?.Invoke(this, e);
        }
    }
}
