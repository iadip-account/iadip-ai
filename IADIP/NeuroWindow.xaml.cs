using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        private void Train_Click(object sender, RoutedEventArgs e) {
            if (!parseLayers()) {
                System.Windows.MessageBox.Show("Неверный ввод скрытых слоев");
                return;
            }

            speed = mvm.Speed;
            maximum = mvm.Maximum;
            tests = mvm.Tests;

            flats = mvm.flats;
            if (flats == null) {
                System.Windows.MessageBox.Show("Данные не загружены!");
                return;
            }

            NET = new NeuralNW(3, layersMas);

            Thread thread = new Thread(Train);
            thread.Start();
            NET.SaveNW("Neuro.nw");
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

        double speed;
        int maximum;
        List<Flat> flats;
        double tests;

        private void Train(List<Flat> flats) {
            double kErr = 1E256;
            int k = 0;
            double kLern = speed;

            double[] X = new double[3];
            double[] Y = new double[1];


            while (k < maximum) {
                kErr = 0;

                foreach (Flat flat in flats) {
                    X[0] = flat.Space;
                    X[0] = flat.Baths;
                    X[0] = flat.Beech;
                    Y[0] = flat.Cost;
                    kErr += NET.LernNW(X, Y, kLern);
                    Action action = () => mvm.Training = "Текущая ошибка: " + Convert.ToString(kErr) + "\r\n" + mvm.Training;
                    Dispatcher.BeginInvoke(action);
                }
                Action action1 = () => mvm.Training = "Выполнена " + (k + 1).ToString() + " итерация обучения" + "\r\n" + mvm.Training;
                Dispatcher.BeginInvoke(action1);
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
            Action action = () => mvm.Error = sum / flats.Count * 100;
            Dispatcher.BeginInvoke(action);
        }

        public void Train() {
            INormalize norm = new NormalizeByDisplacement();
            norm.normalize(flats);
            int test = (int)(flats.Count * tests / 100);
            Random rnd = new Random();
            int ind = rnd.Next(flats.Count - test);
            List<Flat> testList = flats.GetRange(ind, test);
            List<Flat> trainList = flats.GetRange(0, ind).Concat(flats.GetRange(ind + test, flats.Count - ind - test)).ToList();
            Train(trainList);
            Testing(testList);
        }
    }
}
