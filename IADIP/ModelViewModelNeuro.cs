using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace IADIP {
    public class ModelViewModelNeuro : INotifyPropertyChanged {
        protected void NeuroPropertyChanged(string p) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        public ModelViewModelNeuro() {
            XmlSerializer serialize = new XmlSerializer(typeof(List<Segment>));
            Segments = (List<Segment>)serialize.Deserialize(new StreamReader("segments.xml"));
            CollectionSegments = CollectionViewSource.GetDefaultView(Segments);
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
        private double goal;

        private List<Flat> flats;
        public List<Flat> flatsNormalize;

        private List<Segment> segments;

        private ICollectionView collectionFlats;
        private ICollectionView collectionSegments;

        private Segment segment;

        public List<Segment> Segments {
            get {
                return segments;
            }
            set {
                segments = value;
                NeuroPropertyChanged("Segments");
            }
        }

        public List<Flat> Flats {
            get {
                return flats;
            }
            set {
                flats = value;
                NeuroPropertyChanged("Flats");
            }
        }

        public ICollectionView CollectionFlats {
            get {
                return collectionFlats;
            }
            set {
                collectionFlats = value;
                NeuroPropertyChanged("CollectionFlats");
            }
        }

        public ICollectionView CollectionSegments {
            get {
                return collectionSegments;
            }
            set {
                collectionSegments = value;
                NeuroPropertyChanged("CollectionSegments");
            }
        }

        public string Path {
            get {
                return path;
            }
            set {
                path = value;
                NeuroPropertyChanged("Path");
            }
        }

        public Segment Segment {
            get {
                return segment;
            }
            set {
                segment = value;
                NeuroPropertyChanged("Segment");
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

        public double Goal {
            get {
                return goal;
            }
            set {
                goal = value;
                NeuroPropertyChanged("Goal");
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
            Flats = er.getAll();
            CollectionFlats = CollectionViewSource.GetDefaultView(Flats);
        }
    }
}
