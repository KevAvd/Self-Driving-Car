using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar.NeuralNet
{
    internal class CarBrain
    {
        Layer[] layers = new Layer[3];
        float distance = 0;

        public Layer[] Layers { get => layers; set => layers = value; }
        public float Distance { get => distance; set => distance = value; }

        public CarBrain() 
        {
            layers[0] = new Layer(8, 6, Layer.AcitvationFunction.ReLU);
            layers[1] = new Layer(6, 6, Layer.AcitvationFunction.ReLU);
            layers[2] = new Layer(6, 4, Layer.AcitvationFunction.Binary);
        }

        public float[] ProcessInput(float[] inputs)
        {
            layers[0].Inputs = inputs;
            layers[0].feedForward();
            layers[1].Inputs = layers[0].Outputs;
            layers[1].feedForward();
            layers[2].Inputs = layers[1].Outputs;
            layers[2].feedForward();
            return layers[2].Outputs;
        }

        public void Mutate()
        {
            foreach(Layer layer in layers)
            {
                layer.Mutate();
            }
        }

        public void Randomize()
        {
            foreach (Layer layer in layers)
            {
                layer.Randomize();
            }
        }
    }
}
