using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MyTestProject {
    class Program {
        static void Main(string[] args) {

            string regex = @"[^\w]+";
            string input = "呵呵呵呵12，。——————";
            string str = Regex.Replace(ToDBC(input), regex," ");
            Console.WriteLine(str);
        }

        public static string ToDBC(string input) {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++) {
                if (c[i] == 12288) {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
    }
}
