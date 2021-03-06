﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Security;
using System.Threading;

namespace Quotes {
    public class Searcher {
        public int count;
        public int startCount;
        public static string uri = "";
        public static string quote_uri = "https://xueshu.glgoo.com/scholar?q=info:***:scholar.google.com/&output=cite&scirp=0&hl=zh-CN";

        static string regex_quote = "<div id=\"gs_cit0\" tabindex=\"0\" class=\"gs_citr\">.*?</div></td></tr>";
        static string regex_title = "<a onclick=\"return gs_ocit\\(event,'.*','0'\\)\" href=\"#\" class=\"gs_nph\" role=\"button\" aria-controls=\"gs_cit\" aria-haspopup=\"true\">引用</a>";

        static string uri_baidu = "http://xueshu.baidu.com/s?wd=***&rsv_bp=0&tn=SE_baiduxueshu_c1gjeupa&rsv_spt=3&ie=utf-8&f=8&rsv_sug2=1&sc_f_para=sc_tasktype%3D%7BfirstSimpleSearch%7D&rsv_n=2";
        //static string uri_baidu_download = "http://xueshu.baidu.com/u/citation?&url=***&sign=###&diversion=6311015199499354113&t=net";
        static string regex_baidu_sign = "data-sign=\\\".{32}\\\" data-click";
        static string regex_baidu_http = "data-link=\"http%3A%2F%2F.*?\"";

        static string uri_baidu_download = "http://xueshu.baidu.com/u/citation?&callback=noname&sign=###&diversion=6311015199499354113&url=***&t=cite&_=0";

        public static int errorCount = 0;
        //static string regex_baidu_mult_sign = @"<a href=\"".*?"" data-click=""{'button_tp':'title'}"" target=""_blank"">";
        static string regex_baidu_mult_title = @"<div id=""\d*"" class=""result sc_default_result xpath-log"" srcid=""\d*"" tpl=""se_st_sc_default"" mu="".*?""><div class=""sc_content""><h3 class=""t c_font"">.*?<a href="".*?"" data-click=""{'button_tp':'title'}"" target=""_blank"">.*?</a></h3>";
        //static string regex_baidu_mult_all = @"<div id=""\d*"" class=""result sc_default_result xpath-log"" srcid=""\d*"" tpl=""se_st_sc_default"" mu="".*?""><div class=""sc_content""><h3 class=""t c_font"">.*?<a href="".*?"" data-click=""{'button_tp':'title'}"" target=""_blank"">.*?</a></h3>.*?diversion=""6311015199499354113""></i></div>";
        static string regex_baidu_mult_all = @"<div id=""\d*"" class=""result sc_default_result xpath-log"" srcid=""\d*"" tpl=""se_st_sc_default"" mu="".*?""><div class=""sc_content""><h3 class=""t c_font"">.*?<a href="".*?"" data-click=""{'button_tp':'title'}"" target=""_blank"">.*?</a></h3><div class=""sc_info"">.*?<i class=""iconfont icon-cite"">&#xe600;</i><span>引用";

        public static List<string> proxyList = new List<string>();
        public static int proxyIndex = 0;

        public static string proxyString = "http://dev.kuaidaili.com/api/getproxy/?orderid=927014605389071&num=100&b_pcchrome=1&b_pcie=1&b_pcff=1&protocol=1&method=2&an_tr=1&an_an=1&an_ha=1&sp1=1&quality=1&sort=1&dedup=1&sep=4";

        public static string filename = "";
        public static void InitProxy() {
            if (!string.IsNullOrEmpty(filename)) {
                proxyList = File.ReadAllLines(filename).ToList();
            } else {
                if (string.IsNullOrEmpty(proxyString)) {
                    return;
                }
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string str = client.DownloadString(proxyString);
                //proxyList = str.Replace("\r", "|").Replace("\n", "").Split('|').ToList();
                if (str.StartsWith("ERROR")) {
                    Thread.Sleep(5000);
                }
                proxyList = str.Split('|').ToList();
            }

        }
        public static string proxy = "";
        public static string GetProxy() {
            if (proxyIndex >= proxyList.Count - 2) {
                proxyIndex = 0;
                InitProxy();
            }
            return proxyList.Count > 0 ? proxyList[proxyIndex++] : "";
        }



        #region 谷歌搜索
        /// <summary>
        /// 校内： uri = "https://xs.glgoo.com/scholar?hl=zh-CN&q=***&btnG=&lr=";
        /// uri = "https://xue.glgoo.com/scholar?q=***G=&hl=zh-CN&as_sdt=0%2C5"
        /// uri = "https://xueshu.glgoo.com/scholar?q=***&btnG=&hl=zh-CN&as_sdt=0%2C5";
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="saveName"></param>
        public void DoSearch(string fileName, string saveName) {
            uri = "https://xue.glgoo.com/scholar?q=***G=&hl=zh-CN&as_sdt=0%2C5";
            StreamReader sr = new StreamReader(fileName);
            StreamWriter sw = new StreamWriter(saveName, false);
            string str = "";
            str = sr.ReadLine();
            str = str + ",\"quote\",\"auther0\",\"auther1\",\"auther2\",\"title\",\"type\",\"jour\",\"year\",\"volume\",\"mag\",\"page\"";
            sw.WriteLine(str);
            sw.Flush();
            while (!string.IsNullOrEmpty(str = sr.ReadLine())) {
                string[] titles = Regex.Split(str, "\",\"");
                string title = titles[7];
                if (!string.IsNullOrEmpty(title)) {
                    string toWrite = str + "," + GetRes(title);
                    sw.WriteLine(toWrite);
                    sw.Flush();
                }
                count++;
            }
            sr.Close();
            sw.Close();
        }

        public static string GetRes(string title) {

            string sReturn = "";
            try {

                string u = uri.Replace("***", title.Replace(" ", "+"));
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
            //request.ProtocolVersion = HttpVersion.Version10;
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //string data = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //HttpWebRequest request = WebRequest.Create(u) as HttpWebRequest;
            //request.ProtocolVersion = HttpVersion.Version10;
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //HttpWebResponse response = request.GetResponse() as HttpWebRespob7uyhg6n  Q`  Q87/55nse;
            start:
                proxy = GetProxy();
                string data = Download(u, proxy);
                if (data.Length < 100) {
                    goto start;
                }
                string uid = Regex.Match(data, regex_title).ToString();
                uid = uid.Substring(uid.IndexOf('\'') + 1);
                uid = uid.Substring(0, uid.IndexOf('\''));
            start2:
                data = Download(quote_uri.Replace("***", uid), proxy);
                if (data.Length < 100) {
                    goto start2;
                }
                data = Regex.Match(data, regex_quote).ToString();
                string quote = data.Substring(data.IndexOf('>') + 1);
                quote = quote.Substring(0, quote.IndexOf('<'));
                //sReturn = Deal(quote);
            } catch (Exception) {
                return ",\"Error\"";
            }
            return sReturn;
        }
        #endregion

        /// <summary>
        /// Serpanos D N, Papalambrou A. Security and privacy in distributed smart came.ras[J]. Proceedings of the IEEE, 2008, 96(10): 1678-1687.
        /// Gehrmann C.                  ONVIF Security Recommendations[J].                     Onvif Document, 2010.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        private static string Deal_J(string quote, string[] titles, List<string> list,bool isCn) {
            string auther0 = "";
            string auther1 = "";
            string auther2 = "";
            string title = "";
            string type = "";
            string jour = "";
            string year = "";
            string volume = "";
            string mag = "";
            string page = "";
            string publisher = "";
            string place = "";


            string authers = "";
            if (quote.Contains(".")) {
                authers = quote.Substring(0, quote.IndexOf('.'));
            } else {
                return "无引用";
            }


            string[] auther = authers.Split(',');
            auther0 = auther[0];
            if (auther.Length > 1) auther1 = auther[1];
            if (auther.Length > 2) auther2 = auther[2];

            title = quote.Substring(quote.IndexOf(".") + 1, quote.IndexOf("[") - quote.IndexOf(".") - 1).Trim();
            type = quote.Substring(quote.IndexOf("[") + 1, quote.IndexOf("]") - quote.IndexOf("[") - 1).Trim();

            //quote = quote.Replace("//", ".").Replace("\\/\\/", ".");
            string strn = quote.Substring(quote.IndexOf(".") + 1);
            while (strn.Contains("[")) {
                strn = strn.Substring(strn.IndexOf(".") + 1);
            }
            string[] jours = strn.Split(',');
            if (strn.Contains(':')) {
                string[] tmppage = strn.Split(':');
                page = tmppage[tmppage.Length - 1].Trim();
                if (page.EndsWith(".")) {
                    page = page.Substring(0, page.Length - 1);
                }
            }
            if (strn.Contains('(')) {
                mag = strn.Substring(strn.LastIndexOf("(") + 1, strn.LastIndexOf(")") - strn.LastIndexOf("(") - 1);
            }
            if (jours.Length == 1) {
                if (jours[0].Contains(":")) {
                    year = jours[0].Split(':')[0];
                    //page = jours[0].Split(':')[1];
                } else {
                    jour = jours[0].Trim();
                }
                int tmpjour = 0;
                bool b = int.TryParse(jour.Replace(".", ""), out tmpjour);
                if (b) {
                    year = tmpjour.ToString();
                    jour = "";
                }
            } else {
                jour = jours[0].Trim();
                year = jours[1].Trim();
                if (year.Contains('(')) {
                    year = year.Substring(0, year.IndexOf('('));
                } else if (year.Contains(':')) {
                    year = year.Substring(0, year.IndexOf(':'));
                }
                if (jours.Length > 2) {
                    if (jours[2].Contains(":")) {
                        if (jours[2].Contains("(")) {
                            volume = jours[2].Substring(0, jours[2].IndexOf("("));
                        } else {
                            volume = jours[2].Substring(0, jours[2].IndexOf(":")).Trim();
                        }
                        // page = jours[2].Substring(jours[2].IndexOf(":") + 1).Trim().Replace(".", "");
                    } else {
                        if (jours[2].Contains("(")) {
                            volume = jours[2].Substring(0, jours[2].IndexOf("("));
                            //mag = jours[2].Substring(jours[2].IndexOf("(") + 1, jours[2].IndexOf(")") - jours[2].IndexOf("(") - 1);
                        } else {
                            volume = jours[2];
                        }
                    }
                }
            }
            TakeList(titles, list, auther0, title, type, jour, year, volume, mag, page, isCn);

            return ToResoultStr(quote, auther0, auther1, auther2, title, type, jour, year, volume, mag, page, publisher, place);

        }

        private static void TakeList(string[] titles, List<string> list, string auther0, string title, string type, string jour, string year, string volume, string mag, string page,bool isCn) {
            list.Add(MatchValue(UnicodeToString(title), titles[7], isCn).ToString());
            list.Add(UnicodeToString(type).Equals(titles[2]) ? "1" : "0");
            list.Add(UnicodeToString(auther0.ToLower()).Equals(titles[4].ToLower()) ? "1" : "0");
            list.Add(UnicodeToString(jour).ToLower().Equals(titles[8].ToLower()) ? "1" : "0");
            list.Add(UnicodeToString(year).Equals(titles[9]) ? "1" : "0");
            int tmp = 0;
            bool b = int.TryParse(volume, out tmp);
            if (b) {
                volume = tmp.ToString();
            }
            list.Add(UnicodeToString(volume).Equals(titles[10]) ? "1" : "0");

            tmp = 0;
            b = int.TryParse(mag, out tmp);
            if (b) {
                mag = tmp.ToString();
            }
            list.Add(UnicodeToString(mag).Equals(titles[11]) ? "1" : "0");
            list.Add(UnicodeToString(page).Equals(titles[12]) ? "1" : "0");
        }


        /// <summary>
        /// Lamarca A, Chawathe Y, Consolvo S, et al. Place Lab: Device Positioning Using Radio Beacons in the Wild[M]// Pervasive Computing. Springer Berlin Heidelberg, 2005:116--133.
        ///Gustin M S, Lindberg S E. Terrestial Hg Fluxes: Is the Next Exchange Up, Down, or Neither?[M]. Springer US, 2005.
        ///              ONVIF Security Recommendations[J].                     Onvif Document, 2010.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        private static string Deal_M(string quote, string[] titles, List<string> list,bool isCn) {
            string auther0 = "";
            string auther1 = "";
            string auther2 = "";
            string title = "";
            string type = "";
            string jour = "";
            string year = "";
            string volume = "";
            string mag = "";
            string page = "";
            string publisher = "";
            string place = "";


            string authers = "";
            if (quote.Contains(".")) {
                authers = quote.Substring(0, quote.IndexOf('.'));
            } else {
                return "无引用";
            }


            string[] auther = authers.Split(',');
            auther0 = auther[0];
            if (auther.Length > 1) auther1 = auther[1];
            if (auther.Length > 2) auther2 = auther[2];

            title = quote.Substring(quote.IndexOf(".") + 1, quote.IndexOf("[") - quote.IndexOf(".") - 1).Trim();
            type = quote.Substring(quote.IndexOf("[") + 1, quote.IndexOf("]") - quote.IndexOf("[") - 1).Trim();

            quote = quote.Replace("//", ".").Replace("\\/\\/", ".");
            string strn = quote.Substring(quote.IndexOf(".") + 1);
            while (strn.Contains("[")) {
                strn = strn.Substring(strn.IndexOf(".") + 1);
            }
            if (strn.Contains(':')) {
                string[] tmppage = strn.Split(':');
                page = tmppage[tmppage.Length - 1].Trim();
                if (page.EndsWith(".")) {
                    page = page.Substring(0, page.Length - 1);
                }
            }
            string[] jours = strn.Split(',');
            if (jours.Length == 1) {

            } else {
                string tmp = jours[0].Trim();

                if (tmp.Contains(".")) {
                    jour = tmp.Split('.')[0];
                    publisher = tmp.Split('.')[1];
                }
                year = jours[1].Trim();
                if (year.Contains('(')) {
                    year = year.Substring(0, year.IndexOf('('));
                } else if (year.Contains(':')) {
                    year = year.Substring(0, year.IndexOf(':'));
                }

            }
            TakeList(titles, list, auther0, title, type, jour, year, volume, mag, page, isCn);
            return ToResoultStr(quote, auther0, auther1, auther2, title, type, jour, year, volume, mag, page, publisher, place);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        private static string Deal_C(string quote, string[] titles, List<string> list,bool isCn) {
            string auther0 = "";
            string auther1 = "";
            string auther2 = "";
            string title = "";
            string type = "";
            string jour = "";
            string year = "";
            string volume = "";
            string mag = "";
            string page = "";
            string publisher = "";
            string place = "";


            string authers = "";
            if (quote.Contains(".")) {
                authers = quote.Substring(0, quote.IndexOf('.'));
            } else {
                return "无引用";
            }

            string[] auther = authers.Split(',');
            auther0 = auther[0];
            if (auther.Length > 1) auther1 = auther[1];
            if (auther.Length > 2) auther2 = auther[2];

            title = quote.Substring(quote.IndexOf(".") + 1, quote.IndexOf("[") - quote.IndexOf(".") - 1).Trim();
            type = quote.Substring(quote.IndexOf("[") + 1, quote.IndexOf("]") - quote.IndexOf("[") - 1).Trim();

            quote = quote.Replace("//", ".").Replace("\\/\\/", ".");
            string strn = quote.Substring(quote.IndexOf(".") + 1);
            while (strn.Contains("[")) {
                strn = strn.Substring(strn.IndexOf(".") + 1);
            }
            if (strn.Contains(':')) {
                string[] tmppage = strn.Split(':');
                page = tmppage[tmppage.Length - 1].Trim();
                if (page.EndsWith(".")) {
                    page = page.Substring(0, page.Length - 1);
                }
            }
            string[] jours = strn.Split(',');
            if (jours.Length == 1) {

            } else {
                string tmp = jours[0].Trim();
                if (tmp.Contains(".")) {
                    jour = tmp.Split('.')[0];
                    publisher = tmp.Split('.')[1];
                }
                year = jours[jours.Length - 2].Trim();
                if (year.Contains('(')) {
                    year = year.Substring(0, year.IndexOf('('));
                } else if (year.Contains(':')) {
                    year = year.Substring(0, year.IndexOf(':'));
                }
            }

            TakeList(titles, list, auther0, title, type, jour, year, volume, mag, page, isCn);
            return ToResoultStr(quote, auther0, auther1, auther2, title, type, jour, year, volume, mag, page, publisher, place);

        }

        private static string ToResoultStr(string quote, string auther0, string auther1, string auther2, string title, string type, string jour, string year, string volume, string mag, string page, string publisher, string place) {
            return UnicodeToString("\"" + quote + "\",\"" + type + "\",\"" + auther0 + "\",\"" + auther1 + "\",\"" + auther2 + "\",\"" + title + "\",\"" + jour + "\",\"" + year + "\",\"" + volume + "\",\"" + mag + "\",\"" + page + "\",\"" + "" + "\",\"" + place + "\",\"" + publisher + "\"");
        }
        public static string Download(string uri, string proxy) {
            HttpHelper http = new HttpHelper();

            HttpItem item = new HttpItem() {
                URL = uri,//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写   
                Cookie = "",//字符串Cookie     可选项   
                Referer = "",//来源URL     可选项   
                Postdata = "",//Post数据     可选项GET时不需要写   
                Timeout = 100000,//连接超时时间     可选项默认为100000    
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000   
                //UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值   
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko",//用户的浏览器类型，版本，操作系统     可选项有默认值   
                ContentType = "text/html",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
                //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数   
                //Connectionlimit = 1024,//最大连接数     可选项 默认为1024    
                ProxyIp = string.IsNullOrEmpty(proxy.Trim()) ? "" : proxy,//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数 
                //ProxyPwd = "123456",//代理服务器密码     可选项    
                //ProxyUserName = "administrator",//代理服务器账户名     可选项   
                ResultType = ResultType.String
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            return html;
        }


        #region 百度学术

        public void DoSearchBaiDu(string fileName, string saveName) {
            StreamReader sr = new StreamReader(fileName);
            bool isAppend = false;
            if (startCount > 0) {
                isAppend = true;
            }
            StreamWriter sw = new StreamWriter(saveName, isAppend, Encoding.UTF8);
            StreamWriter sw_error = new StreamWriter(saveName + "error.csv", isAppend, Encoding.UTF8);
            string str = "";
            str = sr.ReadLine();
            if (startCount == 0) {
                str = str + ",\"quote\",\"type\",\"auther1\",\"auther2\",\"auther3\",\"title\",\"source_title\",\"year\",\"volume\",\"issue\",\"page\",\"editor_in_chief\",\"place\",\"publisher\",\"matchTitle\",\"matchValue\",\"出版商链接\",\"标题匹配\",\"类型\",\"第一作者\",\"来源\",\"年\",\"卷\",\"期\",\"页\"";
            }
            sw_error.WriteLine(str);
            sw.WriteLine(str);
            while (run&&!string.IsNullOrEmpty(str = sr.ReadLine())) {
                if (count >= startCount) {
                    string[] titles = Regex.Split(str, "\",\"");
                    errorCount = 0;
                    if (!string.IsNullOrEmpty(titles[7])) {
                    start:
                        try {
                            string[] v = GetUriAndSign(titles, proxy, isCn);
                            //string[] v = GetUriAndSign(title, "");
                            string toWrite = str + ",";
                            List<string> list = new List<string>();
                            if (v.Length == 4) {
                                toWrite += GetQuote(titles, v[0], v[1], proxy, false, list, isCn) + "," + "\"" + v[2] + "\"" + "," + "\"" + v[3] + "\"" + "," + "\"" + list[0] + "\"" + "," + "\"" + list[1] + "\"" + "," + "\"" + list[2] + "\"" + "," + "\"" + list[3] + "\"" + "," + "\"" + list[4] + "\"" + "," + "\"" + list[5] + "\"" + "," + "\"" + list[6] + "\"" + "," + "\"" + list[7] + "\"" + "," + "\"" + list[8] + "\"";
                            } else {
                                toWrite += GetQuote(titles, v[0], v[1], proxy, true, list, isCn) + "," + "\"" + "" + "\"" + "," + "\"" + "" + "\"" + "," + "\"" + list[0] + "\"" + "," + "\"" + list[1] + "\"" + "," + "\"" + list[2] + "\"" + "," + "\"" + list[3] + "\"" + "," + "\"" + list[4] + "\"" + "," + "\"" + list[5] + "\"" + "," + "\"" + list[6] + "\"" + "," + "\"" + list[7] + "\"" + "," + "\"" + list[8] + "\"";
                            }
                            sw.WriteLine(toWrite);
                            sw.Flush();
                        } catch (Exception e) {
                            ChangePorxyIP();
                            errorCount++;
                            if (e.Message.StartsWith("NET") && errorCount < 15) {
                                goto start;
                            }
                            sw_error.WriteLine(str + ",\"" + e.Message + "\"");
                            sw_error.Flush();
                        }

                    }
                    if (count % 200 == 0 && count != 0) {
                        ChangePorxyIP();
                    }
                }
                count++;
            }
            sr.Close();
            sw.Close();
            sw_error.Close();
            if (!run) {
                throw new Exception("error");
            }
        }

        private static void ChangePorxyIP() {
            //GetProxy:
            try {
                proxy = GetProxy();
            } catch (Exception) {
                //goto GetProxy;
            }

        }

        public static string UnicodeToString(string str) {
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(str, delegate(Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        }

        public static string[] GetUriAndSign(string[] titles, string proxy,bool isCn) {
            string title = titles[7];
            string[] strs = new string[2];

            string uri = uri_baidu.Replace("***", title.Replace(" ", "+").Replace("&", "%26"));
            string data = Download(uri, proxy);// proxy);
            if (data.Length < 400) {
                throw new Exception("NET:" + data);
            }
            string http = Regex.Match(data, regex_baidu_http).ToString();
            if (string.IsNullOrEmpty(http)) {
                return GetUriAndSignMult(data, title, proxy, isCn);
            }
            http = http.Substring(http.IndexOf('\"') + 1, http.LastIndexOf('\"') - http.IndexOf('\"') - 1);

            string sign = Regex.Match(data, regex_baidu_sign).ToString();
            sign = sign.Substring(sign.IndexOf('\"') + 1, sign.LastIndexOf('\"') - sign.IndexOf('\"') - 1);
            strs[0] = http;
            strs[1] = sign;
            return strs;
        }
        public static string[] GetUriAndSignMult(string data, string title, string proxy,bool isCn) {
            string[] strs = new string[4];

            var matchs = Regex.Matches(data, regex_baidu_mult_all);
            if (matchs.Count == 0) {
                throw new Exception("正则匹配出现问题，可能为无结果，请找俺重新修改程序");
            }
            if (matchs.Count > 0) {
                double matchValue = 0;
                string matchTitle = "";
                string sign = "";
                string url = "";
                for (int i = 0; i < matchs.Count; i++) {
                    string matchAll = matchs[i].ToString();
                    string matchStr = Regex.Match(matchAll, regex_baidu_mult_title).ToString().Replace("</em>", "").Replace("<em>", "");
                    matchStr = matchStr.Substring(0, matchStr.LastIndexOf("</a>"));
                    string tmpTitle = matchStr.Substring(matchStr.LastIndexOf(@"ank"">") + 5);
                    double tmp = MatchValue(title, tmpTitle, isCn);
                    if (tmp > matchValue) {


                        string tmpurl = matchStr.Substring(matchStr.IndexOf("mu=") + 4);
                        tmpurl = tmpurl.Substring(0, tmpurl.IndexOf("\""));


                        string tmpSign = Regex.Match(matchAll, regex_baidu_sign).ToString();
                        if (string.IsNullOrEmpty(tmpSign)) {
                            continue;
                        }
                        tmpSign = tmpSign.Substring(tmpSign.IndexOf('\"') + 1, tmpSign.LastIndexOf('\"') - tmpSign.IndexOf('\"') - 1);

                        matchValue = tmp;
                        matchTitle = tmpTitle;
                        url = tmpurl;
                        sign = tmpSign;
                        if (tmp == 1) {
                            break;
                        }
                    }
                }
                strs[0] = url;
                strs[1] = sign;
                strs[2] = matchTitle;
                strs[3] = matchValue.ToString();
            }


            return strs;
        }

        public static string Download2(string url) {
            WebClient client = new WebClient();
            string data = client.DownloadString(uri);
            return data;
        }

        public static string GetQuote(string[] titles, string http, string sign, string proxy, bool url, List<string> list,bool isCn) {

            string str = "";
            string qhttp = "";
            if (url) {
                str = http;
                qhttp = UriToStr(http);
            } else {
                str = Uri.EscapeDataString(http.Replace("&amp;", "&"));
                qhttp = http;
            }

            list.Add(qhttp);

            string uri = uri_baidu_download.Replace("***", str).Replace("###", sign);

            string data = Download(uri, proxy);
            if (data.Length < 20 || data.Contains("302 Found")) {
                throw new Exception("NET:Download");
            }
            //string[] datas = data.Replace("\n", "").Split('\r');
            string sreturn = DealData(data, titles, list, isCn).Replace("\\\"", "\"\"");
            return sreturn;

        }

        private static string UriToStr(string http) {
            return http.Replace("%25", "%").Replace("%3A", ":").Replace("%2F", "/");
        }

        public static string DealData(string data, string[] titles, List<string> list,bool isCn) {

            string[] datas = Regex.Split(data, "\",\"");
            string quote = "";
            foreach (var str in datas) {
                if (str.StartsWith("sc_GBT7714")) {
                    quote = str;
                    break;
                }
            }
            quote = quote.Replace("sc_GBT7714\":\"", "");

            string type = quote.Substring(quote.IndexOf("[") + 1, quote.IndexOf("]") - quote.IndexOf("[") - 1).Trim();
            if (type.Trim().ToUpper() == "J") {
                return Deal_J(quote, titles, list, isCn);
            } else if (type.Trim().ToUpper() == "M") {
                return Deal_M(quote, titles, list, isCn);
            } else if (type.Trim().ToUpper() == "C") {
                return Deal_C(quote, titles, list, isCn);
            } else {
                return Deal_J(quote, titles, list, isCn);
            }
        }

        public static Dictionary<string, string> DealData(string[] datas) {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            int auterCount = 0;
            foreach (string data in datas) {
                if (string.IsNullOrEmpty(data)) {
                    continue;
                }
                string key = data.Substring(1, data.IndexOf('}') - 1);
                string value = "\"" + data.Substring(data.IndexOf(':') + 1).Trim() + "\"";
                if (!dic.ContainsKey(key)) {
                    dic.Add(key, value);
                } else {
                    if (key.ToLower().Equals("author") && auterCount < 2) {
                        dic[key] = dic[key] + "," + value;
                        auterCount++;
                    }
                }

            }
            for (int i = auterCount; i < 2; i++) {
                dic["Author"] = dic["Author"] + "," + "\"\"";
            }
            return dic;
        }

        public static string Get(Dictionary<string, string> dic, string key) {
            if (dic.ContainsKey(key)) {
                return dic[key];
            } else {
                return "\"\"";
            }
        }


        /// <summary>
        /// 计算匹配度
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static double MatchValue(string str1_src, string str2_src,bool isCn) {
            double matchValue = 0;
            string str1 = DealStringForMatch(str1_src);
            string str2 = DealStringForMatch(str2_src);

            List<string> strArray1 = ToList(str1,isCn);
            List<string> strArray2 = ToList(str2, isCn);

            foreach (var item in strArray1) {
                if (strArray2.Contains(item)) {
                    strArray2.Remove(item);
                    matchValue++;
                    continue;
                }
            }

            strArray1 = ToList(str1, isCn);
            strArray2 = ToList(str2, isCn);
            foreach (var item in strArray2) {
                if (strArray1.Contains(item)) {
                    strArray1.Remove(item);
                    matchValue++;
                    continue;
                }
            }

            strArray1 = ToList(str1, isCn);
            strArray2 = ToList(str2, isCn);
            matchValue = matchValue / (strArray1.Count + strArray2.Count);

            return Math.Round(matchValue, 2);
        }

        private static string DealStringForMatch(string str_src) {
            string regex = @"[^\w]+";
            string str = ToDBC(str_src);
            str = Regex.Replace(str, regex, " ");
            return str;
        }

        private static List<string> ToList(string str1,bool isCn) {
            List<string> lReturn;
            if (isCn) {
                char[] chars = UnicodeToString(str1).ToCharArray();
                lReturn = new List<string>();
                for (int i = 0; i < chars.Length; i++) {
                    lReturn.Add(chars[i].ToString());
                }
            } else {
                lReturn = str1.ToLower().Split(' ').ToList();
            }
            int count = lReturn.Count;
            for (int i = 0; i < count; i++) {
                lReturn.Remove("");
            }
            return lReturn;
        }
        #endregion




        public bool isCn { get; set; }
        public bool run = true;

        internal void DoSearchWOS(string fileName, string saveName) {
            StreamReader sr = new StreamReader(fileName);
            bool isAppend = false;
            if (startCount > 0) {
                isAppend = true;
            }
            StreamWriter sw = new StreamWriter(saveName, isAppend, Encoding.UTF8);
            StreamWriter sw_error = new StreamWriter(saveName.Substring(0, saveName.LastIndexOf(".")) + ".error.csv", isAppend, Encoding.UTF8);
            StreamWriter sw_wosData = new StreamWriter(saveName.Substring(0, saveName.LastIndexOf(".")) + ".wos.txt", isAppend, Encoding.UTF8);
            string str = "";
            str = sr.ReadLine();
            if (startCount == 0) {
                str = str + ",\"quote\",\"type\",\"auther1\",\"auther2\",\"auther3\",\"title\",\"source_title\",\"year\",\"volume\",\"issue\",\"page\",\"editor_in_chief\",\"place\",\"publisher\",\"文献类型\",\"数字对象标识符\",\"入藏号\",\"修正后source\",\"修正后place\",\"修正后publisher\",\"作者4\",\"matchTitle\",\"matchValue\",\"标题匹配\",\"类型\",\"第一作者\",\"来源\",\"年\",\"卷\",\"期\",\"页\"";

                sw_error.WriteLine(str);
                sw.WriteLine(str);
                sw_wosData.WriteLine("PT	AU	BA	BE	GP	AF	BF	CA	TI	SO	SE	BS	LA	DT	CT	CY	CL	SP	HO	DE	ID	AB	C1	RP	EM	RI	OI	FU	FX	CR	NR	TC	Z9	U1	U2	PU	PI	PA	SN	EI	BN	J9	JI	PD	PY	VL	IS	PN	SU	SI	MA	BP	EP	AR	DI	D2	PG	WC	SC	GA	UT	PM");
            }
            WosSearcher.InitHttp();
            while (!string.IsNullOrEmpty(str = sr.ReadLine()) && run) {
                if (count >= startCount) {
                    string[] titles = Regex.Split(str, "\",\"");
                    errorCount = 0;
                    if (!string.IsNullOrEmpty(titles[7])) {
                        //start:
                        try {
                            List<string> list = new List<string>();
                            string v = GetValuesWOS(titles, list, sw_wosData, isCn);
                            str += "," + v;
                            foreach (var item in list) {
                                str += "," + "\"" + item + "\"";
                            }
                            string toWrite = str;
                            sw.WriteLine(toWrite);
                            sw.Flush();
                        } catch (Exception e) {
                            errorCount++;
                            WosSearcher.InitHttp();
                            if (errorCount < 3) {
                                //goto start;
                            }
                            sw_error.WriteLine(str + ",\"" + e.Message + "\"");
                            sw_error.Flush();
                        }

                    }
                }
                count++;
            }
            sr.Close();
            sw.Close();
            sw_wosData.Close();
            sw_error.Close();
            if (!run) {
                throw new Exception("error");
            }
        }

        private static string GetValuesWOS(string[] titles, List<string> list, StreamWriter sw,bool isCn) {
            string auther0 = "";
            string auther1 = "";
            string auther2 = "";
            string auther3 = "";
            string title = "";
            string type = "";
            string jour = "";
            string year = "";
            string volume = "";
            string mag = "";
            string page = "";
            string publisher = "";
            string place = "";


            string[] values = WosSearcher.Search(titles[7], isCn);
            string[] authors = values[1].Split(';');
            auther0 = WosAuthToUcasAuth(authors[0]);
            if (authors.Length > 1) {
                auther1 = WosAuthToUcasAuth(authors[1]);
            }
            if (authors.Length > 2) {
                auther2 = WosAuthToUcasAuth(authors[2]);
            }
            if (authors.Length > 3) {
                auther3 = WosAuthToUcasAuth(authors[3]);
            }
            title = values[8];
            type = values[0];
            jour = values[9];

            year = values[44];
            volume = values[45];
            mag = values[46];
            string startP = DelFrontZero(values[51]);
            string endP = DelFrontZero(values[52]);
            if (string.IsNullOrEmpty(values[51]) || string.IsNullOrEmpty(values[52])) {
                page = startP + endP;
            } else {
                page = startP + "-" + endP;
            }
            publisher = values[35];
            place = values[36];

            string matchValue = values[values.Length - 1];
            string matchTitle = values[values.Length - 2];

            sw.WriteLine(values[values.Length - 3]);
            sw.Flush();
            //,\"修正后source\",\"修正后place\",\"修正后publisher\",\"作者4\",
            TakeList(titles, list, auther0, title, type, jour, year, volume, mag, page, isCn);
            return ToResoultStr("", auther0, auther1, auther2, title, type, jour, year, volume, mag, page, publisher, place)
                + ",\"" + values[13] + "\""
                + ",\"" + values[54] + "\""
                + ",\"" + values[60] + "\""
                + ",\"" + SourceLetterDeal(jour) + "\""
                + ",\"" + FirstLetterUper(place) + "\""
                + ",\"" + FirstLetterUper(publisher) + "\""
                + ",\"" + auther3 + "\""
                + ",\"" + matchTitle + "\""
                + ",\"" + matchValue + "\"";
        }

        public static string WosAuthToUcasAuth(string author) {
            string sReturn = author.Replace(",", ""); ;
            string[] name = sReturn.Split(' ');
            var tmpName = FirstLetterUper(name[0]);
            if (name.Length == 2) {
                Char[] chars = name[1].ToCharArray();
                for (int i = 0; i < chars.Length; i++) {
                    tmpName += " " + chars[i].ToString();
                }
                sReturn = tmpName;
            }
            return sReturn.Trim();
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="word_src"></param>
        /// <returns></returns>
        public static string FirstLetterUper(string word_src) {
            string word = word_src.Trim();
            if (string.IsNullOrEmpty(word)) {
                return "";
            }
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();

        }


        static List<string> NoUpList = new List<string>() { "an", "a", "the", "of", "and", "or", "in", "to" };
        public static string FirstLetterUper2(string word_src) {
            string word = word_src.Trim();
            if (string.IsNullOrEmpty(word)) {
                return "";
            }
            if (NoUpList.Contains(word.ToLower())) {
                return word.ToLower();
            } else {
                return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
            }

        }

        public static string SourceLetterDeal(string source_src) {
            string[] sourceArray = source_src.Trim().Split(' ');
            string sReturn = "";
            for (int i = 0; i < sourceArray.Length; i++) {
                sReturn += FirstLetterUper2(sourceArray[i]) + " ";
            }
            return sReturn.Trim();
        }

        /// <summary>
        /// 删除开头的0
        /// </summary>
        /// <param name="word_src"></param>
        /// <returns></returns>
        public static string DelFrontZero(string word_src) {
            string word = word_src.Trim();
            while (word.Length > 0 && word.StartsWith("0")) {
                word = word.Substring(1);
            }
            return word;
        }


        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
