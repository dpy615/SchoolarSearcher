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
    public partial class SearchWosControl : UserControl {
        public SearchWosControl() {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        bool run = false;
        Thread thread;
        Searcher searcher;
        string saveFileName;
        private void button3_Click(object sender, EventArgs e) {
            try {
                run = false;
                searcher.run = false;
            } catch (Exception) {
            }
            if (SearcherControl.searcherList.Contains(this)) {
                int index = SearcherControl.searcherList.IndexOf(this);
                for (int i = index; i < SearcherControl.searcherList.Count; i++) {
                    SearcherControl.searcherList[i].Location = new Point(SearcherControl.searcherList[i].Location.X, SearcherControl.searcherList[i].Location.Y - 36);
                }
                SearcherControl.searcherList.Remove(this);
            }
            SearchWosControl_ControlRemoved(null, null);
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e) {
            openFileDialog1.DefaultExt = ".csv";
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e) {
            thread = new Thread(new ThreadStart(DoWOS));
            thread.Start();
            button2.Enabled = false;
            run = true;
            new Thread(new ThreadStart(Time)).Start();
        }

        private void DoWOS() {
            try {
                saveFileName = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf(".")) + ".res.csv";
                searcher = new Searcher();
                searcher.DoSearchWOS(textBox1.Text, saveFileName);
                MessageBox.Show("完成");
            } catch (ThreadAbortException) {
            } catch (Exception e) {
                if (e.Message != "error") MessageBox.Show(e.ToString());
                button2.Enabled = true;
            }

        }

        private void Time() {
            while (run) {
                try {
                    label1.Text = "完成计数：" + searcher.count;
                    //label2.Text = "代理IP：" + Searcher.proxy;
                    Thread.Sleep(1000);
                } catch (Exception) {
                }

            }
        }

        private void button4_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(saveFileName));
        }

        private void SearchWosControl_ControlRemoved(object sender, ControlEventArgs e) {
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
