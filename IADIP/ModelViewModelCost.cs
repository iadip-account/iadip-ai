using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IADIP {
    public class ModelViewModelCost : INotifyPropertyChanged {
        protected void CostPropertyChanged(string p) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private double area;
        private double bathes;
        private double beech;
        private double cost;

        public double Area {
            get {
                return area;
            }
            set {
                area = value;
                CostPropertyChanged("Area");
            }
        }

        public double Bathes {
            get {
                return bathes;
            }
            set {
                bathes = value;
                CostPropertyChanged("Bathes");
            }
        }

        public double Beech {
            get {
                return beech;
            }
            set {
                beech = value;
                CostPropertyChanged("Beech");
            }
        }

        public double Cost {
            get {
                return cost;
            }
            set {
                cost = value;
                CostPropertyChanged("Cost");
            }
        }
    }
}
