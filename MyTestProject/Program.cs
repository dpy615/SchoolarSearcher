using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MyTestProject {
    class Program {
        static void Main(string[] args) {
            string http = "http%253A%252F%252Fwww.cnki.com.cn%252FArticle%252FCJFDTotal-JSJC199705009.htm";

            string str = Uri.UnescapeDataString(http);

            Console.WriteLine(str);
        }
    }
}
