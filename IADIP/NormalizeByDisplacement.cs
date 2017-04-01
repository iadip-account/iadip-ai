using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class NormalizeByDisplacement : INormalize {
        const int A = 0;
        const int B = 1;

        public void normalize(List<Flat> flats) {
            double minSpace = flats.Min(q => q.Space);
            double maxSpace = flats.Max(q => q.Space);
            double minBathes = flats.Min(q => q.Baths);
            double maxBathes = flats.Max(q => q.Baths);
            double minBeech = flats.Min(q => q.Beech);
            double maxBeech = flats.Max(q => q.Beech);
            double minCost = flats.Min(q => q.Cost);
            double maxCost = flats.Max(q => q.Cost);

            save (minSpace, maxSpace, minBathes, maxBathes, minBeech, maxBeech, minCost, maxCost);

            foreach (Flat flat in flats) {
                normalizeSpace(flat, minSpace, maxSpace);
                normalizeBathes(flat, minBathes, maxBathes);
                normalizeBeech(flat, minBeech, maxBeech);
                normalizeCost(flat, minCost, maxCost);
            }
        }

        private void save
            (
            double minSpace,
            double maxSpace,
            double minBathes,
            double maxBathes,
            double minBeech,
            double maxBeech,
            double minCost,
            double maxCost
            ) {
            Properties.Settings.Default.maxSpace = maxSpace;
            Properties.Settings.Default.minSpace = minSpace;
            Properties.Settings.Default.maxBaths = maxBathes;
            Properties.Settings.Default.minBaths = minBathes;
            Properties.Settings.Default.maxBeech = maxBeech;
            Properties.Settings.Default.minBeech = minBeech;
            Properties.Settings.Default.minCost = maxCost;
            Properties.Settings.Default.maxCost = minCost;
            Properties.Settings.Default.Save();
        }

        private double displacement(double value, double min, double max) {
            return (value - min) * (B - A) / (max - min) + A;
        }

        private void normalizeSpace(Flat flat, double min, double max) {
            flat.Space = displacement(flat.Space, min, max);
        }

        private void normalizeBathes(Flat flat, double min, double max) {
            flat.Baths = displacement(flat.Baths, min, max);
        }

        private void normalizeBeech(Flat flat, double min, double max) {
            flat.Beech = displacement(flat.Beech, min, max);
        }

        private void normalizeCost(Flat flat, double min, double max) {
            flat.Cost = displacement(flat.Cost, min, max);
        }
    }
}
