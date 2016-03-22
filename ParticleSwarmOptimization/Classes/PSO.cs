using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSwarmOptimization.Classes
{
    public class GraphMapper
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
    public class SeriesMapper
    {
        public string Category { get; set; }
        public List<GraphMapper> GraphDataSource { get; set; }
    }
    public class Vector<T>
    {
        public T ValueX { get; set; }
        public T ValueY { get; set; }
        public T FitnessValue => ((dynamic)ValueX * ValueX) + ((dynamic)ValueY * ValueY);

        public Vector()
        {
            ValueX = (dynamic)0;
            ValueY = (dynamic)0;
        }
        public Vector(dynamic a, dynamic b)
        {
            ValueX = a;
            ValueY = b;
        }

        public static Vector<T> operator +(Vector<T> v1, Vector<T> v2)
        {
            return new Vector<T>(
                (dynamic)v1.ValueX + v2.ValueX,
                (dynamic)v1.ValueY + v2.ValueY);
        }
        public static Vector<T> operator +(Vector<T> v1, dynamic value)
        {
            return new Vector<T>(
                (dynamic)v1.ValueX + value,
                (dynamic)v1.ValueY + value);
        }
        public static Vector<T> operator -(Vector<T> v1, dynamic value)
        {
            return new Vector<T>(
                (dynamic)v1.ValueX - value,
                (dynamic)v1.ValueY - value);
        }
        public static Vector<T> operator -(Vector<T> v1, Vector<T> v2)
        {
            return new Vector<T>(
                (dynamic)v1.ValueX - v2.ValueX,
                (dynamic)v1.ValueY - v2.ValueY);
        }

        public static Vector<T> operator *(Vector<T> v1, dynamic value)
        {
            return new Vector<T>(
                (dynamic)v1.ValueX * value,
                (dynamic)v1.ValueY * value);
        }
        public static Vector<T> operator *(dynamic value, Vector<T> v1)
        {
            return new Vector<T>(
                (dynamic)v1.ValueX * value,
                (dynamic)v1.ValueY * value);
        }

    }
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

        public Vector<double> Velocity { get; set; }
        public Vector<double> LocalBest { get; set; }
        public Vector<double> GlobalBest { get; set; }

        public Chromosome()
        {
            Velocity = new Vector<double>(ValueX, ValueY);
            LocalBest = new Vector<double>(ValueX, ValueY);
            GlobalBest = new Vector<double>(ValueX, ValueY);
        }
        public void CalculateVelocity()
        {
            var q = NextDouble(new Random(), 0, 1);
            var x = 2*NextDouble(new Random(), 0, 1);
            var y = (LocalBest - new Vector<double>(ValueX, ValueY));
            var z = (2*NextDouble(new Random(), 0, 1)*(GlobalBest - new Vector<double>(ValueX, ValueY)));
            Velocity = Velocity +
                   (2 * NextDouble(new Random(), 0, 1) * (LocalBest - new Vector<double>(ValueX, ValueY)))
                   + (2 * NextDouble(new Random(), 0, 1) * (GlobalBest - new Vector<double>(ValueX, ValueY)));
        }

        public void UpdatePosition()
        {
            ValueX = Velocity.ValueX;
            ValueY = Velocity.ValueY;
        }

        public static double NextDouble(Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }
    }

    public class Population
    {
        #region Public Properties
        public List<Chromosome> Chromosomes { get; set; }
        public Vector<double> GlobalBest { get; set; }
        public Vector<double> LocalBest { get; set; }

        #endregion

        #region Constructor
        public Population(Random random)
        {
            Chromosomes = new List<Chromosome>();
            Enumerable.Range(1, 50).ToList().ForEach(m =>
            {
                var chromosome = new Chromosome
                {
                    ValueX = Chromosome.NextDouble(random, -2, 2),
                    ValueY = Chromosome.NextDouble(random, -5, 5),
                };
                chromosome.GlobalBest = new Vector<double>(chromosome.ValueX, chromosome.ValueY);
                chromosome.LocalBest = new Vector<double>(chromosome.ValueX, chromosome.ValueY);
                Chromosomes.Add(chromosome);
            });
            var best = Chromosomes.FirstOrDefault(m => m.FitnessValue == Chromosomes.Max(x => x.FitnessValue));

            if (best != null)
            {
                GlobalBest = best.GlobalBest;
                LocalBest = best.LocalBest;
            }
        }

        public Population(List<Chromosome> chromosomes)
        {
            Chromosomes = chromosomes;
        }
        #endregion

        #region Calculation Helpers


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

        #endregion
    }
}
