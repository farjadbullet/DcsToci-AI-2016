using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BackPropagationAlgorithm.Classes;
using Telerik.Charting;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace BackPropagationAlgorithm
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
            InitGraphs();
        }

        private void InitGraphs()
        {
            List<Input> inputList = new List<Input>();
            List<double> actualList = new List<double>();
            List<double> testedList = new List<double>();
            Random random = new Random();
            NeuralNetwork neuralNetwork = new NeuralNetwork();
            for (int i = 0; i < 2000; i++)
            {
                var input = new Input
                {
                    ValueX = NeuralNetwork.NextDouble(random, -2, 2),
                    ValueY = NeuralNetwork.NextDouble(random, -5, 5),
                };
                inputList.Add(input);

                neuralNetwork.NeuronX.Value = input.ValueX;
                neuralNetwork.NeuronY.Value = input.ValueY;

                neuralNetwork.GenerateHiddenLayer();
                neuralNetwork.GenerateOutputNeuron();

                if (Math.Abs(neuralNetwork.RosenBack - neuralNetwork.OutputNeuron.Value) <= 0.05)
                {
                    // Finished
                    //RadMessageBox.Show("Weights adjusted for Iteration " + i);
                }
                else
                {
                    // Adjust Weights
                    neuralNetwork.AdjustWeights();
                }
            }
            for (int i = 0; i < 300; i++)
            {
                var input = new Input
                {
                    ValueX = NeuralNetwork.NextDouble(random, -2, 2),
                    ValueY = NeuralNetwork.NextDouble(random, -5, 5),
                };
                inputList.Add(input);

                neuralNetwork.NeuronX.Value = input.ValueX;
                neuralNetwork.NeuronY.Value = input.ValueY;

                neuralNetwork.GenerateHiddenLayer();
                neuralNetwork.GenerateOutputNeuron();
                if (Math.Abs(neuralNetwork.RosenBack - neuralNetwork.OutputNeuron.Value) <= 0.05)
                {
                    // Finished
                    //RadMessageBox.Show("Weights adjusted for Iteration " + i);
                }
                else
                {
                    // Adjust Weights
                    neuralNetwork.AdjustWeights();
                }
                testedList.Add(neuralNetwork.OutputNeuron.Value);
                actualList.Add(neuralNetwork.RosenBack);
            }

            CartesianArea cartesianArea1 = new CartesianArea();
            int height = 0;
            RadChartView chart = new RadChartView
            {
                AreaDesign = cartesianArea1,
                Location = new Point(0, height),
                Name = "radChartView1",
                ShowGrid = true,
                Size = new Size(1300, 700),
                TabIndex = 0,
                Text = "radChartView1",
                //Title = "Iteration " + (i + 1).ToString(),
                ShowTitle = true,
                ShowLegend = true,
            };
            int count = 0;
            var actualGraphMapper = actualList.Take(50).Select(m => new GraphMapper
            {
                Category = (count++).ToString(),
                Value = m
            }).ToList();

            count = 0;
            var testGraphMapper = testedList.Take(50).Select(m => new GraphMapper
            {
                Category = (count++).ToString(),
                Value = m
            }).ToList();

            LineSeries rosenBackSeries = new LineSeries
            {
                LegendTitle = "RosenBack Function",
                PointSize = new SizeF(10, 10),
                BorderWidth = 2,
                CategoryMember = "Category",
                ValueMember = "Value",
                DataSource = actualGraphMapper,
                ShowLabels = true,
                CombineMode = ChartSeriesCombineMode.None
            };
            chart.Series.Add(rosenBackSeries);
            LineSeries trainedSeries = new LineSeries
            {
                LegendTitle = "Algo Result",
                PointSize = new SizeF(10, 10),
                BorderWidth = 2,
                CategoryMember = "Category",
                ValueMember = "Value",
                DataSource = testGraphMapper,
                ShowLabels = true,
                CombineMode = ChartSeriesCombineMode.None
            };
            chart.Series.Add(trainedSeries);
            chart.ShowSmartLabels = true;

            ((CartesianArea)chart.View.Area).ShowGrid = true;

            this.Controls.Add(chart);
            chart.ExportToImage(@"C:\Users\Farjad\Desktop\TOCI2\NN\" + chart.Title + ".png", chart.Size);
        }
    }
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
}
