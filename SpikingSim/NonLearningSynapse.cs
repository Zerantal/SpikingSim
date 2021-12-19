namespace SpikingLibrary
{   
    public class NonLearningSynapse : Synapse
    {
        public NonLearningSynapse(int delay, double efficacy) :
            base(delay, efficacy, default)
        {            
            //Contract.Requires<ArgumentOutOfRangeException>(delay >= 1);
        }

        internal override void ActivateSynapse(long time)
        {
            PostsynapticNeuron.V += Weight;           
        }

        internal override void Bap(long time)
        {
        }
    }
}
