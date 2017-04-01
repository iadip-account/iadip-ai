using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IADIP {
    interface IPreprocessing {
        List<Flat> preprocess(List<FlatList> collection);
    }
}
