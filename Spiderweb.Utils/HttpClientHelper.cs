using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Spiderweb.Utils
{
    public class HttpClientHelper
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url, out string statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        public static string RestfulGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            // Get response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());
                // Console application output
                return reader.ReadToEnd();
            }
        }

        public static T GetResponse<T>(string url)
            where T : class, new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, string postData, out string statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";

            HttpClient httpClient = new HttpClient();
            //httpClient..setParameter(HttpMethodParams.HTTP_CONTENT_CHARSET, "utf-8");

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }

            return null;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData)
            where T : class, new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            StringContent httpContent = new StringContent(postData, Encoding.UTF8, "text/json");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httpContent.Headers.ContentType.CharSet = "utf-8";

            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            httpClient.Timeout = new TimeSpan(0, 0, 1);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.26.2");
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

            T result = default(T);

            try
            {
                Task<HttpResponseMessage> httpTask = httpClient.PostAsync(url, httpContent);
                Thread.Sleep(50);
                HttpResponseMessage response = httpTask.Result;
                httpTask.Dispose();

                if (response.IsSuccessStatusCode)
                {
                    Task<string> t = response.Content.ReadAsStringAsync();
                    string s = t.Result;

                    result = JsonConvert.DeserializeObject<T>(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData, string token)
            where T : class, new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            StringContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            httpContent.Headers.Add("token", token);

            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, POST_TIME_OUT);
            //httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.26.2");
            //httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

            T result = default(T);

            try
            {
                Task<HttpResponseMessage> httpTask = httpClient.PostAsync(url, httpContent);
                Thread.Sleep(50);
                HttpResponseMessage response = httpTask.Result;
                httpTask.Dispose();

                if (response.IsSuccessStatusCode)
                {
                    Task<string> t = response.Content.ReadAsStringAsync();
                    string s = t.Result;

                    result = JsonConvert.DeserializeObject<T>(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("POST操作出错", ex);
            }

            return result;
        }

        const int POST_TIME_OUT = 5000;

        public static T PostResponse<T>(Uri url, string content)
        {
            string result = "";
            T resultT = default(T);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            req.Timeout = POST_TIME_OUT;
            Console.WriteLine(req.Headers.Count);

            byte[] byteData = Encoding.UTF8.GetBytes(content);
            req.ContentLength = byteData.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.ReadTimeout = POST_TIME_OUT;
                    reqStream.WriteTimeout = POST_TIME_OUT;
                    reqStream.Write(byteData, 0, byteData.Length);
                    reqStream.Close();
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("POST操作出错", ex);
            }

            if (!string.IsNullOrEmpty(result))
                resultT = JsonConvert.DeserializeObject<T>(result);

            return resultT;
        }


        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
            where T : class, new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }

        public static string PostResponse(string url, string postData, string token, string appId, string serviceURL, out string statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";

            httpContent.Headers.Add("token", token);
            httpContent.Headers.Add("appId", appId);
            httpContent.Headers.Add("serviceURL", serviceURL);


            HttpClient httpClient = new HttpClient();
            //httpClient..setParameter(HttpMethodParams.HTTP_CONTENT_CHARSET, "utf-8");

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }

            return null;
        }

        /// <summary>
        /// 修改API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string KongPatchResponse(string url, string postData)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "PATCH";

            byte[] btBodys = Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        /// <summary>
        /// 创建API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string KongAddResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };
            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 删除API
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool KongDeleteResponse(string url)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.DeleteAsync(url).Result;
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// 修改或者更改API        
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string KongPutResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };

            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.PutAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 检索API
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string KongSerchResponse(string url)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }
    }
}
