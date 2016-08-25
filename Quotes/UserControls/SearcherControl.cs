using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quotes.UserControls;

namespace Quotes {
    public partial class SearcherControl : UserControl {
        public static List<SearchWosControl> searcherList = new List<SearchWosControl>();
        public SearcherControl() {
            InitializeComponent();
            searcherList.Add(searchWosControl1);
        }

        public void Close() {
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e) {
            if (searcherList.Count > 11) {
                MessageBox.Show("最多只允许开启12个任务");
                return;
            }
            SearchWosControl swc = new SearchWosControl();
            Point fre = new Point(38, 39);
            if (searcherList.Count > 0) {
                fre = searcherList[searcherList.Count - 1].Location;
                fre = new Point(fre.X, fre.Y + 36);
            }
            //swc.SetBounds(fre.X, fre.Y + swc.Height, 868, 38);
            this.panel1.Controls.Add(swc);
            swc.Location = fre;
            swc.Visible = true;
            this.Refresh();
            searcherList.Add(swc);
        }
    }
}
