using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class PreprocessingFindSolded : IPreprocessing {

        public List<Flat> preprocess(List<FlatList> arrays) {
            List<Flat> flats = new List<Flat>();
            int k = 0;
            foreach (var array in arrays) {
                k++;
                foreach (var flat in array.Flats) {
                    bool sold = false;
                    for (int i = k; i < arrays.Count; i++) {
                        if (arrays[i].Flats.Where(q => q.Number == flat.Number && q.Firm == flat.Firm).FirstOrDefault() == null) {
                            sold = true;
                            break;
                        } else {
                            arrays[i].Flats.RemoveAll(q => q.Number == flat.Number && q.Firm == flat.Firm);
                        }
                    }
                    if (sold) {
                        flats.Add(flat);
                    }
                }
            }
            return flats;
        }
    }
}
