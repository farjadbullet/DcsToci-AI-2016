﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Classes
{
    public class Chromosome
    {
        public double ValueX { get; set; }
        public double ValueY { get; set; }
        public double FitnessValue => Math.Abs((2 * ValueX) + (3 * ValueY));
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

        public Chromosome BestChromosome { get; set; }
        public Chromosome AverageChromosome { get; set; }
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
            var firstParent = Chromosomes.First(m => m.AccumulatedFitness > random.NextDouble());
            var secondParent = Chromosomes.First(m => m.AccumulatedFitness > random.NextDouble());

            // Create OffSprings
            var firstOffSpring = new Chromosome
            {
                ValueX = firstParent.ValueX,
                ValueY = secondParent.ValueY,
            };
            var secondOffspring = new Chromosome
            {
                ValueX = firstParent.ValueY,
                ValueY = secondParent.ValueX,
            };
            Chromosomes.Add(firstOffSpring);
            Chromosomes.Add(secondOffspring);
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