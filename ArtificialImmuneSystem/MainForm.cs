using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtificialImmuneSystem.Classes;
using Telerik.Charting;
using Telerik.WinControls.UI;

namespace ArtificialImmuneSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitGraphs();
        }

        public void InitGraphs()
        {
            Random random = new Random();

            // Initial Population

            // List to store best of every iteration
            var bestResult = new List<double>();

            // List to store average of every iteration
            var averageResult = new List<double>();

            for (int i = 0; i < 20; i++)
            {
                Population initialPopulation = new Population(random);
                List<GraphMapper> bestOfIteration = new List<GraphMapper>();
                List<GraphMapper> worstOfIteration = new List<GraphMapper>();
                List<GraphMapper> averageOfIteration = new List<GraphMapper>();
                initialPopulation.CalculateNormalizedFitness();
                initialPopulation.OrderChromosomesDescending();
                initialPopulation.CalculateAccumulatedFitness();
                for (int j = 0; j < 20; j++)
                {
                    #region Perform GA Operations

                    // Perform GA Operations


                    initialPopulation.CloneElements();
                    initialPopulation.PerformMutation(random);
                    initialPopulation.Production(10, random);



                    bestOfIteration.Add(new GraphMapper
                    {
                        Category = "G " + (j + 1).ToString(),
                        Value = initialPopulation.BestFitness,
                    });
                    worstOfIteration.Add(new GraphMapper
                    {
                        Category = "G " + (j + 1).ToString(),
                        Value = initialPopulation.WorstFitness,
                    });

                    averageOfIteration.Add(new GraphMapper
                    {
                        Category = "G " + (j + 1).ToString(),
                        Value = initialPopulation.AverageFitness,
                    });


                    #endregion
                }


                List<SeriesMapper> listOfSeries = new List<SeriesMapper>
                {
                    new SeriesMapper
                    {
                        Category = "Best of Iteration",
                        GraphDataSource = bestOfIteration,
                    },
                    new SeriesMapper
                    {
                        Category = "Worst of Iteration",
                        GraphDataSource = worstOfIteration,
                    },
                    new SeriesMapper
                    {
                        Category = "Average of Iteration",
                        GraphDataSource = averageOfIteration,
                    }
                };

                // Add Best And Average Result to respective lists
                averageResult.Add(initialPopulation.Chromosomes.Sum(m => m.FitnessValue) /
                                  initialPopulation.Chromosomes.Count);
                bestResult.Add(initialPopulation.Chromosomes.Max(m => m.FitnessValue));

                #region Add Charts dynamically to Form

                CartesianArea cartesianArea1 = new CartesianArea();
                int height = 0;
                if (i > 0)
                {
                    height = this.Controls[i - 1].Height + this.Controls[i - 1].Location.Y;
                }

                RadChartView chart = new RadChartView
                {
                    AreaDesign = cartesianArea1,
                    Location = new Point(0, height),
                    Name = "radChartView1",
                    ShowGrid = true,
                    Size = new Size(1300, 700),
                    TabIndex = 0,
                    Text = "radChartView1",
                    Title = "Iteration " + (i + 1).ToString(),
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
                chart.ExportToImage(@"C:\Users\Farjad\Desktop\TOCI2\AIS\" + chart.Title + ".png", chart.Size);

                #endregion
            }

            #region Best of All Chart

            var heightOfPreviousChart = this.Controls[this.Controls.Count - 1].Height +
                                        this.Controls[this.Controls.Count - 1].Location.Y;
            RadChartView chartForBestOfIterations = new RadChartView
            {
                AreaDesign = new CartesianArea(),
                Location = new Point(0, heightOfPreviousChart),
                Name = "radChartView1",
                ShowGrid = true,
                Size = new Size(1300, 700),
                TabIndex = 0,
                Title = "Best of all Iterations",
                ShowTitle = true,
                Text = "radChartView1",
                ShowLegend = true,
            };
            var dataSourceForBestResultsGraph = new List<GraphMapper>();
            int counter = 1;
            bestResult.ForEach(m =>
            {
                dataSourceForBestResultsGraph.Add(new GraphMapper
                {
                    Category = "P" + counter.ToString(),
                    Value = m,
                });
                counter += 1;
            });
            LineSeries lineSeriesForBestOfIteration = new LineSeries
            {
                LegendTitle = "Best of All Iterations",
                PointSize = new SizeF(10, 10),
                BorderWidth = 2,
                CategoryMember = "Category",
                ValueMember = "Value",
                DataSource = dataSourceForBestResultsGraph,
                ShowLabels = true,
                CombineMode = ChartSeriesCombineMode.None
            };
            chartForBestOfIterations.Series.Add(lineSeriesForBestOfIteration);
            chartForBestOfIterations.ShowSmartLabels = true;

            ((CartesianArea)chartForBestOfIterations.View.Area).ShowGrid = true;

            this.Controls.Add(chartForBestOfIterations);
            chartForBestOfIterations.ExportToImage(
                @"C:\Users\Farjad\Desktop\TOCI2\AIS\" + chartForBestOfIterations.Title + ".png",
                chartForBestOfIterations.Size);

            #endregion

            #region Average of All Chart

            RadChartView chartForAverageOfIterations = new RadChartView
            {
                AreaDesign = new CartesianArea(),
                //Anchor = AnchorStyles.Left,
                //Dock = DockStyle.Fill,
                Location = new Point(0, chartForBestOfIterations.Height + chartForBestOfIterations.Location.Y),
                Name = "radChartView1",
                ShowGrid = true,
                Size = new Size(1300, 700),
                Title = "Average of all Iterations",
                ShowTitle = true,
                TabIndex = 0,
                Text = "radChartView1",
                ShowLegend = true,

            };
            var dataSourceForAverageResultsGraph = new List<GraphMapper>();
            counter = 1;
            averageResult.ForEach(m =>
            {
                dataSourceForAverageResultsGraph.Add(new GraphMapper
                {
                    Category = "P" + counter,
                    Value = m,
                });
                counter += 1;
            });
            LineSeries lineSeriesForAverageOfIteration = new LineSeries
            {
                LegendTitle = "Average of All Iterations",
                PointSize = new SizeF(10, 10),
                BorderWidth = 2,
                CategoryMember = "Category",
                ValueMember = "Value",
                DataSource = dataSourceForAverageResultsGraph,
                ShowLabels = true,
                CombineMode = ChartSeriesCombineMode.None
            };
            chartForAverageOfIterations.Series.Add(lineSeriesForAverageOfIteration);
            chartForAverageOfIterations.ShowSmartLabels = true;

            ((CartesianArea)chartForAverageOfIterations.View.Area).ShowGrid = true;

            this.Controls.Add(chartForAverageOfIterations);
            chartForAverageOfIterations.ExportToImage(
                @"C:\Users\Farjad\Desktop\TOCI2\AIS\" + chartForAverageOfIterations.Title + ".png",
                chartForAverageOfIterations.Size);

            #endregion
        }
    }
}
