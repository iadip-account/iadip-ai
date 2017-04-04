using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IADIP {
    public class ModelViewModelNeuro : INotifyPropertyChanged {
        protected void NeuroPropertyChanged(string p) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string path;
        private double speed;
        private string layers;
        private int maximum;
        private int iterations;
        private double tests;
        private string training;
        private double error;

        public List<Flat> flats;

        public string Path {
            get {
                return path;
            }
            set {
                path = value;
                NeuroPropertyChanged("Path");
            }
        }

        public string Layers {
            get {
                return layers;
            }
            set {
                layers = value;
                NeuroPropertyChanged("Layers");
            }
        }

        public string Training {
            get {
                return training;
            }
            set {
                training = value;
                NeuroPropertyChanged("Training");
            }
        }

        public double Speed {
            get {
                return speed;
            }
            set {
                speed = value;
                NeuroPropertyChanged("Speed");
            }
        }

        public double Tests {
            get {
                return tests;
            }
            set {
                tests = value;
                NeuroPropertyChanged("Tests");
            }
        }

        public double Error {
            get {
                return error;
            }
            set {
                error = value;
                NeuroPropertyChanged("Error");
            }
        }

        public int Maximum {
            get {
                return maximum;
            }
            set {
                maximum = value;
                NeuroPropertyChanged("Maximum");
            }
        }

        public int Iterations {
            get {
                return iterations;
            }
            set {
                iterations = value;
                NeuroPropertyChanged("Iterations");
            }
        }

        public void SelectPath() {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Path = fd.SelectedPath;
            }
        }

        public void Read() {
            string[] pathes = System.IO.Directory.GetFiles(Path);
            ExcelReader er = new ExcelReader(pathes);
            flats = er.getAll();
        }

       
    }

}
