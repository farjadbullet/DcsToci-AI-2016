using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using GeneticAlgorithm.Classes;
using Telerik.Charting;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace GeneticAlgorithm
{

    public partial class MainForm : RadForm
    {
        public MainForm()
        {
            InitializeComponent();

        }


        public void InitGraphs()
        {
            Random random = new Random();
            // Initial Population
            Population initialPopulation = new Population(random);

            // List to store best of every iteration
            var bestResult = new List<double>();

            // List to store average of every iteration
            var averageResult = new List<double>();

            for (int i = 0; i < 20; i++)
            {
                #region Perform GA Operations

                // Perform GA Operations
                List<GraphMapper> bestOfIteration = new List<GraphMapper>();
                initialPopulation.CalculateNormalizedFitness();
                initialPopulation.OrderChromosomesDescending();
                initialPopulation.CalculateAccumulatedFitness();
                for (int j = 0; j < 20; j++)
                {
                    initialPopulation.PerformCrossOver(random);
                    if (j % 2 == 0)
                    {
                        initialPopulation.PerformMutation(random, random.NextDouble());
                    }
                    initialPopulation.PerformTruncation();
                    bestOfIteration.Add(new GraphMapper
                    {
                        Category = "G " + (j + 1).ToString(),
                        Value = initialPopulation.BestFitness,
                    });

                }

                #endregion

                // Add Best And Average Result to respective lists
                averageResult.Add(initialPopulation.Chromosomes.Sum(m => m.FitnessValue) /
                        initialPopulation.Chromosomes.Count);
                bestResult.Add(initialPopulation.Chromosomes.Max(m => m.FitnessValue));

                #region Add Charts dynamically to Form

                CartesianArea cartesianArea1 = new CartesianArea();
                var height = 700 * i;
                RadChartView chart = new RadChartView
                {
                    AreaDesign = cartesianArea1,
                    //Anchor = AnchorStyles.Left,
                    //Dock = DockStyle.Fill,
                    Location = new Point(0, height),
                    Name = "radChartView1",
                    ShowGrid = true,
                    Size = new Size(1300, 700),
                    TabIndex = 0,
                    Text = "radChartView1",
                    //ThemeName = visualStudio2012DarkTheme1.ThemeName,
                    ShowLegend = true,

                };
                LineSeries lineSeries = new LineSeries();
                var dataSource = bestOfIteration;
                lineSeries.LegendTitle = "Population " + (i + 1);
                //lineSeries.IsVisibleInLegend = true;
                lineSeries.PointSize = new SizeF(10, 10);
                lineSeries.BorderWidth = 2;
                lineSeries.CategoryMember = "Category";
                lineSeries.ValueMember = "Value";
                lineSeries.DataSource = dataSource;
                lineSeries.ShowLabels = true;
                lineSeries.CombineMode = ChartSeriesCombineMode.None;
                chart.Series.Add(lineSeries);
                chart.ShowSmartLabels = true;

                ((CartesianArea)chart.View.Area).ShowGrid = true;

                this.Controls.Add(chart);

                #endregion
            }

            #region Best of All Chart

            RadChartView chartForBestOfIterations = new RadChartView
            {
                AreaDesign = new CartesianArea(),
                //Anchor = AnchorStyles.Left,
                //Dock = DockStyle.Fill,
                Location = new Point(0, 700 * 20),
                Name = "radChartView1",
                ShowGrid = true,
                Size = new Size(1300, 700),
                TabIndex = 0,
                Text = "radChartView1",
                //ThemeName = visualStudio2012DarkTheme1.ThemeName,
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
            //lineSeries.IsVisibleInLegend = true;
            chartForBestOfIterations.Series.Add(lineSeriesForBestOfIteration);
            chartForBestOfIterations.ShowSmartLabels = true;

            ((CartesianArea)chartForBestOfIterations.View.Area).ShowGrid = true;

            this.Controls.Add(chartForBestOfIterations);
            #endregion

            #region Average of All Chart

            RadChartView chartForAverageOfIterations = new RadChartView
            {
                AreaDesign = new CartesianArea(),
                //Anchor = AnchorStyles.Left,
                //Dock = DockStyle.Fill,
                Location = new Point(0, 700 * 21),
                Name = "radChartView1",
                ShowGrid = true,
                Size = new Size(1300, 700),
                TabIndex = 0,
                Text = "radChartView1",
                //ThemeName = visualStudio2012DarkTheme1.ThemeName,
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
            //lineSeries.IsVisibleInLegend = true;
            chartForAverageOfIterations.Series.Add(lineSeriesForAverageOfIteration);
            chartForAverageOfIterations.ShowSmartLabels = true;

            ((CartesianArea)chartForAverageOfIterations.View.Area).ShowGrid = true;

            this.Controls.Add(chartForAverageOfIterations);
            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitGraphs();
        }
    }
    public class GraphMapper
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
}
