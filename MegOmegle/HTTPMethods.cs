/* HTTPMethods.cs
 * 
 * General methods for transmitting data over HTTP.
 * 
 */

using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;

namespace MegOmegle
{
    class HTTPMethods
    {
        public delegate void ResponseCallBack(byte[] data); //The method to send the response to

        private static NameValueCollection getKVPs(string raw)
        {
            NameValueCollection values = new NameValueCollection();

            //Get each key/value pair from the raw string
            if (raw.Contains("="))
            {
                foreach (string pair in raw.Split('&'))
                {
                    string[] splitPair = pair.Split('=');
                    values[splitPair[0]] = splitPair[1];
                }
            }

            return values;
        }

        /// <summary>
        /// Converts a raw response into an ASCII string.
        /// </summary>
        /// <param name="raw">The raw server response.</param>
        /// <returns>The response in ASCII, or "null" if the response was null.</returns>
        public static string getASCII(byte[] raw)
        {
            return raw != null ? Encoding.ASCII.GetString(raw) : "null";
        }

        /// <summary>
        /// Synchronously gets data from a web server.
        /// </summary>
        /// <param name="url">The location of the data to retrieve.</param>
        /// <returns>The resource specified by the URL.</returns>
        public static byte[] getData(string url)
        {
            //Attempt to get the data
            try
            {
                using (WebClient w = new WebClient())
                    return w.DownloadData(url);
            }
            catch { return null; }
        }

        /// <summary>
        /// Asynchronously gets data from a web server.
        /// </summary>
        /// <param name="url">The location of the data to retrieve.</param>
        /// <param name="callback">The <see cref="ResponseCallback"/> to call with the server's response.</param>
        public static void getDataAsync(string url, ResponseCallBack callback)
        {
            using (WebClient w = new WebClient())
            {
                //Sets up the event handler
                w.DownloadDataCompleted += delegate(object sender, DownloadDataCompletedEventArgs e)
                {
                    if (e.Error == null && callback != null)
                        callback(e.Result);
                };

                try { w.DownloadDataAsync(new Uri(url)); }
                catch { callback(null); }
            }
        }

        /// <summary>
        /// Synchronously posts data to a web server.
        /// </summary>
        /// <param name="url">The location to post the data to.</param>
        /// <param name="data">The data to post.</param>
        /// <returns>The server's response.</returns>
        public static byte[] postData(string url, string data)
        {
            //Attempt to post the data
            try
            {
                using (WebClient w = new WebClient())
                    return w.UploadValues(url, getKVPs(data));
            }
            catch { return null; }
        }

        /// <summary>
        /// Asynchronously posts data to a web server.
        /// </summary>
        /// <param name="url">The location to post the data to.</param>
        /// <param name="data">The data to post.</param>
        /// <param name="callback">The <see cref="ResponseCallback"/> to call with the server's response.</param>
        public static void postDataAsync(string url, string data, ResponseCallBack callback)
        {
            using (WebClient w = new WebClient())
            {
                //Sets up the event handler
                w.UploadValuesCompleted += delegate(object sender, UploadValuesCompletedEventArgs e)
                {
                    if (e.Error == null && callback != null)
                        callback(e.Result);
                };

                w.UploadValuesAsync(new Uri(url), getKVPs(data));
            }
        }
    }
}
