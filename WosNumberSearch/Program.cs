using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WosNumberSearch {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {



            string filename = args[0];
            string startIndex = args[1];
            string length = args[2];

            int start = int.Parse(startIndex);
            int len = int.Parse(length);
            WosSearcher.InitHttp();
            Console.WriteLine("读取文件中...");
            string[] src = File.ReadAllLines(filename);
            Console.WriteLine("读取文件完毕，共 " + src.Length + " 条，开始检索 " + startIndex + " 到 " + (start + len) + " 条...");
            int step = 50;
            if (start + len > src.Length) {
                len = src.Length - start;
            }
            string[] res = new string[len];
            DateTime dt1 = DateTime.Now;
            try {
                for (int i = start; i < start + len; i += step) {
                    Console.WriteLine("第 " + i + " 条\t" + DateTime.Now.ToString());
                    string srcnum = "";
                    int count = 0;
                    if (i + step < src.Length) {
                        count = step;
                    } else {
                        count = src.Length - i;
                    }
                    for (int j = 0; j < count - 1; j++) {
                        srcnum += src[i + j] + " or ";
                    }
                    srcnum += src[i + count - 1];
                    //Console.WriteLine(DateTime.Now.ToString());
                    try {
                        var dic = WosSearcher.Search(srcnum, count);
                        for (int j = 0; j < count; j++) {
                            try {
                                res[i - start + j] = string.Format("{0}\t{1}\t{2}", src[i + j], dic[src[i + j]], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            } catch (Exception) {
                                res[i - start + j] = string.Format("{0}\t{1}\t{2}", src[i + j], "Error", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    } catch (Exception) {
                        WosSearcher.InitHttp();
                        i -= step;
                        continue;
                    }

                }
                DateTime dt2 = DateTime.Now;
                File.WriteAllLines("res_" + startIndex + "_" + length, res);
                Console.WriteLine("over!\r\ncost:" + (dt2 - dt1).TotalSeconds);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                File.WriteAllLines("res_" + startIndex + "_" + length + ".txt", res);
            }
            Console.Read();
        }
    }
}
