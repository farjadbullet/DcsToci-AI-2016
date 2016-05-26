using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPropagationAlgorithm.Classes
{
    public class Input
    {
        private double _valueX;
        public double ValueX
        {
            get { return _valueX; }
            set
            {
                if (value > 2)
                {
                    _valueX = 2;
                }
                else if (value < -2)
                {
                    _valueX = -2;
                }
                else
                {
                    _valueX = value;
                }
            }
        }

        private double _valueY;
        public double ValueY
        {
            get { return _valueY; }
            set
            {
                if (value > 5)
                {
                    _valueY = 5;
                }
                else if (value < -5)
                {
                    _valueY = -5;
                }
                else
                {
                    _valueY = value;
                }
            }
        }
    }
    public class NeuralNetwork
    {
        public Neuron NeuronX { get; set; }
        public Neuron NeuronY { get; set; }

        public Neuron HiddenNeuronX { get; set; }
        public Neuron HiddenNeuronY { get; set; }

        public Neuron OutputNeuron { get; set; }

        public double RosenBack => (NeuronX.Value * NeuronX.Value + NeuronY.Value * NeuronY.Value);

        public NeuralNetwork()
        {
            var random = new Random();

            // Generate Two Neurons
            NeuronX = new Neuron(-2, 2)
            {
                WeightX = NextDouble(random, 0, 1),
                WeightY = NextDouble(random, 0, 1)
            };
            NeuronY = new Neuron(-5, 5)
            {
                WeightX = NextDouble(random, 0, 1),
                WeightY = NextDouble(random, 0, 1)
            };

            var weightForHiddenNeuron = NextDouble(random, 0, 1);
            HiddenNeuronX = new Neuron(-2, 2)
            {
                //Value = ((NeuronX.Value * NeuronX.WeightX) + NeuronY.Value - NeuronY.WeightX).Sigmoid(),
                WeightX = weightForHiddenNeuron,
                WeightY = weightForHiddenNeuron
            };

            weightForHiddenNeuron = NextDouble(random, 0, 1);
            HiddenNeuronY = new Neuron(-5, 5)
            {
                //Value = ((NeuronX.Value * NeuronX.WeightY) + NeuronY.Value - NeuronY.WeightY).Sigmoid(),
                WeightX = weightForHiddenNeuron,
                WeightY = weightForHiddenNeuron
            };

            OutputNeuron = new Neuron(-29, 29);

            //// Generate Hidden Neurons using initial Neurons
            //GenerateHiddenLayer(random);

            //// Generate Output Neuron
            //GenerateOutputNeuron();
        }

        public void GenerateOutputNeuron()
        {
            OutputNeuron.Value =
                ((HiddenNeuronX.Value * HiddenNeuronX.WeightX) + (HiddenNeuronY.Value * HiddenNeuronY.WeightY)).Sigmoid();
        }

        public void GenerateHiddenLayer()
        {
            HiddenNeuronX.Value = ((NeuronX.Value * NeuronX.WeightX) + (NeuronY.Value * NeuronY.WeightX)).Sigmoid();
            HiddenNeuronY.Value = ((NeuronX.Value * NeuronX.WeightY) + (NeuronY.Value * NeuronY.WeightY)).Sigmoid();
        }

        public void AdjustWeights()
        {
            NeuronX.WeightX +=
                (0.2 * RosenBack - OutputNeuron.Value) * NeuronX.Value *
                HiddenNeuronX.Value * (1 - HiddenNeuronX.Value);

            NeuronX.WeightY +=
                (0.2 * RosenBack - OutputNeuron.Value) * NeuronX.Value *
                HiddenNeuronY.Value * (1 - HiddenNeuronY.Value);

            NeuronY.WeightX +=
                (0.2 * RosenBack - OutputNeuron.Value) * NeuronY.Value *
                HiddenNeuronX.Value * (1 - HiddenNeuronX.Value);

            NeuronY.WeightY +=
                (0.2 * RosenBack - OutputNeuron.Value) * NeuronY.Value *
                HiddenNeuronY.Value * (1 - HiddenNeuronY.Value);

            HiddenNeuronX.WeightX =
                HiddenNeuronX.WeightY =
                    HiddenNeuronX.WeightX +
                    (0.2 * RosenBack - OutputNeuron.Value) * HiddenNeuronX.Value * OutputNeuron.Value * (1 - OutputNeuron.Value);

            HiddenNeuronY.WeightX =
                HiddenNeuronY.WeightY =
                    HiddenNeuronY.WeightX +
                    (0.2 * RosenBack - OutputNeuron.Value) * HiddenNeuronY.Value * OutputNeuron.Value * (1 - OutputNeuron.Value);
        }

        public static double NextDouble(Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }
    }

    public class Neuron
    {
        private readonly int _lowerRange;
        private readonly int _higherRange;

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                if (value > _higherRange)
                {
                    _value = _higherRange;
                }
                else if (value < _lowerRange)
                {
                    _value = _lowerRange;
                }
                else
                {
                    _value = value;
                }
            }
        }

        private double _weightX;
        public double WeightX
        {
            get { return _weightX; }
            set
            {
                if (value > 1)
                {
                    _weightX = 1;
                }
                else if (value < 0)
                {
                    _weightX = 0;
                }
                else
                {
                    _weightX = value;
                }
            }
        }

        private double _weightY;
        public double WeightY
        {
            get { return _weightY; }
            set
            {
                if (value > 5)
                {
                    _weightY = 5;
                }
                else if (value < -5)
                {
                    _weightY = -5;
                }
                else
                {
                    _weightY = value;
                }
            }
        }

        public Neuron()
        { }
        public Neuron(int min, int max)
        {
            _lowerRange = min;
            _higherRange = max;
        }
    }

    public static class Helpers
    {
        public static double Sigmoid(this double self)
        {
            var x = (1 / (1 + Math.Pow(Math.E, (-1 * self))));
            return x;
        }
    }
}