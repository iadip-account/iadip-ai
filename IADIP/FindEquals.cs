using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IADIP {
    public class FindEquals {
        public void preprocess(List<Flat> flats) {
            List<Flat> deleted = new List<Flat>();
            for (int i = 0; i < flats.Count; i++) {
                for (int j = i + 1; j < flats.Count; j++) {
                    if (flats[i].Cost == flats[j].Cost
                        && flats[i].Baths == flats[j].Baths
                        && flats[i].Beech == flats[j].Beech
                        && flats[i].City == flats[j].City
                        && flats[i].Firm == flats[j].Firm
                        && flats[i].Number == flats[j].Number
                        && flats[i].Space == flats[j].Space) {
                        deleted.Add(flats[j]);
                    }
                }
            }
            foreach (var flat in deleted) {
                flats.Remove(flat);
            }
        }
    }
}
