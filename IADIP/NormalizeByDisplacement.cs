using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class NormalizeByDisplacement : INormalize {
        public List<Flat> normalize(List<Flat> flats) {
            double maxSpace = flats.Max(q => q.Space);
            double maxBathes = flats.Max(q => q.Baths);
            double maxBeech = flats.Max(q => q.Beech);
            double maxCost = flats.Max(q => q.Cost);

            save (maxSpace, maxBathes, maxBeech, maxCost);

            List<Flat> flatsNormalize = new List<Flat>();
            foreach (Flat flat in flats) {
                Flat flatNormalize = new Flat(flat.Number,
                normalizeSpace(flat, maxSpace),
                normalizeBathes(flat, maxBathes),
                normalizeBeech(flat, maxBeech),
                normalizeCost(flat, maxCost),
                flat.City,
                flat.Firm);
                flatsNormalize.Add(flatNormalize);
            }
            return flatsNormalize;
        }

        private void save
            (
            double maxSpace,
            double maxBathes,
            double maxBeech,
            double maxCost
            ) {
            Properties.Settings.Default.maxSpace = maxSpace;
            Properties.Settings.Default.maxBaths = maxBathes;
            Properties.Settings.Default.maxBeech = maxBeech;
            Properties.Settings.Default.maxCost = maxCost;
            Properties.Settings.Default.Save();
        }

        public static double displacement(double value, double max) {
            return value / max;
        }

        public static double deDisplacement(double value, double max) {
            return value * max;
        }

        private double normalizeSpace(Flat flat, double max) {
            return displacement(flat.Space, max);
        }

        private double normalizeBathes(Flat flat, double max) {
            return displacement(flat.Baths, max);
        }

        private double normalizeBeech(Flat flat, double max) {
            return displacement(flat.Beech, max);
        }

        private double normalizeCost(Flat flat, double max) {
            return displacement(flat.Cost, max);
        }
    }
}
