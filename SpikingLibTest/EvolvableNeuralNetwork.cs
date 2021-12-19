using System.Collections.Generic;
using System.Linq;
using MathLib.Evolution;
using System.Collections.ObjectModel;

using Util; 
using MathLib.Graph;

namespace SpikingLibTest
{
    internal class EvolvableNeuralNetwork : GaObject<EvolvableNeuralNetwork, GraphChromosome<SpikingNetGraph>>
    {
        private readonly ReadOnlyCollection<int> _inputVertices;
        private readonly ReadOnlyCollection<int> _outputVertices;

        private EvolvableNeuralNetwork(GraphChromosome<SpikingNetGraph> chromosome, ReadOnlyCollection<int> inputVertices,
            ReadOnlyCollection<int> outputVertices)
        {
            // Contract.Requires(chromosome != null);
            Chromosome = chromosome;
            _inputVertices = inputVertices;
            _outputVertices = outputVertices;
        }

        public EvolvableNeuralNetwork(int numInputs, int numOutputs)
        {
            // Contract.Requires(numInputs > 0);
            // Contract.Requires(numOutputs > 0);

            SpikingNetGraph initGraph = new SpikingNetGraph();

            List<int> inputVertices = new List<int>();
            List<int> outputVertices = new List<int>();            
            for (int i = 0; i < numInputs; i++)
                inputVertices.Add(initGraph.AddVertex());
            for (int i = 0; i < numOutputs; i++)
                outputVertices.Add(initGraph.AddVertex());

            _inputVertices = new ReadOnlyCollection<int>(inputVertices);
            _outputVertices = new ReadOnlyCollection<int>(outputVertices);

            Chromosome = new GraphChromosome<SpikingNetGraph>(initGraph);

            GraphChromosome<SpikingNetGraph>.ImmortalVertices = _inputVertices.Union(_outputVertices).ToList();
        }
       
        protected override EvolvableNeuralNetwork CreateObject()
        {
            return new EvolvableNeuralNetwork(Chromosome, _inputVertices, _outputVertices);        
        }

        [ToDo]
        public override double Fitness()
        {

            return StaticRandom.NextDouble();
        }

        public Graph NetworkStructure => Chromosome.ChromosomalGraph;
    }     
}
