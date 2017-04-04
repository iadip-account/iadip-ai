using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace IADIP {
    public class ExcelReader : IReader {
        string[] pathes;

        public ExcelReader(string[] pathes) {
            this.pathes = pathesSort(pathes);
        }

        const int NUMBER = 9;
        const int SPACE = 2;
        const int BATHES = 4;
        const int BEECH = 5;
        const int COST = 1;
        const int CITY = 6;
        const int FIRM = 8;

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

                object[,] array = (object[,])objSheet.Range["A1:I" + lastRow.ToString()].Value;

                List<Flat> currFlats = new List<Flat>();

                for (int i = 1; i < array.GetLength(0); i++) {

                    if (!isFill(array[i, SPACE])) continue;
                    if (isHat(array[i, SPACE])) continue;

                    Flat flat = new Flat(
                        convertToInt(array[i, NUMBER]),
                        convertToDouble(array[i, SPACE]),
                        convertToDouble(array[i, BATHES]),
                        convertToDouble(array[i, BEECH]),
                        convertToDouble(array[i, COST]),
                        convertToString(array[i, CITY]),
                        convertToString(array[i, FIRM])
                        );
                    flat.Baths = convertToDouble(array[i, BATHES]);
                    currFlats.Add(flat);
                    
                }
                arrays.Add(new FlatList(currFlats));
                objExcel.Quit();
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

        private bool isFill(object space) {
            return space != null;
        }

        private bool isHat(object space) {
            double temp = -1;
            return !Double.TryParse(space.ToString(), out temp);
        }

        private string[] pathesSort(string[] array) {
            bool flag = true;
            while (flag) {
                flag = false;
                for (int i = 0; i < array.Length - 1; ++i)
                    if (array[i].CompareTo(array[i + 1]) > 0) {
                        string buf = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = buf;
                        flag = true;
                    }
            }
            return array;
        }
    }
}
