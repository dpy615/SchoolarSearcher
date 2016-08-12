using System;
using System.Collections.Generic;
using System.Text;

namespace MyTestProject {
    class Program {
        static void Main(string[] args) {
            string str = "哈哈哈，这是几个字？";
            var chars = str.ToCharArray();
            Console.WriteLine(chars.Length);
            Console.Read();
        }
    }
}
