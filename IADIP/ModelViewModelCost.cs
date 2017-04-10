using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Xml.Serialization;

namespace IADIP {
    public class ModelViewModelCost : INotifyPropertyChanged {
        protected void CostPropertyChanged(string p) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ModelViewModelCost() {
            XmlSerializer serialize = new XmlSerializer(typeof(List<Segment>));
            Segments = (List<Segment>)serialize.Deserialize(new StreamReader("segments.xml"));
            CollectionSegments = CollectionViewSource.GetDefaultView(Segments);
        }

        private double area;
        private double bathes;
        private double beech;
        private double cost;

        private List<Segment> segments;

        private ICollectionView collectionSegments;

        private Segment segment;

        public List<Segment> Segments {
            get {
                return segments;
            }
            set {
                segments = value;
                CostPropertyChanged("Segments");
            }
        }

        public ICollectionView CollectionSegments {
            get {
                return collectionSegments;
            }
            set {
                collectionSegments = value;
                CostPropertyChanged("CollectionSegments");
            }
        }

        public Segment Segment {
            get {
                return segment;
            }
            set {
                segment = value;
                CostPropertyChanged("Segment");
            }
        }

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
