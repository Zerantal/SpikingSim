using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SpikingLibrary
{
    [ContractClass(typeof(InputSourceContract))]
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

                SpikingNetEngine.Scheduler.ScheduleEvent(new ScheduledEvent(Sched_SynchroniseInputs), 1);
                _synchronisationEventScheduled = true;
            }
        }

        private static void Sched_SynchroniseInputs(long time)
        {
            lock (ObjSync)
            {
                foreach (Tuple<InputSource, int> t in PendingInputsToSynchronise)
                {
                    Contract.Assume(t.Item2 > 0);
                    t.Item1.Start(t.Item2);
                }
                PendingInputsToSynchronise.Clear();
                _synchronisationEventScheduled = false;
            }
        }

        public abstract void Start(int startTime = 1);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop")]
        public abstract void Stop();

        [ContractInvariantMethod]
        private static void ObjectInvariant()
        {
        }
    }
}
