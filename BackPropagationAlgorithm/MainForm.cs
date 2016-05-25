using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BackPropagationAlgorithm.Classes;

namespace BackPropagationAlgorithm
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitGraphs()
        {
            List<Input> inputList = new List<Input>();
            Random random = new Random();
            for (int i = 0; i < 700; i++)
            {
                inputList.Add(new Input
                {
                    ValueX = NeuralNetwork.NextDouble(random, -2, 2),
                    ValueY = NeuralNetwork.NextDouble(random, -5, 5),
                });

            }
        }
    }
}
