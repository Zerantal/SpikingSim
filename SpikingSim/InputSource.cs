using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SpikingLibrary
{
    public abstract class InputSource
    {
        private static readonly List<Tuple<InputSource, int>> PendingInputsToSynchronise =
            new List<Tuple<InputSource, int>>();
        private static bool _synchronisationEventScheduled;

        private static readonly object ObjSync = new object();

        public static void SynchronizeInputSignals(Tuple<InputSource, int>[] signals)
        {
            // Contract.Requires(signals != null);
            // Contract.Requires(Contract.ForAll(signals, s => s != null));
            // Contract.Requires(Contract.ForAll(signals, s => s.Item1 != null));
            // Contract.Requires(Contract.ForAll(signals, s => s.Item2 > 0));

            lock (ObjSync)
            {
                PendingInputsToSynchronise.AddRange(signals);
                if (_synchronisationEventScheduled) return;

                SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_SynchroniseInputs, 1);
                _synchronisationEventScheduled = true;
            }
        }

        private static void Scheduler_SynchroniseInputs(long timeInterval)
        {
            lock (ObjSync)
            {
                foreach (var (source, time) in PendingInputsToSynchronise)
                {
                    Contract.Assume(time > 0);
                    source.Start(time);
                }
                PendingInputsToSynchronise.Clear();
                _synchronisationEventScheduled = false;
            }
        }

        public abstract void Start(int startTime = 1);

        // ReSharper disable once UnusedMemberInSuper.Global
        public abstract void Stop();

        [ContractInvariantMethod]
        private static void ObjectInvariant()
        {
        }
    }
}
