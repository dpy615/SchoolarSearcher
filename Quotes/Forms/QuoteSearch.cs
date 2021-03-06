﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Quotes {
    public partial class QuoteSearch : Form {
        public QuoteSearch() {
            InitializeComponent();
        }

        private void excelCsvToolStripMenuItem_Click(object sender, EventArgs e) {
            excel2Csv1.Visible = true;
            searcherControl1.Close();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e) {
            new AboutBox1().Show();
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e) {
            searcherControl1.Visible = true;
            excel2Csv1.Close();
        }

        private void QuoteSearch_FormClosing(object sender, FormClosingEventArgs e) {
            for (int i = 0; i < SearcherControl.searcherList.Count; i++) {
                SearcherControl.searcherList[i].Close() ;
            }

            for (int i = 0; i < SearcherControl.searcherBaiduList.Count; i++) {
                SearcherControl.searcherBaiduList[i].Close();
            }
            
        }

        private void tMP1ToolStripMenuItem_Click(object sender, EventArgs e) {

        }
    }
}
