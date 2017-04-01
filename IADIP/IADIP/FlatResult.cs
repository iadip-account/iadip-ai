using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    public class FlatResult {
        public Flat Flat { get; private set; }
        public double Result { get; private set; }

        public FlatResult(Flat Flat, double Result) {
            this.Flat = Flat;
            this.Result = Result;
        }
    }
}
