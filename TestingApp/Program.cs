using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp
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
        public List<Chromosome> Chromosomes { get; set; }
        public Chromosome BestChromosome { get; set; }
        public Chromosome AverageChromosome { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            var list = GetRandomChromoSomeList(random);
            var resultList = new List<List<Chromosome>>();
            var bestResult = new List<double>();
            var averageResult = new List<double>();
            list = list.OrderByDescending(m => m.FitnessValue).ToList();
            for (int i = 0; i < 20; i++)
            {
                var fitnessValuesSum = list.Sum(m => m.FitnessValue);

                // calculate normalized fitness
                list.ForEach(m => { m.NormalizedFitness = m.FitnessValue / fitnessValuesSum; });

                // should be equal to 1
                var sumOfNormalizedFitness = list.Sum(m => m.NormalizedFitness);

                list = list.OrderByDescending(m => m.FitnessValue).ToList();

                // calculate accumulated fitness value
                double start = 0;
                list.ForEach(m =>
                {
                    m.AccumulatedFitness = start + m.NormalizedFitness;
                    start += m.NormalizedFitness;
                });

                // should be 1
                var lastAccumulatedFitness = list.Last().AccumulatedFitness;

                var firstParent = list.First(m => m.AccumulatedFitness > random.NextDouble());
                var secondParent = list.First(m => m.AccumulatedFitness > random.NextDouble());

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

                list.Add(firstOffSpring);
                list.Add(secondOffspring);
                list.Remove(list.First(m => m.FitnessValue == list.Min(x => x.FitnessValue)));
                list.Remove(list.First(m => m.FitnessValue == list.Min(x => x.FitnessValue)));

                // Mutation
                list[random.Next(0, 49)].ValueX += 2;
                list[random.Next(0, 49)].ValueY -= 2;
                resultList.Add(list);
                averageResult.Add(list.Sum(m => m.FitnessValue)/list.Count);
                bestResult.Add(list.Max(m => m.FitnessValue));
            }

        }


        private static List<Chromosome> GetRandomChromoSomeList(Random random)
        {
            List<Chromosome> list = new List<Chromosome>();
            Enumerable.Range(1, 50).ToList().ForEach(m =>
            {
                list.Add(new Chromosome
                {
                    ValueX = NextDouble(random, -2, 2),
                    ValueY = NextDouble(random, -5, 5),
                });
            });
            return list;
        }

        static double NextDouble(Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }
    }
}
