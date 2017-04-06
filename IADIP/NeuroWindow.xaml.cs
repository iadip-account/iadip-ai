using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IADIP {
    public partial class NeuroWindow : Window {
        ModelViewModelNeuro mvm;
        private NeuralNW NET;
        private int[] layersMas;

        public NeuroWindow() {
            InitializeComponent();

            mvm = new ModelViewModelNeuro();

            DataContext = mvm;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            mvm.SelectPath();
        }

        private void Read_Click(object sender, RoutedEventArgs e) {
            mvm.Read();
            System.Windows.MessageBox.Show("Данные загружены");
        }

        private async void Train_Click(object sender, RoutedEventArgs e) {
            if (!parseLayers()) {
                System.Windows.MessageBox.Show("Неверный ввод скрытых слоев");
                return;
            }

            if (mvm.flats == null) {
                System.Windows.MessageBox.Show("Данные не загружены!");
                return;
            }

            NET = new NeuralNW(3, layersMas);
            await Train();
        }

        private bool parseLayers() {
            if (mvm.Layers != null) {
                string[] temp = mvm.Layers.Split(',');
                layersMas = new int[temp.Length + 1];
                int a = 0;
                for (int i = 0; i < temp.Length; i++) {
                    if (Int32.TryParse(temp[i].Trim(), out a)) {
                        layersMas[i] = a;
                    } else {
                        return false;
                    }
                }
            } else {
                layersMas = new int[1];
            }
            layersMas[layersMas.Length - 1] = 1;
            return true;
        }


        private void Train(List<Flat> flats) {
            double kErr = 1E256;
            int k = 0;
            double kLern = mvm.Speed;

            double[] X = new double[3];
            double[] Y = new double[1];

            while (k < mvm.Maximum) {
                kErr = 0;

                foreach (Flat flat in flats) {
                    X[0] = flat.Space;
                    X[0] = flat.Baths;
                    X[0] = flat.Beech;
                    Y[0] = flat.Cost;
                    kErr += NET.LernNW(X, Y, kLern);
                    mvm.Training = "Текущая ошибка: " + Convert.ToString(kErr) + "\r\n" + mvm.Training;
                }
                mvm.Training = "Выполнена " + (k + 1).ToString() + " итерация обучения" + "\r\n" + mvm.Training;
                k++;
            }
        }

        private void Testing(List<Flat> flats) {
            double sum = 0;
            double[] Y = new double[1];
            double[] X = new double[3];

            foreach (Flat flat in flats) {
                X[0] = flat.Space;
                X[0] = flat.Baths;
                X[0] = flat.Beech;
                NET.NetOUT(X, out Y);
                sum += Math.Abs(flat.Cost - Y[0]) / flat.Cost;
            }
            mvm.Error = sum / flats.Count * 100;
        }

        public async Task Train() {
            INormalize norm = new NormalizeByDisplacement();
            norm.normalize(mvm.flats);
            int test = (int)(mvm.flats.Count * mvm.Tests / 100);
            Random rnd = new Random();
            int ind = rnd.Next(mvm.flats.Count - test);
            List<Flat> testList = mvm.flats.GetRange(ind, test);
            List<Flat> trainList = mvm.flats.GetRange(0, ind).Concat(mvm.flats.GetRange(ind + test, mvm.flats.Count - ind - test)).ToList();
            await Task.Run(() => Train(mvm.flats));
            Testing(testList);
            NET.SaveNW("Neuro.nw");
        }
    }
}
