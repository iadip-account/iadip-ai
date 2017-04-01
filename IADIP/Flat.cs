using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class Flat {
        public int Number { get; private set; }
        public double Space { get; set; }
        public double Baths { get; set; }
        public double Beech { get; set; }
        public double Cost { get; set; }

        public string City { get; private set; }
        public string Firm { get; private set; }

        public Flat(int Number, double Space, double Bath, double Beech, double Cost, string City, string Firm) {
            this.Number = Number;
            this.Space = Space;
            this.Baths = Baths;
            this.Beech = Beech;
            this.Cost = Cost;
            this.City = City;
            this.Firm = Firm;
        }
    }

    public class FlatList {
        public List<Flat> Flats { get; private set; }

        public FlatList(List<Flat> Flats) {
            this.Flats = Flats;
        }
    }
}
