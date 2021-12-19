#region

using System.Collections.Generic;
using MathLib.Evolution;
using MathLib.Graph;
using SpikingLibrary;
using System.Diagnostics.Contracts;

using Util;
#endregion

namespace SpikingLibTest
{
    internal class SpikingNetGraph : Graph, IMutator
    {
        private readonly Dictionary <int, NeuronDescription> _neuronParameters;
        private readonly Dictionary<int, SynapseDescription> _synapseParameters;

        private const int AxonalDelayMutationVariance = 20;
        private const int MaxInitDelayVariance = 200;
        private static readonly StdpParameters StdpMutationVariances = new StdpParameters
        {
            A2Negative = 1e-3,
            A2Positive = 1e-3,
            A3Negative = 1e-3,
            A3Positive = 1e-3,
            TauNegative = 5,
            TauPositive = 5,
            TauX = 5,
            TauY = 5
        };

        private static readonly NeuronParameters NeuronMutationVariances = new NeuronParameters(0.1, 0.2, 1, 1, 3);

        private static readonly NeuronParameters[] PredefinedNeuronTypes =
            {
                NeuronParameters.Accommodation, NeuronParameters.Bistability, NeuronParameters.Class1, NeuronParameters.Class2,
                NeuronParameters.DAP,
                NeuronParameters.InhibitionInducedBursting, NeuronParameters.InhibitionInducedSpiking, NeuronParameters.Integrator,
                NeuronParameters.MixedMode, NeuronParameters.PhasicBursting, NeuronParameters.PhasicSpiking, NeuronParameters.ReboundBurst,
                NeuronParameters.ReboundSpike, NeuronParameters.Resonator, NeuronParameters.SpikeFrequencyAdaptation,
                NeuronParameters.SpikeLatency,
                NeuronParameters.SubthresholdOscillation, NeuronParameters.ThresholdVariability, NeuronParameters.TonicBursting,
                NeuronParameters.TonicSpiking
            };

        private static readonly StdpParameters[] PredefinedSynapseTypes =
            {
                StdpParameters.HippocampalCulture, StdpParameters.RatVisualCortexL23
            };

        public SpikingNetGraph()
        {
            VertexAddedEvent += SpikingNetGraph_VertexAddedEvent;
            VertexRemovedEvent += SpikingNetGraph_VertexRemovedEvent;
            EdgeAddedEvent += SpikingNetGraph_EdgeAddedEvent;
            EdgeRemovedEvent += SpikingNetGraph_EdgeRemovedEvent;

            _neuronParameters = new Dictionary<int, NeuronDescription>();
            _synapseParameters = new Dictionary<int, SynapseDescription>();
        }

        private static NeuronDescription GenerateNeuronParameters()
        {
            return new NeuronDescription(PredefinedNeuronTypes[StaticRandom.Next(PredefinedNeuronTypes.Length)].DeepClone());

        }

        private static SynapseDescription GenerateSynapseParameters()
        {
            return new SynapseDescription(StaticRandom.Next(1, MaxInitDelayVariance),
                                          PredefinedSynapseTypes[StaticRandom.Next(PredefinedSynapseTypes.Length)].
                                              DeepClone());        
        }

        #region IMutator Members

        public void Mutate()
        {
            foreach (NeuronDescription n in _neuronParameters.Values)
            {
                Contract.Assume(n != null);
                NeuronParameters neuronParams = n.Parameters;
                n.Parameters = new NeuronParameters(
                    neuronParams.A + (StaticRandom.NextDouble() - 0.5)*NeuronMutationVariances.A,
                    neuronParams.B + (StaticRandom.NextDouble() - 0.5)*NeuronMutationVariances.B,
                    neuronParams.C + (StaticRandom.NextDouble() - 0.5)*NeuronMutationVariances.C,
                    neuronParams.D + (StaticRandom.NextDouble() - 0.5)*NeuronMutationVariances.D,
                    neuronParams.I + (StaticRandom.NextDouble() - 0.5)*NeuronMutationVariances.I);
            }

            foreach (SynapseDescription synapse in _synapseParameters.Values)
            {
                Contract.Assume(synapse != null);
                StdpParameters oldParams = synapse.LearningParameters;
                StdpParameters newParams = new StdpParameters
                {
                    A2Negative =
                        oldParams.A2Negative +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.A2Negative,
                    A2Positive =
                        oldParams.A2Positive +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.A2Positive,
                    A3Negative =
                        oldParams.A3Negative +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.A3Negative,
                    A3Positive =
                        oldParams.A3Positive +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.A3Positive,
                    TauNegative =
                        oldParams.TauNegative +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.TauNegative,
                    TauPositive =
                        oldParams.TauPositive +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.TauPositive,
                    TauX =
                        oldParams.A2Negative +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.TauX,
                    TauY =
                        oldParams.TauY +
                        (StaticRandom.NextDouble() - 0.5) * StdpMutationVariances.TauY
                };
                int axonalDelay = synapse.AxonalDelay + StaticRandom.Next(-AxonalDelayMutationVariance, AxonalDelayMutationVariance);
                if (axonalDelay < 1)
                    axonalDelay = 1;
                
                synapse.SetParameters(axonalDelay, newParams);
            }
        }

        #endregion

        private void SpikingNetGraph_EdgeRemovedEvent(object sender, EdgeChangeEventArgs e)
        {
            _synapseParameters.Remove(e.EdgeId);            
        }

        private void SpikingNetGraph_EdgeAddedEvent(object sender, EdgeChangeEventArgs e)
        {
            _synapseParameters.Add(e.EdgeId, GenerateSynapseParameters());            
        }

        private void SpikingNetGraph_VertexRemovedEvent(object sender, VertexChangeEventArgs e)
        {
            _neuronParameters.Remove(e.VertexId);            
        }

        private void SpikingNetGraph_VertexAddedEvent(object sender, VertexChangeEventArgs e)
        {
            _neuronParameters.Add(e.VertexId, GenerateNeuronParameters());            
        }

    }
}