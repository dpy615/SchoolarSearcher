using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Quotes.UserControls {
    public partial class SearchWosControl : UserControl {
        public SearchWosControl() {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e) {
            if (SearcherControl.searcherList.Contains(this)) {
                int index = SearcherControl.searcherList.IndexOf(this);
                for (int i = index; i < SearcherControl.searcherList.Count; i++) {
                    SearcherControl.searcherList[i].Location = new Point(SearcherControl.searcherList[i].Location.X, SearcherControl.searcherList[i].Location.Y - 36);
                }
                SearcherControl.searcherList.Remove(this);
            }
            this.Dispose();
        }
    }
}
