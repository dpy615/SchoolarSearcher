using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Quotes.UserControls {
    public partial class SearchBaiDuControl : UserControl {
        public SearchBaiDuControl() {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        bool run = false;
        Searcher searcher = new Searcher();
        string saveFileName;
        private void button3_Click(object sender, EventArgs e) {
            try {
                run = false;
                searcher.run = false;
            } catch (Exception) {
            }
            if (SearcherControl.searcherBaiduList.Contains(this)) {
                int index = SearcherControl.searcherBaiduList.IndexOf(this);
                for (int i = index; i < SearcherControl.searcherBaiduList.Count; i++) {
                    SearcherControl.searcherBaiduList[i].Location = new Point(SearcherControl.searcherBaiduList[i].Location.X, SearcherControl.searcherBaiduList[i].Location.Y - 36);
                }
                SearcherControl.searcherBaiduList.Remove(this);
            }
            SearchBaiduControl_ControlRemoved(null, null);
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e) {
            openFileDialog1.DefaultExt = ".csv";
            openFileDialog1.FileName = "*.csv";
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            saveFileName = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf(".")) + ".res.csv";
        }

        private void button2_Click(object sender, EventArgs e) {
            Searcher.proxyString = Config.proxyString;
            searcher.isCn = checkBox1.Checked;
            new Thread(new ThreadStart(Do)).Start();
            button2.Enabled = false;
            run = true;
            new Thread(new ThreadStart(Time)).Start();

        }

        private void Do() {
            try {
                //int.TryParse(textBox4.Text.ToString(), out search.startCount);
                Searcher.proxy = Searcher.GetProxy();
                searcher.DoSearchBaiDu(textBox1.Text, saveFileName);
                MessageBox.Show("完成");
            } catch (Exception e) {
                if (e.Message != "error")
                    MessageBox.Show(e.ToString());
            }

            button3.Enabled = true;

        }

        private void Time() {
            while (run) {
                try {
                    label1.Text = "完成计数：" + searcher.count;
                    label2.Text = "代理IP：" + Searcher.proxy;
                    Thread.Sleep(1000);
                } catch (Exception) {
                }

            }
        }

        private void button4_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(saveFileName));
        }

        private void SearchBaiduControl_ControlRemoved(object sender, ControlEventArgs e) {
            run = false;
            if (searcher != null) {
                searcher.run = false;
            }
        }

        internal void Close() {
            run = false;
            if (searcher != null) {
                searcher.run = false;
            }
        }
    }
}
