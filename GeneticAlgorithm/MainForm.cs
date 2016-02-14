using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
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
            InitGraphs();
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

            for (int i = 0; i < 1; i++)
            {
                // Perform GA Operations
                initialPopulation.CalculateNormalizedFitness();
                initialPopulation.OrderChromosomesDescending();
                initialPopulation.CalculateAccumulatedFitness();
                initialPopulation.PerformCrossOver(random);
                initialPopulation.PerformMutation(random, random.NextDouble());
                initialPopulation.PerformTruncation();

                // Add Best And Average Result to respective lists
                averageResult.Add(initialPopulation.Chromosomes.Sum(m => m.FitnessValue) / initialPopulation.Chromosomes.Count);
                bestResult.Add(initialPopulation.Chromosomes.Max(m => m.FitnessValue));

                #region Add Chart
                CartesianArea cartesianArea1 = new CartesianArea();
                RadChartView chart = new RadChartView
                {
                    AreaDesign = cartesianArea1,
                    Dock = DockStyle.Fill,
                    Location = new Point(0, 356 * i),
                    Name = "radChartView1",
                    ShowGrid = false,
                    Size = new Size(515, 356),
                    TabIndex = 0,
                    Text = "radChartView1",
                    ShowLegend = true,

                };
                BarSeries lineSeries = new BarSeries();
                var dataSource = initialPopulation.Chromosomes;
                lineSeries.LegendTitle = "Population " + i;
                lineSeries.IsVisibleInLegend = true;
                lineSeries.PointSize = new SizeF(10, 10);
                lineSeries.BorderWidth = 2;
                lineSeries.CategoryMember = "ValueY";
                lineSeries.ValueMember = "ValueX";
                lineSeries.DataSource = dataSource;
                lineSeries.ShowLabels = true;
                lineSeries.CombineMode = ChartSeriesCombineMode.None;
                chart.Series.Add(lineSeries);
                chart.ShowSmartLabels = true;

                ((CartesianArea)chart.View.Area).ShowGrid = true;
                CartesianArea area = chart.GetArea<CartesianArea>();
                foreach (var series in chart.Series)
                {
                    int w = 1;
                    foreach (var uiChartElement in series.Children)
                    {
                        var dpe = (DataPointElement)uiChartElement;
                        dpe.IsVisible = false;
                        AnimatedPropertySetting setting = new AnimatedPropertySetting
                        {
                            StartValue = false,
                            EndValue = true,
                            Property = UIChartElement.IsVisibleProperty,
                            ApplyDelay = 40 + 40 * i,
                            NumFrames = 2
                        };
                        setting.ApplyValue(dpe);
                        i++;
                    }
                }
                area.ShowGrid = true;
                area.GridDesign.DrawHorizontalStripes = true;
                area.GridDesign.DrawVerticalStripes = true;
                this.Controls.Add(chart);
                #endregion
            }


        }
    }

}
