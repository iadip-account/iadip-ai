using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace IADIP {
    public class ExcelReader : IReader {
        string[] pathes;

        public ExcelReader(string[] pathes) {
            this.pathes = pathes;
        }

        const int NUMBER = 8;
        const int SPACE = 1;
        const int BATHES = 3;
        const int BEECH = 4;
        const int COST = 0;
        const int CITY = 0;
        const int FIRM = 0;

        public List<Flat> getAll() {
            IPreprocessing preprocessing = new PreprocessingFindSolded();
            IMissing missing = new MissingByAverage();
            List<Flat> flats = preprocessing.preprocess(arraysRawData());
            missing.fillMissingValues(flats);
            return flats;
        }

        private List<FlatList> arraysRawData() {
            List<FlatList> arrays = new List<FlatList>();
            foreach (string path in pathes) {
                Excel.Application objExcel = new Excel.Application();
                Excel.Workbook objBook = objExcel.Workbooks.Open(path);
                Excel.Worksheet objSheet = (Excel.Worksheet)objBook.Sheets[1];
                var lastRow = objSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                object[,] array = (object[,])objSheet.Range["A2:I" + lastRow.ToString()].Value;

                List<Flat> currFlats = new List<Flat>();

                for (int i = 1; i < array.GetLength(0); i++) {
                    Flat flat = new Flat(
                        convertToInt(array[i, NUMBER]),
                        convertToDouble(array[i, SPACE]),
                        convertToDouble(array[i, BATHES]),
                        convertToDouble(array[i, BEECH]),
                        convertToDouble(array[i, COST]),
                        convertToString(array[i, CITY]),
                        convertToString(array[i, FIRM])
                        );
                    currFlats.Add(flat);
                    
                }
                arrays.Add(new FlatList(currFlats));
            }
            return arrays;
        }

        private double convertToDouble(object value) { 
            double temp = -1;
            return (value != null) ? (Double.TryParse(value.ToString(), out temp)) ? temp : temp : temp;
        }

        private int convertToInt(object value) {
            int temp = -1;
            return (value != null) ? (Int32.TryParse(value.ToString(), out temp)) ? temp : temp : temp;
        }

        private string convertToString(object value) {
            return (value != null) ? value.ToString() : string.Empty;
        }
    }
}
