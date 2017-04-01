using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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
        private string training;
        private double error;

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

    }
}
