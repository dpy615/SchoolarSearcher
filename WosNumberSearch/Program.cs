﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WosNumberSearch {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            WosSearcher.InitHttp();
            string regex = @"<div class=""search-results-data-cite"">被引频次: \d*<br>";
            string[] src = new string[]{"000287978001186"
                                        ,"000309406701108"
                                        ,"000221794800164"
                                        ,"000238969200006"
                                        ,"000295396600047"
                                        ,"000231784500004"
                                        ,"000284150002092"
                                        ,"000187460000003"
                                        ,"A1993BY79J00160"
                                        ,"A1994BA72M00240"
                                        ,"A1996BF68P00067"
                                        ,"A1997BJ13Y00073"
                                        ,"A1992BW47G00390"
                                        ,"000364968700002"
                                        ,"000233155100019"
                                        ,"000233155100020"
                                        ,"000184694400046"
                                        ,"000279038001059"
                                        ,"000235518700107"
                                        ,"000224459000084"
                                        ,"000300061900169"
                                        ,"000079028000009"
                                        ,"000255099301056"
                                        ,"000309166201049"
                                        ,"000250382805022"
                                        ,"000356196400001"
                                        ,"000287417502007"
                                        ,"000286163200039"
                                        ,"000309166201054"
                                        ,"000361555601098"
                                        ,"000331094300116"
                                        ,"000351830500343"
                                        ,"000361555602085"
                                        ,"000361555602081"
                                        ,"000361555602085"
                                        ,"000361555601098"
                                        ,"000259736802095"
                                        ,"000279038000130"
                                        ,"000295615801070"
                                        ,"000287417500002"
                                        ,"000346356200001"
                                        ,"000230923100119"
                                        ,"000295615801060"
                                        ,"000259736801086"
                                        ,"000186833000193"
                                        ,"000259736800162"
                                        ,"000084507900017"
                                        ,"000291771100011"
                                        ,"A1994BA93D00067"
                                        ,"000294955300205"
                                        ,"000280118000070"
                                        ,"A1997BH89T00020"
                                        ,"000309676200171"
                                        ,"000305621100018"
                                        ,"000245559901106"
                                        ,"000185328600014"
                                        ,"000173806500105"
                                        ,"000245559900029"
                                        ,"A1992BW69B00031"
                                        ,"000185328700167"
                                        ,"A1994BC14D00011"
                                        ,"000186055300025"
                                        ,"000184335100043"
                                        ,"000244475700011"
                                        ,"000235046903129"
                                        ,"000286944500025"
                                        ,"000182404900059"
                                        ,"000225886500036"
                                        ,"000172440900176"
                                        ,"000178714800563"
                                        ,"000372006900010"
                                        ,"000320450400003"
                                        ,"000251798600047"
                                        ,"000224456700041"
                                        ,"000245099900060"
                                        ,"000229281900002"
                                        ,"000258298800002"
                                        ,"000182770600341"
                                        ,"000236522100010"
                                        ,"000249117700042"
                                        ,"000275366201040"
                                        ,"000287416300235"
                                        ,"000288888800073"
                                        ,"000223848100023"
                                        ,"000336893605056"
                                        ,"000259418001073"
                                        ,"000257505701004"
                                        ,"000259418000064"};
            string[] res = new string[src.Length];
            for (int i = 0; i < src.Length; i++) {
                string tmp = WosSearcher.Search(src[i], false)[0];
                string value = Regex.Match(tmp, regex).ToString();
                value = value.Substring(value.IndexOf(":")+1, value.IndexOf("<") - value.IndexOf(":")).Trim();
                res[i] = src[i] + "\t" + value + "\t" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                Console.WriteLine(i);
            }
            File.WriteAllLines("res.txt", res);
            Console.Read();
        }
    }
}