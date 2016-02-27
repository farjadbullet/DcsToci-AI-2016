using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Classes
{
    public class Chromosome
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
        public double FitnessValue => (ValueX * ValueX) + (ValueY * ValueY);
        public double NormalizedFitness { get; set; }
        public double AccumulatedFitness { get; set; }
    }

    public class Population
    {
        #region Public Properties
        public List<Chromosome> Chromosomes { get; set; }

        // Sum of all chromosome's fitness
        public double PopulationFitness => Chromosomes.Sum(m => m.FitnessValue);

        // Sum of all chromosome's Normalized Fitness
        public double NormalizedFitness => Chromosomes.Sum(m => m.NormalizedFitness);

        // Sum of all chromosome's Accumulated Fitness
        public double AccumulatedFitness => Chromosomes.Last().NormalizedFitness;

        public Chromosome BestChromosome
        {
            get { return Chromosomes.First(m => m.FitnessValue == Chromosomes.Max(x => x.FitnessValue)); }
        }
        public double BestFitness
        {
            get { return Chromosomes.Max(x => x.FitnessValue); }
        }

        public double AverageFitness
        {
            get { return Chromosomes.Sum(m => m.FitnessValue) / Chromosomes.Count; }
        }

        public double WorstFitness
        {
            get { return Chromosomes.Min(m => m.FitnessValue); }
        }

        #endregion

        #region Constructor
        public Population(Random random)
        {
            Chromosomes = new List<Chromosome>();
            Enumerable.Range(1, 50).ToList().ForEach(m =>
            {
                Chromosomes.Add(new Chromosome
                {
                    ValueX = NextDouble(random, -2, 2),
                    ValueY = NextDouble(random, -5, 5),
                });
            });
        }

        public Population(List<Chromosome> chromosomes)
        {
            Chromosomes = chromosomes;
        }
        #endregion

        #region Calculation Helpers
        public void CalculateNormalizedFitness()
        {
            Chromosomes.ForEach(m => { m.NormalizedFitness = m.FitnessValue / PopulationFitness; });
        }

        public void CalculateAccumulatedFitness()
        {
            double start = 0;
            Chromosomes.ForEach(m =>
            {
                m.AccumulatedFitness = start + m.NormalizedFitness;
                start += m.NormalizedFitness;
            });
        }

        #endregion

        #region Ordering Helpers
        public void OrderChromosomes()
        {
            Chromosomes = Chromosomes.OrderBy(m => m.FitnessValue).ToList();
        }
        public void OrderChromosomesDescending()
        {
            Chromosomes = Chromosomes.OrderByDescending(m => m.FitnessValue).ToList();
        }
        #endregion

        #region Genetic Algorithm Operations Helpers
        public void PerformCrossOver(Random random)
        {
            var firstParent = Chromosomes.FirstOrDefault(m => m.AccumulatedFitness > random.NextDouble());
            var secondParent = Chromosomes.FirstOrDefault(m => m.AccumulatedFitness > random.NextDouble());
            if (firstParent != null && secondParent != null)
            {
                var firstOffSpring = new Chromosome
                {
                    ValueX = firstParent.ValueX,
                    ValueY = secondParent.ValueY,
                };
                Chromosomes.Add(firstOffSpring);
            }
            // Create OffSprings
            if (secondParent != null && firstParent != null)
            {
                var secondOffspring = new Chromosome
                {
                    ValueX = firstParent.ValueY,
                    ValueY = secondParent.ValueX,
                };

                Chromosomes.Add(secondOffspring);
            }
        }

        public void PerformMutation(Random random, double mutationFactor)
        {
            Chromosomes[random.Next(0, 49)].ValueX += mutationFactor;
            Chromosomes[random.Next(0, 49)].ValueY -= mutationFactor;
        }

        public void PerformTruncation()
        {
            Chromosomes.Remove(Chromosomes.First(m => m.FitnessValue == Chromosomes.Min(x => x.FitnessValue)));
            Chromosomes.Remove(Chromosomes.First(m => m.FitnessValue == Chromosomes.Min(x => x.FitnessValue)));
        }
        #endregion
        static double NextDouble(Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }
    }
}
