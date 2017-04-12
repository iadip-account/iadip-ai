using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private int[] layersMas;

        private NeuralNW NET;

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
            Flats.Sort(new FlatComparer());
            CollectionFlats = CollectionViewSource.GetDefaultView(Flats);
        }

        private bool parseLayers() {
            if (Layers != null) {
                string[] temp = Layers.Split(',');
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
            double kLern = Speed;

            double[] X = new double[3];
            double[] Y = new double[1];

            while (k < Maximum && kErr > Goal) {
                kErr = 0;

                foreach (Flat flat in flats) {
                    X[0] = flat.Space;
                    X[0] = flat.Baths;
                    X[0] = flat.Beech;
                    Y[0] = flat.Cost;
                    kErr += NET.LernNW(X, Y, kLern);
                }
                Training = "Выполнена " + (k + 1).ToString() + " итерация обучения (Текущая ошибка: " + Convert.ToString(kErr) + ")";

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
            Error = sum / flats.Count * 100;
        }

        public async Task Train() {
            if (!parseLayers()) {
                System.Windows.MessageBox.Show("Неверный ввод скрытых слоев");
                return;
            }
            if (Segment != null) {
                if (Segments.Where(q => q.Name == Segment.Name).ToList().Count > 1) {
                    System.Windows.MessageBox.Show("Названия сегментов не должны повторяться!");
                    return;
                }
                NET = new NeuralNW(3, layersMas);

                List<Flat> list = Flats.Where(q => q.Cost <= Segment.To && q.Cost >= Segment.From).ToList();

                INormalize norm = new NormalizeByDisplacement();
                flatsNormalize = norm.normalize(list);

                int test = (int)(flatsNormalize.Count * Tests / 100);
                Random rnd = new Random();
                int ind = rnd.Next(flatsNormalize.Count - test);
                List<Flat> testList = flatsNormalize.GetRange(ind, test);
                List<Flat> trainList = flatsNormalize.GetRange(0, ind)
                    .Concat(flatsNormalize.GetRange(ind + test, flatsNormalize.Count - ind - test))
                    .ToList();
                await Task.Run(() => Train(trainList));
                Testing(testList);
                NET.SaveNW(Segment.Name + ".nw");
            } else {
                System.Windows.MessageBox.Show("Выберите сегмент!");
                return;
            }
        }
    }
}
