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
        /// <summary>
        /// Synchronously posts data to a web server.
        /// </summary>
        /// <param name="url">The location to post the data to.</param>
        /// <param name="data">The data to post.</param>
        /// <returns>The server's response.</returns>
        public static string postData(string url, string data)
        {
            //Attempt to post the data
            try
            {
                using (WebClient w = new WebClient())
                    return Encoding.ASCII.GetString(w.UploadValues(url, getKVPs(data)));
            }
            catch { return "null"; }
        }

        public delegate void PostCallBack(string data); //The method to send the response to

        /// <summary>
        /// Asynchronously posts data to a web server.
        /// </summary>
        /// <param name="url">The location to post the data to.</param>
        /// <param name="data">The data to post.</param>
        /// <param name="callback">The <see cref="PostCallBack"/> to call with the server's response.</param>
        public static void postDataAsync(string url, string data, PostCallBack callback)
        {
            using (WebClient w = new WebClient())
            {
                //Sets up the event handler
                w.UploadValuesCompleted += delegate(object sender, UploadValuesCompletedEventArgs e)
                {
                    try { callback(Encoding.ASCII.GetString(e.Result)); }
                    catch { }; //Because of the server closing the connection and null callbacks
                };

                w.UploadValuesAsync(new Uri(url), getKVPs(data));
            }
        }

        private static  NameValueCollection getKVPs(string raw)
        {
            NameValueCollection values = new NameValueCollection();

            //Get each key/value pair from the raw string
            if (raw.Contains("="))
                foreach (string pair in raw.Split('&'))
                {
                    string[] splitPair = pair.Split('=');
                    values[splitPair[0]] = splitPair[1];
                }

            return values;
        }
    }
}
