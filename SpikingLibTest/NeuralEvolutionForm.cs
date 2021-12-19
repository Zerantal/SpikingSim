using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Diagnostics;

using MathLib.Evolution;
using Wolfram.NETLink;

namespace SpikingLibTest
{
    public partial class NeuralEvolutionForm : Form
    {
        private Thread _evolutionThread;
        private EvolutionEngine<EvolvableNeuralNetwork> _evolutionEngine;

        public NeuralEvolutionForm()
        {
            InitializeComponent();

            GraphChromosome<SpikingNetGraph>.EdgeAdditionRate = 15;
            GraphChromosome<SpikingNetGraph>.VertexAdditionRate = 5;
            GeneticAlgorithm.MutationRate = 0.01;

            // setup MathPictureBox            
            IKernelLink ml = MathLinkFactory.CreateKernelLink();
            if (ml == null)
            {
                MessageBox.Show(this, @"Unable to start Mathematica Kernel :(", @"Error", MessageBoxButtons.OK);
                Close();
            }
            Contract.Assume(ml != null);
            ml.WaitAndDiscardAnswer();
            mathPictureBox1.Link = ml;

            EvolveTest();
        }

        private void EvolveTest()
        {
            List<EvolvableNeuralNetwork> initPopulation = new List<EvolvableNeuralNetwork>
                                                              {new EvolvableNeuralNetwork(3, 1)};

            _evolutionEngine = new EvolutionEngine<EvolvableNeuralNetwork>(initPopulation, 20)
                                                               {ElitismSelection = 10};            
            _evolutionEngine.NewGenerationCreated += _evolutionEngine_NewGenerationCreated;

            _evolutionThread = new Thread(StartEvolution);
            _evolutionThread.Start(2000);            
        }

        private void StartEvolution(object numGenerations)
        {
            int n = Convert.ToInt32(numGenerations);

            if (n <= 0)
            {
                Debug.WriteLine("Incorrect parameter passed to StartEvolution");
                return;
            }

            _evolutionEngine.Evolve(n);
        }

        private void _evolutionEngine_NewGenerationCreated(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new EventHandler(_evolutionEngine_NewGenerationCreated));
            else
            {
                EvolvableNeuralNetwork bestSolution = _evolutionEngine.BestSolution;
                outputTB.AppendText("Generation # " + _evolutionEngine.CurrentGeneration + ", Best: " +
                                    bestSolution.NetworkStructure + "   (" + bestSolution.Fitness() + ")" + Environment.NewLine);
                mathPictureBox1.MathCommand = "GraphPlot[ " + bestSolution.NetworkStructure + ", VertexLabeling -> True]";
            }            
        }
    }
}
