namespace Quotes {
    partial class QuoteSearch {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuoteSearch));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.检索ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelCsvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.引文拆分ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searcherControl1 = new Quotes.SearcherControl();
            this.excel2Csv1 = new Quotes.Excel2Csv();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.检索ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.工具ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 检索ToolStripMenuItem
            // 
            this.检索ToolStripMenuItem.Name = "检索ToolStripMenuItem";
            this.检索ToolStripMenuItem.ShortcutKeyDisplayString = "J";
            this.检索ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.检索ToolStripMenuItem.Text = "检索";
            this.检索ToolStripMenuItem.Click += new System.EventHandler(this.SearchToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelCsvToolStripMenuItem,
            this.引文拆分ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // excelCsvToolStripMenuItem
            // 
            this.excelCsvToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.excelCsvToolStripMenuItem.Name = "excelCsvToolStripMenuItem";
            this.excelCsvToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.excelCsvToolStripMenuItem.Text = "Excel&Csv工具";
            this.excelCsvToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.excelCsvToolStripMenuItem.Click += new System.EventHandler(this.excelCsvToolStripMenuItem_Click);
            // 
            // 引文拆分ToolStripMenuItem
            // 
            this.引文拆分ToolStripMenuItem.Name = "引文拆分ToolStripMenuItem";
            this.引文拆分ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.引文拆分ToolStripMenuItem.Text = "引文拆分";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // searcherControl1
            // 
            this.searcherControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searcherControl1.Location = new System.Drawing.Point(0, 25);
            this.searcherControl1.Name = "searcherControl1";
            this.searcherControl1.Size = new System.Drawing.Size(984, 536);
            this.searcherControl1.TabIndex = 2;
            this.searcherControl1.Visible = false;
            // 
            // excel2Csv1
            // 
            this.excel2Csv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.excel2Csv1.Location = new System.Drawing.Point(0, 25);
            this.excel2Csv1.Name = "excel2Csv1";
            this.excel2Csv1.Size = new System.Drawing.Size(984, 536);
            this.excel2Csv1.TabIndex = 1;
            this.excel2Csv1.Visible = false;
            // 
            // QuoteSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.searcherControl1);
            this.Controls.Add(this.excel2Csv1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "QuoteSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuoteSearch";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelCsvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private Excel2Csv excel2Csv1;
        private System.Windows.Forms.ToolStripMenuItem 检索ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 引文拆分ToolStripMenuItem;
        private SearcherControl searcherControl1;
    }
}