using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MyTestProject {
    class Program {
        static void Main(string[] args) {
            string http = "Hello";
            string[] strs = new string[] {"234" };
            //strs.GetLowerBound
            string str = http.ToLowerInvariant();

            Console.WriteLine(str);
        }
    }
}
