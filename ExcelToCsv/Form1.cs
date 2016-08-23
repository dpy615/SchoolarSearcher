using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ExcelToCsv {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            textBox1.Text = filename;
            if (!string.IsNullOrEmpty(filename)) {
                if (filename.ToLower().EndsWith("csv")) {
                    textBox2.Text = filename.Substring(0, filename.LastIndexOf('.')) + "_New.xlsx";
                } else {
                    textBox2.Text = filename.Substring(0, filename.LastIndexOf('.')) + "_New.csv";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            new Thread(DoExcel).Start();
            button3.Enabled = false;
        }

        private void DoExcel() {
            try {
                ExcelChange(textBox1.Text, textBox2.Text);
                MessageBox.Show("完成");
            } catch (Exception e) {
                MessageBox.Show("失败！\r\n" + e.Message);
            }
            button3.Enabled = true;
        }

        public static void ExcelChange(string inputFileName, string outFileName) {
            StreamWriter sw = new StreamWriter(outFileName, false, Encoding.UTF8);
            try {
                using (var fs = new FileStream(inputFileName, FileMode.Open)) {
                    XSSFWorkbook workBook = new XSSFWorkbook(fs);
                    ISheet sheet1 = workBook.GetSheet("Sheet1");
                    int cellCount = sheet1.GetRow(0).LastCellNum;
                    for (int i = 0; i < sheet1.LastRowNum + 1; i++) {
                        IRow row = sheet1.GetRow(i);
                        string str = "";
                        for (int j = 0; j < cellCount; j++) {
                            ICell cell = row.GetCell(j);
                            str += "\"" + (cell == null ? "" : cell.ToString()) + "\"" + ",";
                        }
                        str = str.Substring(0, str.Length - 1);
                        sw.WriteLine(str);
                        sw.Flush();
                    }
                }
            } catch (Exception) {

                throw;
            } finally {
                sw.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            new Thread(DoCsv).Start();
            button4.Enabled = false;
        }

        private void DoCsv() {
            try {
                CsvChange(textBox1.Text, textBox2.Text, label1);
                MessageBox.Show("完成");
            } catch (Exception e) {
                MessageBox.Show("失败！\r\n" + e.Message);
            }
            button4.Enabled = true;
        }

        public static void CsvChange(string inputFileName, string outFileName, Label label1) {
            StreamReader sr = new StreamReader(inputFileName, Encoding.UTF8);
            try {
                SetLabel(label1, "新建实例");
                XSSFWorkbook workBook = new XSSFWorkbook();
                ISheet sheet1 = workBook.CreateSheet("Sheet1");
                string str = "";
                int i = 0;
                while (!string.IsNullOrEmpty((str = sr.ReadLine()))) {
                    string[] values = Regex.Split(str.Substring(1, str.Length - 2), "\",\"");
                    IRow row = sheet1.CreateRow(i++);
                    SetLabel(label1, "第 " + i + "行");
                    for (int j = 0; j < values.Length; j++) {
                        ICell cell = row.CreateCell(j);
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue(values[j]);
                    }
                }
                using (var fs = new FileStream(outFileName, FileMode.CreateNew)) {
                    workBook.Write(fs);
                    fs.Close();
                }
            } catch (Exception) {

                throw;
            } finally {
                sr.Close();
            }
        }

        public static void SetLabel(Label label1, string str) {
            label1.Text = "状态：" + str;
        }

        private void button5_Click(object sender, EventArgs e) {
            new Thread(Do8Type).Start();
            button5.Enabled = false;
        }

        private void Do8Type() {
            try {
                TypeCsvChange(textBox1.Text, textBox2.Text, label1);
                MessageBox.Show("完成");
            } catch (Exception e) {
                MessageBox.Show("失败！\r\n" + e.Message);
            }
            button5.Enabled = true;
        }
        public static void TypeCsvChange(string inputFileName, string outFileName, Label label1) {
            StreamReader sr = new StreamReader(inputFileName, Encoding.UTF8);
            StreamWriter sw_title_J = new StreamWriter(new FileStream(outFileName + "_title_J.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_title_M = new StreamWriter(new FileStream(outFileName + "_title_M.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_title_C = new StreamWriter(new FileStream(outFileName + "_title_C.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_title_Other = new StreamWriter(new FileStream(outFileName + "_title_Other.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_notitle_J = new StreamWriter(new FileStream(outFileName + "_notitle_J.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_notitle_M = new StreamWriter(new FileStream(outFileName + "_notitle_M.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_notitle_C = new StreamWriter(new FileStream(outFileName + "_notitle_C.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_notitle_Other = new StreamWriter(new FileStream(outFileName + "_notitle_Other.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_error = new StreamWriter(new FileStream(outFileName + "_error.csv", FileMode.CreateNew), Encoding.UTF8);
            try {
                SetLabel(label1, "开始读取");
                string str = sr.ReadLine();
                sw_title_J.WriteLine(str);
                sw_title_M.WriteLine(str);
                sw_title_C.WriteLine(str);
                sw_title_Other.WriteLine(str);
                sw_notitle_J.WriteLine(str);
                sw_notitle_M.WriteLine(str);
                sw_notitle_C.WriteLine(str);
                sw_notitle_Other.WriteLine(str);
                sw_error.WriteLine(str);
                int i = 0;
                while (!string.IsNullOrEmpty((str = sr.ReadLine()))) {
                    try {
                        SetLabel(label1, "第" + (i++) + "条");

                        string tmpstr = str;
                        string[] res = Regex.Split(tmpstr, "\",\"");
                        int length = res.Length;
                        while (Regex.Split(tmpstr, "\",\"").Length != 17 || !tmpstr.EndsWith("\"")) {
                            str = sr.ReadLine();
                            if (str == null) {
                                return;
                            }
                            tmpstr += str;
                        }

                        string[] values = Regex.Split(str.Substring(1, str.Length - 2), "\",\"");

                        string type = values[2];
                        string title = values[7];
                        if (!string.IsNullOrEmpty(title)) {
                            if (type.Trim().ToUpper() == "J") {
                                sw_title_J.WriteLine(str);
                            } else if (type.Trim().ToUpper() == "M") {
                                sw_title_M.WriteLine(str);
                            } else if (type.Trim().ToUpper() == "C") {
                                sw_title_C.WriteLine(str);
                            } else {
                                sw_title_Other.WriteLine(str);
                            }
                        } else {
                            if (type.Trim().ToUpper() == "J") {
                                sw_notitle_J.WriteLine(str);
                            } else if (type.Trim().ToUpper() == "M") {
                                sw_notitle_M.WriteLine(str);
                            } else if (type.Trim().ToUpper() == "C") {
                                sw_notitle_C.WriteLine(str);
                            } else {
                                sw_notitle_Other.WriteLine(str);
                            }
                        }
                    } catch (Exception) {
                        sw_error.WriteLine(str);
                    }

                }
                SetLabel(label1, "完成");
            } catch (Exception) {

                throw;
            } finally {
                sr.Close();
                sw_title_J.Close();
                sw_title_M.Close();
                sw_title_C.Close();
                sw_title_Other.Close();
                sw_notitle_J.Close();
                sw_notitle_M.Close();
                sw_notitle_C.Close();
                sw_notitle_Other.Close();
            }
        }

        private void textBox3_Click(object sender, EventArgs e) {
            int tmp = 0;
            bool b = int.TryParse(textBox3.Text, out tmp);
            if (!b) {
                textBox3.Text = "";
            }
        }
        private void button6_Click(object sender, System.EventArgs e) {
            new Thread(CutCsv).Start();
            button6.Enabled = false;
        }
        private void CutCsv() {
            try {
                CutCsv(textBox1.Text, textBox2.Text, label1, textBox3.Text);
                MessageBox.Show("完成");
            } catch (Exception e) {
                MessageBox.Show("失败！\r\n" + e.Message);
            }
            button6.Enabled = true;
        }
        public static void CutCsv(string inputFileName, string outFileName, Label label1, string count) {
            int num = int.Parse(count);
            StreamReader sr = new StreamReader(inputFileName, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(new FileStream(outFileName + "_0.csv", FileMode.CreateNew), Encoding.UTF8);
            StreamWriter sw_error = new StreamWriter(new FileStream(outFileName + "_error.csv", FileMode.CreateNew), Encoding.UTF8);
            try {
                SetLabel(label1, "开始读取");
                string head = sr.ReadLine();
                sw.WriteLine(head);
                string str = "";
                int i = 0;
                int fileCount = 0;
                while (!string.IsNullOrEmpty((str = sr.ReadLine()))) {
                    try {
                        SetLabel(label1, "第" + (fileCount * num + i++) + "条");
                        string tmpstr = str;
                        string[] res = Regex.Split(tmpstr, "\",\"");
                        int length = res.Length;
                        while (Regex.Split(tmpstr, "\",\"").Length != 17 || !tmpstr.EndsWith("\"")) {
                            str = sr.ReadLine();
                            if (str == null) {
                                return;
                            }
                            tmpstr += str;
                        }

                        sw.WriteLine(tmpstr);
                        sw.Flush();
                        if (i > num) {
                            sw.Close();
                            i = 0;
                            fileCount++;
                            sw = new StreamWriter(new FileStream(outFileName + "_" + fileCount + ".csv", FileMode.CreateNew), Encoding.UTF8);
                            sw.WriteLine(head);
                        }
                    } catch (Exception) {
                        sw_error.WriteLine(str);
                    }

                }
                SetLabel(label1, "完成");
            } catch (Exception) {

                throw;
            } finally {
                sr.Close();
                sw.Close();
                sw_error.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            textBox4.Text = "";
            for (int i = 0; i < openFileDialog1.FileNames.Length; i++) {
                textBox4.Text += openFileDialog1.FileNames[i] + ";";
            }
            if (openFileDialog1.FileName.Length > 0) {
                textBox5.Text = openFileDialog1.FileName.Substring(0, openFileDialog1.FileName.LastIndexOf('.')) + ".Combine.csv";
            }
            openFileDialog1.Multiselect = false;


        }

        private void button8_Click(object sender, EventArgs e) {
            new Thread(MergeCsv).Start();
            button8.Enabled = false;
        }
        private void MergeCsv() {
            try {
                MergeCsv(textBox4.Text, textBox5.Text, label1);
                MessageBox.Show("完成");
            } catch (Exception e) {
                MessageBox.Show("失败！\r\n" + e.Message);
            }
            button8.Enabled = true;
        }

        public static void MergeCsv(string inputFileName, string outFileName, Label label1) {
            StreamWriter sw = null;
            StreamWriter sw_error = null;

            try {
                if (File.Exists(outFileName)) {
                    File.Delete(outFileName);
                }
                if (File.Exists(outFileName + ".error")) {
                    File.Delete(outFileName + ".error");
                }
                sw = new StreamWriter(new FileStream(outFileName, FileMode.Create), Encoding.UTF8);
                sw_error = new StreamWriter(new FileStream(outFileName + ".error", FileMode.CreateNew), Encoding.UTF8);
                string[] fileList = inputFileName.Split(';');
                SetLabel(label1, "开始合并");
                for (int i = 0; i < fileList.Length; i++) {
                    if (string.IsNullOrEmpty(fileList[i])) {
                        continue;
                    }
                    StreamReader sr = new StreamReader(fileList[i], Encoding.UTF8);
                    string head = sr.ReadLine();
                    if (i == 0) {
                        sw.WriteLine(head);
                    }
                    string str = "";
                    int j = 1;
                    while (!string.IsNullOrEmpty((str = sr.ReadLine()))) {
                        try {
                            SetLabel(label1, string.Format("正在合并文件{0}中的第{1}条", fileList[i], j++));
                            string tmpstr = str;
                            sw.WriteLine(tmpstr);
                            sw.Flush();
                        } catch (Exception) {
                            sw_error.WriteLine(str);
                        }

                    }
                    SetLabel(label1, "完成");
                    sr.Close();
                }
            } catch (Exception) {

                throw;
            } finally {
                sw.Close();
                sw_error.Close();
            }
        }

    }
}
