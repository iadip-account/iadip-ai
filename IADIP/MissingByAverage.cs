using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class MissingByAverage : IMissing {

        public void fillMissingValues(List<Flat> flats) {
            var dictionary = new Dictionary<string, List<Flat>>();

            foreach (Flat flat in flats) {
                if (!dictionary.ContainsKey(flat.City)) {
                    dictionary.Add(flat.City, new List<Flat>());
                }
                dictionary[flat.City].Add(flat);
            }


            foreach (var item in dictionary) {
                fillSpace(item);
                fillBathes(item);
                fillBeech(item);
                fillCost(item);
            }
        }

        private void fillSpace(KeyValuePair<string, List<Flat>> item) {
            List<Flat> tempList = item.Value.Where(q => q.Space == -1).ToList();
            List<Flat> tempListNotNull = item.Value.Where(q => q.Space != -1).ToList();
            double average = tempListNotNull.Sum(q => q.Space) / tempListNotNull.Count;
            foreach (Flat flat in tempList) {
                flat.Space = Math.Round(average, 3);
            }
        }

        private void fillBathes(KeyValuePair<string, List<Flat>> item) {
            List<Flat> tempList = item.Value.Where(q => q.Baths == -1).ToList();
            List<Flat> tempListNotNull = item.Value.Where(q => q.Baths != -1).ToList();
            double average = tempListNotNull.Sum(q => q.Baths) / tempListNotNull.Count;
            foreach (Flat flat in tempList) {
                flat.Baths = Math.Round(average, 3);
            }
        }

        private void fillBeech(KeyValuePair<string, List<Flat>> item) {
            List<Flat> tempList = item.Value.Where(q => q.Beech == -1).ToList();
            List<Flat> tempListNotNull = item.Value.Where(q => q.Beech != -1).ToList();
            double average = tempListNotNull.Sum(q => q.Beech) / tempListNotNull.Count;
            foreach (Flat flat in tempList) {
                flat.Beech = Math.Round(average, 3);
            }
        }

        private void fillCost(KeyValuePair<string, List<Flat>> item) {
            List<Flat> tempList = item.Value.Where(q => q.Cost == -1).ToList();
            List<Flat> tempListNotNull = item.Value.Where(q => q.Cost != -1).ToList();
            double average = tempListNotNull.Sum(q => q.Cost) / tempListNotNull.Count;
            foreach (Flat flat in tempList) {
                flat.Cost = Math.Round(average, 3);
            }
        }
    }
}
