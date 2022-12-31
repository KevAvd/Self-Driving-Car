using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar.NeuralNet
{
    internal class CarBrain
    {
        //Neural Net
        NeuroneLayer layer1 = new NeuroneLayer(Globals.RAY_NBR, 6);
        NeuroneLayer layer2 = new NeuroneLayer(6, 4);

        //Car commands
        bool forward = false;
        bool backward = false;
        bool left = false;
        bool right = false;

        //Getters-Setters
        public bool Forward { get => forward; set => forward = value; }
        public bool Backward { get => backward; set => backward = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public NeuroneLayer Layer1 { get => layer1; set => layer1 = value; }
        public NeuroneLayer Layer2 { get => layer2; set => layer2 = value; }

        public void ProcessInput(float[] inputs)
        {
            layer1.Inputs = inputs;
            layer1.feedForward();
            layer2.Inputs = layer1.Outputs;
            layer2.feedForward();

            if (layer2.Outputs[0] == 1)
                forward = true;
            else
                forward = false;

            if (layer2.Outputs[1] == 1)
                backward = true;
            else
                backward = false;

            if (layer2.Outputs[2] == 1)
                left = true;
            else
                left = false;

            if (layer2.Outputs[3] == 1)
                right = true;
            else
                right = false;
        }

        public void WriteLine()
        {
            Console.WriteLine($"[Layer 0]/[Layer 1]/[Layer 2]");
            Console.WriteLine($"[ENTRY 0][OUT={layer1.Inputs[0]}]  [NEURONE 0][OUT={layer1.Outputs[0]}]  [NEURONE 0][OUT={layer2.Outputs[0]}][Forward]                                   ");
            Console.WriteLine($"[ENTRY 1][OUT={layer1.Inputs[1]}]  [NEURONE 1][OUT={layer1.Outputs[1]}]  [NEURONE 1][OUT={layer2.Outputs[1]}][backward]                                   ");
            Console.WriteLine($"[ENTRY 2][OUT={layer1.Inputs[2]}]  [NEURONE 2][OUT={layer1.Outputs[2]}]  [NEURONE 2][OUT={layer2.Outputs[2]}][left]                                   ");
            Console.WriteLine($"[ENTRY 3][OUT={layer1.Inputs[3]}]  [NEURONE 3][OUT={layer1.Outputs[3]}]  [NEURONE 3][OUT={layer2.Outputs[3]}][right]                                   ");
            Console.WriteLine($"[ENTRY 4][OUT={layer1.Inputs[4]}]  [NEURONE 4][OUT={layer1.Outputs[4]}]                                                                         ");
            Console.WriteLine($"[ENTRY 5][OUT={layer1.Inputs[5]}]  [NEURONE 5][OUT={layer1.Outputs[5]}]                                                                         ");
            //Console.WriteLine("Layer0-1 Weights");
            //for (int i = 0; i < layer1.Weights.GetLength(0); i++)
            //{
            //    for (int j = 0; j < layer1.Weights.GetLength(1); j++)
            //    {
            //        Console.WriteLine($"[WEIGHT][ENTRY {i}][NEURONE {j}] {layer1.Weights[i, j]}");
            //    }
            //}
            //Console.WriteLine("Layer1-1 Weights");
            //for (int i = 0; i < layer2.Weights.GetLength(0); i++)
            //{
            //    for (int j = 0; j < layer2.Weights.GetLength(1); j++)
            //    {
            //        Console.WriteLine($"[WEIGHT][NEURONE {i}][NEURONE {j}] {layer2.Weights[i, j]}");
            //    }
            //}
        }

        public void Randomize()
        {
            layer1.Randomize();
            layer2.Randomize();
        }

        public void Mutate()
        {
            layer1.Mutate();
            layer2.Mutate();
        }
    }

    internal class NeuroneLayer
    {
        //Properties
        float[] inputs;
        float[] outputs;
        float[] biases;
        float[,] weights;
        int nbrIn;
        int nbrOut;

        public float[] Inputs { get => inputs; set => inputs = value; }
        public float[] Outputs { get => outputs; }
        public float[] Biases { get => biases; set => biases = value; }
        public float[,] Weights { get => weights; set => weights = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nbrIn"> Number of neurone in this layer </param>
        /// <param name="nbrOut"> Number of neurone in the next layer </param>
        public NeuroneLayer(int nbrIn, int nbrOut)
        {
            this.nbrIn = nbrIn;
            this.nbrOut = nbrOut;
            inputs = new float[nbrIn];
            outputs = new float[nbrOut];
            biases = new float[nbrOut];
            weights = new float[nbrIn,nbrOut];
        }

        public void feedForward()
        {
            if (nbrIn != inputs.Length)
            {
                Console.WriteLine("[NEURONE_LAYER][FEED_FORWARD] Wrong data size");
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

                //Check if neurone fire or not
                if(sum + biases[i] > 0)
                    outputs[i] = 1;
                else
                    outputs[i] = 0;
            }
        }

        public void Randomize()
        {
            for(int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i,j] = (float)(GameMath.Rnd.NextDouble() * 2 - 1);
                }
            }
            for(int i = 0; i < biases.Length; i++)
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
                    weights[i, j] = GameMath.lerp(weights[i, j], (float)(GameMath.Rnd.NextDouble() * 2 - 1), Globals.MUTATION_AMMOUNT);
                }
            }
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = GameMath.lerp(biases[i], (float)(GameMath.Rnd.NextDouble() * 2 - 1), Globals.MUTATION_AMMOUNT); ;
            }
        }
    }
}
