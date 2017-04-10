using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class NormalizeByDisplacement : INormalize {
        const int A = 0;
        const int B = 1;

        public List<Flat> normalize(List<Flat> flats) {
            double minSpace = flats.Min(q => q.Space);
            double maxSpace = flats.Max(q => q.Space);
            double minBathes = flats.Min(q => q.Baths);
            double maxBathes = flats.Max(q => q.Baths);
            double minBeech = flats.Min(q => q.Beech);
            double maxBeech = flats.Max(q => q.Beech);
            double minCost = flats.Min(q => q.Cost);
            double maxCost = flats.Max(q => q.Cost);

            save (minSpace, maxSpace, minBathes, maxBathes, minBeech, maxBeech, minCost, maxCost);

            List<Flat> flatsNormalize = new List<Flat>();
            foreach (Flat flat in flats) {
                Flat flatNormalize = new Flat(flat.Number,
                normalizeSpace(flat, minSpace, maxSpace),
                normalizeBathes(flat, minBathes, maxBathes),
                normalizeBeech(flat, minBeech, maxBeech),
                normalizeCost(flat, minCost, maxCost),
                flat.City,
                flat.Firm);
                flatsNormalize.Add(flatNormalize);
            }
            return flatsNormalize;
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
            Properties.Settings.Default.minCost = minCost;
            Properties.Settings.Default.maxCost = maxCost;
            Properties.Settings.Default.Save();
        }

        public static double displacement(double value, double min, double max) {
            return (value - min) * (B - A) / (max - min) + A;
        }

        public static double deDisplacement(double value, double min, double max) {
            return (value - A) * (max - min) / (B - A) + min;
        }

        private double normalizeSpace(Flat flat, double min, double max) {
            return displacement(flat.Space, min, max);
        }

        private double normalizeBathes(Flat flat, double min, double max) {
            return displacement(flat.Baths, min, max);
        }

        private double normalizeBeech(Flat flat, double min, double max) {
            return displacement(flat.Beech, min, max);
        }

        private double normalizeCost(Flat flat, double min, double max) {
            return displacement(flat.Cost, min, max);
        }
    }
}
