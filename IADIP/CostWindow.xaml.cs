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

            mvm = new ModelViewModelCost();

            DataContext = mvm;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (mvm.Segment != null) {
                if (!File.Exists(mvm.Segment.Name + ".nw")) {
                    MessageBox.Show("Нейронная сеть для данного сегмента не найдена");
                    return;
                }
                NET = new NeuralNW(mvm.Segment.Name + ".nw", false);

                double[] Y = new double[1];
                double[] X = new double[3];

                X[0] = NormalizeByDisplacement.displacement(mvm.Area,  Properties.Settings.Default.maxSpace);
                X[0] = NormalizeByDisplacement.displacement(mvm.Bathes, Properties.Settings.Default.maxSpace);
                X[0] = NormalizeByDisplacement.displacement(mvm.Beech, Properties.Settings.Default.maxBeech);
                NET.NetOUT(X, out Y);

                mvm.Cost = NormalizeByDisplacement.deDisplacement(Y[0], Properties.Settings.Default.maxCost);
            } else {
                MessageBox.Show("Выберите сегмент");
            }
        }
    }
}
