using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace LibCheck.Core
{
    public class HttpHelper
    {
        public static string HttpGet(string uri, CookieContainer cookies)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            webRequest.CookieContainer = cookies;
            webRequest.Proxy = null;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = WebRequestMethods.Http.Get;

            WebResponse response = webRequest.GetResponse();

            string tmp = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                tmp = reader.ReadToEnd();
            }
            response.Close();
            return tmp;
        }

        public static string HttpPost(string uri, string parameters, CookieContainer cookies)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            webRequest.CookieContainer = cookies;
            webRequest.Proxy = null;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = WebRequestMethods.Http.Post;
            byte[] bytes = Encoding.ASCII.GetBytes(parameters);
            Stream os = null;
            try
            {
                webRequest.ContentLength = bytes.Length;
                os = webRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
            }
            finally
            {
                if (os != null)
                    os.Close();
            }

            WebResponse webResponse = webRequest.GetResponse();
            if (webResponse == null)
            { return null; }

            string result = string.Empty;

            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd().Trim();
            }
            return result;
        }
    }
}
