using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParticleSwarmOptimization.Classes;
using Telerik.Charting;
using Telerik.WinControls.UI;

namespace ParticleSwarmOptimization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Random random = new Random();
            // Initial Population
            Population initialPopulation = new Population(random);

            // List to store best of every iteration
            var bestResult = new List<double>();

            // List to store average of every iteration
            var averageResult = new List<double>();
            List<GraphMapper> globalBestOfIteration = new List<GraphMapper>();
            List<GraphMapper> localBestOfIteration = new List<GraphMapper>();
            List<GraphMapper> averageOfIteration = new List<GraphMapper>();

            for (int i = 0; i < 20; i++)
            {
                List<GraphMapper> populationFitnessValue = new List<GraphMapper>();
                List<GraphMapper> populationGFitnessValue = new List<GraphMapper>();

                foreach (Chromosome chromosome in initialPopulation.Chromosomes)
                {
                    if (chromosome.FitnessValue > initialPopulation.LocalBest.FitnessValue)
                    {
                        initialPopulation.LocalBest.ValueX = chromosome.ValueX;
                        initialPopulation.LocalBest.ValueY = chromosome.ValueY;
                    }


                    if (initialPopulation.LocalBest.FitnessValue > initialPopulation.GlobalBest.FitnessValue)
                    {
                        initialPopulation.GlobalBest = initialPopulation.LocalBest;
                    }
                }
                foreach (Chromosome chromosome in initialPopulation.Chromosomes)
                {
                    chromosome.CalculateVelocity();
                    chromosome.UpdatePosition();

                    populationFitnessValue.Add(new GraphMapper
                    {
                        Category = "C " + (initialPopulation.Chromosomes.IndexOf(chromosome) + 1).ToString(),
                        Value = chromosome.FitnessValue,
                    });

                    populationGFitnessValue.Add(new GraphMapper
                    {
                        Category = "C " + (initialPopulation.Chromosomes.IndexOf(chromosome) + 1).ToString(),
                        Value = chromosome.GlobalBest.FitnessValue,
                    });
                }

                List<SeriesMapper> listOfGraphMappers = new List<SeriesMapper>
                {
                    new SeriesMapper
                    {
                        Category = "Position Fitness Value",
                        GraphDataSource = populationFitnessValue,
                    },
                    new SeriesMapper
                    {
                        Category = "Global Best Fitness Value",
                        GraphDataSource = populationGFitnessValue,
                    },
                };

                #region Add Charts dynamically to Form

                CartesianArea cartesianArea = new CartesianArea();
                int heightGraph = 0;
                if (i > 0)
                {
                    heightGraph = this.Controls[i - 1].Height + this.Controls[i - 1].Location.Y;
                }

                RadChartView radChart = new RadChartView
                {
                    AreaDesign = cartesianArea,
                    Location = new Point(0, heightGraph),
                    Name = "radChartView1",
                    ShowGrid = true,
                    Size = new Size(1300, 700),
                    TabIndex = 0,
                    Text = "radChartView1",
                    Title = "Iteration " + (i + 1).ToString(),
                    ShowTitle = true,
                    ShowLegend = true,
                };
                foreach (SeriesMapper graphMappers in listOfGraphMappers)
                {
                    LineSeries series = new LineSeries
                    {
                        LegendTitle = graphMappers.Category,
                        PointSize = new SizeF(10, 10),
                        BorderWidth = 2,
                        CategoryMember = "Category",
                        ValueMember = "Value",
                        DataSource = graphMappers.GraphDataSource,
                        ShowLabels = true,
                        CombineMode = ChartSeriesCombineMode.None
                    };
                    radChart.Series.Add(series);
                    radChart.ShowSmartLabels = true;

                    ((CartesianArea)radChart.View.Area).ShowGrid = true;
                }
                this.Controls.Add(radChart);
                radChart.ExportToImage(@"C:\Users\Farjad\Desktop\TOCI2\" + radChart.Title + ".png", radChart.Size);
                #endregion

                globalBestOfIteration.Add(new GraphMapper
                {
                    Category = "G " + (i + 1).ToString(),
                    Value = initialPopulation.LocalBest.FitnessValue,
                    //Value = initialPopulation.Chromosomes.FirstOrDefault(m => m.FitnessValue == initialPopulation.Chromosomes.Max(w => w.FitnessValue)).FitnessValue,
                });
                localBestOfIteration.Add(new GraphMapper
                {
                    Category = "G " + (i + 1).ToString(),
                    Value = initialPopulation.Chromosomes.Max(m => m.FitnessValue),
                });
                averageOfIteration.Add(new GraphMapper
                {
                    Category = "G " + (i + 1).ToString(),
                    Value = initialPopulation.Chromosomes.Average(m => m.FitnessValue),
                });

            }

            var x = 0;
            List<SeriesMapper> listOfSeries = new List<SeriesMapper>
                {
                    new SeriesMapper
                    {
                        Category = "Global Best of Iteration",
                        GraphDataSource = globalBestOfIteration,
                    },
                    new SeriesMapper
                    {
                        Category = "Local Best of Iteration",
                        GraphDataSource = localBestOfIteration,
                    },
                     new SeriesMapper
                    {
                        Category = "Average Best of Iteration",
                        GraphDataSource = averageOfIteration,
                    },
                };

            #region Add Charts dynamically to Form

            CartesianArea cartesianArea1 = new CartesianArea();
            int height = 0;
            height = this.Controls[Controls.Count - 1].Height + this.Controls[Controls.Count - 1].Location.Y;

            RadChartView chart = new RadChartView
            {
                AreaDesign = cartesianArea1,
                Location = new Point(0, height),
                Name = "radChartView1",
                ShowGrid = true,
                Size = new Size(1300, 700),
                TabIndex = 0,
                Text = "radChartView1",
                Title = "Summary Graph",
                ShowTitle = true,
                ShowLegend = true,
            };
            foreach (SeriesMapper graphMappers in listOfSeries)
            {
                LineSeries series = new LineSeries
                {
                    LegendTitle = graphMappers.Category,
                    PointSize = new SizeF(10, 10),
                    BorderWidth = 2,
                    CategoryMember = "Category",
                    ValueMember = "Value",
                    DataSource = graphMappers.GraphDataSource,
                    ShowLabels = true,
                    CombineMode = ChartSeriesCombineMode.None
                };
                chart.Series.Add(series);
                chart.ShowSmartLabels = true;

                ((CartesianArea)chart.View.Area).ShowGrid = true;
            }
            this.Controls.Add(chart);
            chart.ExportToImage(@"C:\Users\Farjad\Desktop\TOCI2\" + chart.Title + ".png", chart.Size);
            #endregion


        }
    }
}
