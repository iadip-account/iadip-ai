using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml.Serialization;

namespace IADIP {
    public partial class NeuroWindow : Window {
        ModelViewModelNeuro mvm;

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
            try {
                if (mvm.Flats == null) {
                    System.Windows.MessageBox.Show("Данные не загружены!");
                    return;
                }
                await mvm.Train();
            } catch {
                System.Windows.MessageBox.Show("Ошибка ввода");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            mvm.Segments.Add(new Segment());
            mvm.CollectionSegments.Refresh();
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            XmlSerializer serialize = new XmlSerializer(typeof(List<Segment>));
            serialize.Serialize(new StreamWriter("segments.xml"), mvm.Segments);
        }


    }
}
