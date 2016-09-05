using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WosNumberSearch {
    public class WosSearcher {
        public static string jessionID = "";
        public static string sID = "";
        static HttpHelper http;
        static HttpResult result;
        static string cookie;
        public static string[] Search(string title ,bool isCn) {
            return DownLoad(http, ref result, cookie, title, isCn);
        }

        public  static void InitHttp() {
            http = new HttpHelper();
            HttpItem item = new HttpItem() {
                URL = "http://apps.webofknowledge.com/",//URL     必需项    
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
                ProxyIp = "",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数 
                //ProxyPwd = "123456",//代理服务器密码     可选项    
                //ProxyUserName = "administrator",//代理服务器账户名     可选项   
                ResultType = ResultType.String
            };
            result = http.GetHtml(item);
            string html = result.Html;
            cookie = result.Cookie;

            MoveTmp(http, ref result, "");
            cookie = "SID=\"" + sID + "\"; CUSTOMER=\"CAS National Sciences Library of Chinese Academy of Sciences\"; E_GROUP_NAME=\"CAS Library of Beijing Headquarter\"; JSESSIONID=" + jessionID;
        }

        public static string reg_title = @"doc=\d{0,3}""><value lang_id="""">.*?</value>";
        public static string reg_rurl = "<input type=\"hidden\" id=\"rurl\" name=\"rurl\" value=\".*?\" />";
        public static string reg_qid = @"<input type=""hidden"" name=""qid"" value=""\d*""/>";
        private static string[] DownLoad(HttpHelper http, ref HttpResult result, string cookie, string title,bool isCn) {
            string[] sReturn = null;
            HttpItem item = new HttpItem() {
                URL = "http://apps.webofknowledge.com/WOS_GeneralSearch.do",//URL     必需项    
                Method = "POST",//URL     可选项 默认为Get   
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写   
                Cookie = cookie,//字符串Cookie     可选项   
                Referer = "",//来源URL     可选项   

                Postdata = "fieldCount=1&action=search&product=WOS&search_mode=GeneralSearch&SID=" + sID + "&max_field_count=50&max_field_notice=%E6%B3%A8%E6%84%8F%3A+%E6%97%A0%E6%B3%95%E6%B7%BB%E5%8A%A0%E5%8F%A6%E4%B8%80%E5%AD%97%E6%AE%B5%E3%80%82&input_invalid_notice=%E6%A3%80%E7%B4%A2%E9%94%99%E8%AF%AF%3A+%E8%AF%B7%E8%BE%93%E5%85%A5%E6%A3%80%E7%B4%A2%E8%AF%8D%E3%80%82&exp_notice=%E6%A3%80%E7%B4%A2%E9%94%99%E8%AF%AF%3A+%E4%B8%93%E5%88%A9%E6%A3%80%E7%B4%A2%E8%AF%8D%E5%8F%AF%E5%9C%A8%E5%A4%9A%E4%B8%AA%E5%AE%B6%E6%97%8F%E4%B8%AD%E6%89%BE%E5%88%B0+%28&input_invalid_notice_limits=+%3Cbr%2F%3E%E6%B3%A8%3A+%E6%BB%9A%E5%8A%A8%E6%A1%86%E4%B8%AD%E6%98%BE%E7%A4%BA%E7%9A%84%E5%AD%97%E6%AE%B5%E5%BF%85%E9%A1%BB%E8%87%B3%E5%B0%91%E4%B8%8E%E4%B8%80%E4%B8%AA%E5%85%B6%E4%BB%96%E6%A3%80%E7%B4%A2%E5%AD%97%E6%AE%B5%E7%9B%B8%E7%BB%84%E9%85%8D%E3%80%82&sa_params=WOS%7C%7C" + sID + "%7Chttp%3A%2F%2Fapps.webofknowledge.com%7C%27&formUpdated=true&value%28input1%29=" + title + "&value%28select1%29=UT&x=912&y=298&value%28hidInput1%29=&limitStatus=collapsed&ss_lemmatization=On&ss_spellchecking=Suggest&SinceLastVisit_UTC=&SinceLastVisit_DATE=&period=Range+Selection&range=ALL&startYear=1900&endYear=2016&editions=SCI&editions=SSCI&editions=ISTP&editions=CCR&editions=IC&update_back2search_link_param=yes&ssStatus=display%3Anone&ss_showsuggestions=ON&ss_numDefaultGeneralSearchFields=1&ss_query_language=&rs_sort_by=PY.D%3BLD.D%3BSO.A%3BVL.D%3BPG.A%3BAU.A",
                Timeout = 100000,//连接超时时间     可选项默认为100000    
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000   
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
                ResultType = ResultType.String,
                Host = "apps.webofknowledge.com",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            };

            item.Header.Add("Origin", "http://apps.webofknowledge.com");

            result = http.GetHtml(item);
            MoveTmp(http, ref result, cookie);


            sReturn = new string[1];
            sReturn[0] = result.Html;
            return sReturn;
        }

        private static void MoveTmp(HttpHelper http, ref HttpResult result, string cookie) {
            HttpItem item;
            string c = "";
            if (string.IsNullOrEmpty(cookie)) {
                c = result.Cookie;
            } else {
                c = cookie;
            }
            while (result.StatusDescription == "Moved Temporarily") {
                item = new HttpItem() {
                    URL = result.Header.GetValues("Location")[0],//URL     必需项    
                    Method = "get",//URL     可选项 默认为Get   
                    IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写   
                    Cookie = c,//字符串Cookie     可选项   
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
                    ProxyIp = "",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数 
                    //ProxyPwd = "123456",//代理服务器密码     可选项    
                    //ProxyUserName = "administrator",//代理服务器账户名     可选项   
                    ResultType = ResultType.String
                };
                result = http.GetHtml(item);
                if (!string.IsNullOrEmpty(result.Cookie)) {
                    if (result.Cookie.Contains("JSESSIONID=")) {
                        string str = result.Cookie;
                        jessionID = str.Substring(str.IndexOf("JSESSIONID="));
                        jessionID = jessionID.Substring(jessionID.IndexOf("=") + 1);
                        jessionID = jessionID.Substring(0, 32);
                    }
                    if (result.Cookie.Contains("SID=")) {
                        string str = result.Cookie;
                        sID = str.Substring(str.IndexOf("SID="));
                        sID = sID.Substring(sID.IndexOf("\"") + 1);
                        sID = sID.Substring(0, sID.IndexOf("\""));
                    }
                }

            }
        }
    }
}
