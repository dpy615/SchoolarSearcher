using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Quotes {
    public partial class Excel2Csv : UserControl {
        public Excel2Csv() {
            InitializeComponent();
        }

        public void Close() {
            this.Visible = false;
        }
    }
}
