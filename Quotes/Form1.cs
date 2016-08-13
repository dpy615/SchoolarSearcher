﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Quotes {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        List<string> proxyList = new List<string>();
        private void button1_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            textBox1.Text = filename;
            textBox2.Text = filename.Substring(0, filename.LastIndexOf('.')) + "_res.csv";
        }

        private void button2_Click(object sender, EventArgs e) {
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.ShowDialog();
            textBox2.Text = saveFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e) {
        init:
            try {
                Searcher.proxyString = textBox5.Text;
                Searcher.InitProxy();
            } catch (Exception ex) {
                goto init;
            }
            new Thread(new ThreadStart(Do)).Start();
            button3.Enabled = false;
            new Thread(new ThreadStart(Time)).Start();



        }
        private void Do() {
            try {
                int.TryParse(textBox4.Text.ToString(), out Searcher.startCount);
                Searcher.proxy = Searcher.GetProxy();
                Searcher.DoSearchBaiDu(textBox1.Text, textBox2.Text);
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
                button3.Enabled = true;
            }

        }
        bool run = true;
        private void Time() {
            while (run) {
                try {
                    label1.Text = "完成计数：" + Searcher.count;
                    label4.Text = "当前代理IP：" + Searcher.proxy;
                    Thread.Sleep(1000);
                } catch (Exception) {
                }

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            run = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }

        private void button4_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            textBox3.Text = filename;
            Searcher.filename = filename;


        }
    }
}