using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IADIP {
    /// <summary>
    /// Логика взаимодействия для CostWindow.xaml
    /// </summary>
    public partial class CostWindow : Window {
        NeuralNW NET;
        ModelViewModelCost mvm;
        public CostWindow() {
            InitializeComponent();
            if (!File.Exists("Neuro.nw")) {
                MessageBox.Show("Нейронная сеть не найдена");
                return;
            }
            NET = new NeuralNW("Neuro.nw", false);
            mvm = new ModelViewModelCost();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            double[] Y = new double[1];
            double[] X = new double[3];

            X[0] = mvm.Area;
            X[0] = mvm.Bathes;
            X[0] = mvm.Beech;
            NET.NetOUT(X, out Y);

            mvm.Cost = Y[0];
        }
    }
}
