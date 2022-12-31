using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar.NeuralNet
{
    internal class Layer
    {
        //Properties
        float[] inputs;
        float[] outputs;
        float[] biases;
        float[,] weights;
        int nbrIn;
        int nbrOut;
        AcitvationFunction function;

        public float[] Inputs { get => inputs; set => inputs = value; }
        public float[] Outputs { get => outputs; }
        public float[] Biases { get => biases; set => biases = value; }
        public float[,] Weights { get => weights; set => weights = value; }

        public enum AcitvationFunction
        {
            ReLU, Binary
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nbrIn"> Number of neurone in this layer </param>
        /// <param name="nbrOut"> Number of neurone in the next layer </param>
        /// <param name="function"> Number of neurone in the next layer </param>
        public Layer(int nbrIn, int nbrOut, AcitvationFunction function)
        {
            this.nbrIn = nbrIn;
            this.nbrOut = nbrOut;
            inputs = new float[nbrIn];
            outputs = new float[nbrOut];
            biases = new float[nbrOut];
            weights = new float[nbrIn, nbrOut];
            this.function = function;
        }

        public void feedForward()
        {
            if (nbrIn != inputs.Length)
            {
                Console.WriteLine("[LAYER][FEED_FORWARD] Wrong data size");
                return;
            }

            for (int i = 0; i < outputs.Length; i++)
            {
                //Init sum
                float sum = 0;

                //Calculate sum from inputs and weights
                for (int j = 0; j < inputs.Length; j++)
                {
                    sum += inputs[j] * weights[j, i];
                }

                //Apply activation function
                switch (function)
                {
                    case AcitvationFunction.ReLU: outputs[i] = ReLUFunction(sum + biases[i]); break;
                    case AcitvationFunction.Binary: outputs[i] = BinaryFunction(sum + biases[i]); break;
                }
            }
        }

        float BinaryFunction(float input)
        {
            if (input > 0)
                return 1;
            else
                return 0;
        }

        float ReLUFunction(float input)
        {
            return Math.Max(0, input);
        }

        public void Randomize()
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = (float)(GameMath.Rnd.NextDouble() * 2 - 1);
                }
            }
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = (float)(GameMath.Rnd.NextDouble() * 2 - 1);
            }
        }

        public void Mutate()
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    if(GameMath.Rnd.Next(1,101) > Globals.MUTATION_CHANCE) { continue; }
                    weights[i, j] = GameMath.lerp(weights[i, j], (float)(GameMath.Rnd.NextDouble() * 2 - 1), Globals.MUTATION_AMMOUNT);
                }
            }
            for (int i = 0; i < biases.Length; i++)
            {
                if (GameMath.Rnd.Next(1, 101) > Globals.MUTATION_CHANCE) { continue; }
                biases[i] = GameMath.lerp(biases[i], (float)(GameMath.Rnd.NextDouble() * 2 - 1), Globals.MUTATION_AMMOUNT); ;
            }
        }
    }
}
