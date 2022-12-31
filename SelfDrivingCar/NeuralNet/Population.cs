using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar.NeuralNet
{
    internal class Population
    {
        Genome bestGenome;
        float bestScore = 0;
        CarBrain[] carBrains = new CarBrain[Globals.AI_PER_GENERATION];

        public Population()
        {
            for(int i = 0; i < carBrains.Length; i++)
            {
                carBrains[i] = new CarBrain();
                carBrains[i].Randomize();
            }
        }

        public float[] ProcessInputWithBrain(int index, float[] inupts)
        {
            return carBrains[index].ProcessInput(inupts);
        }

        public void SetBrainDistance(int index, float distance)
        {
            carBrains[index].Distance = distance;
        }

        public void NextGeneration()
        {
            FoundBestBrain();
            for(int i = 0; i < carBrains.Length; i++)
            {
                CopyBestGenome(carBrains[i]);
                if (i != 0) { carBrains[i].Mutate(); }
            }
        }

        public void FoundBestBrain()
        {
            float score = 0;
            for(int i = 0; i < carBrains.Length; i++)
            {
                if (carBrains[i].Distance > score)
                {
                    score = carBrains[i].Distance;
                    SaveBrain(carBrains[i]);
                }
            }
        }

        public void CopyBestGenome(CarBrain brain)
        {
            Array.Copy(bestGenome.b1, brain.Layers[0].Biases, bestGenome.b1.Length);
            Array.Copy(bestGenome.b2, brain.Layers[1].Biases, bestGenome.b2.Length);
            Array.Copy(bestGenome.b3, brain.Layers[2].Biases, bestGenome.b3.Length);
            Array.Copy(bestGenome.w1, brain.Layers[0].Weights, bestGenome.w1.Length);
            Array.Copy(bestGenome.w2, brain.Layers[1].Weights, bestGenome.w2.Length);
            Array.Copy(bestGenome.w3, brain.Layers[2].Weights, bestGenome.w3.Length);
        }

        public void SaveBrain(CarBrain brain)
        {
            Genome genome = new Genome();
            genome.b1 = new float[brain.Layers[0].Biases.Length];
            genome.b2 = new float[brain.Layers[1].Biases.Length];
            genome.b3 = new float[brain.Layers[2].Biases.Length];
            genome.w1 = new float[brain.Layers[0].Weights.GetLength(0), brain.Layers[0].Weights.GetLength(1)];
            genome.w2 = new float[brain.Layers[1].Weights.GetLength(0), brain.Layers[1].Weights.GetLength(1)];
            genome.w3 = new float[brain.Layers[2].Weights.GetLength(0), brain.Layers[2].Weights.GetLength(1)];
            Array.Copy(brain.Layers[0].Biases, genome.b1, brain.Layers[0].Biases.Length);
            Array.Copy(brain.Layers[1].Biases, genome.b2, brain.Layers[1].Biases.Length);
            Array.Copy(brain.Layers[2].Biases, genome.b3, brain.Layers[2].Biases.Length);
            Array.Copy(brain.Layers[0].Weights, genome.w1, brain.Layers[0].Weights.Length);
            Array.Copy(brain.Layers[1].Weights, genome.w2, brain.Layers[1].Weights.Length);
            Array.Copy(brain.Layers[2].Weights, genome.w3, brain.Layers[2].Weights.Length);
            bestGenome = genome;

            string genomeString = "[LAYER 1]\n";
            for(int i = 0; i < bestGenome.b1.Length; i++)
            {
                genomeString += $"[BIAS {i}] {bestGenome.b1[0]}\n";
            }
            for (int i = 0; i < bestGenome.w1.GetLength(0); i++)
            {
                for (int j = 0; j < bestGenome.w1.GetLength(1); j++)
                {
                    genomeString += $"[WEIGHT {i},{j}] {bestGenome.w1[i,j]}\n";
                }
            }
            genomeString += "[LAYER 2]\n";
            for (int i = 0; i < bestGenome.b2.Length; i++)
            {
                genomeString += $"[BIAS {i}] {bestGenome.b2[0]}\n";
            }
            for (int i = 0; i < bestGenome.w2.GetLength(0); i++)
            {
                for (int j = 0; j < bestGenome.w2.GetLength(1); j++)
                {
                    genomeString += $"[WEIGHT {i},{j}] {bestGenome.w2[i, j]}\n";
                }
            }
            genomeString += "[LAYER 3]\n";
            for (int i = 0; i < bestGenome.b3.Length; i++)
            {
                genomeString += $"[BIAS {i}] {bestGenome.b3[0]}\n";
            }
            for (int i = 0; i < bestGenome.w3.GetLength(0); i++)
            {
                for (int j = 0; j < bestGenome.w3.GetLength(1); j++)
                {
                    genomeString += $"[WEIGHT {i},{j}] {bestGenome.w3[i, j]}\n";
                }
            }
            File.WriteAllText("..\\..\\..\\..\\BestBrain.txt", genomeString);
        }
    }

    struct Genome
    {
        public float[] b1;
        public float[] b2;
        public float[] b3;
        public float[,] w1;
        public float[,] w2;
        public float[,] w3;
    }
}
