/* RecaptchaData.cs
 * 
 * Encapsulates the data needed for reCaptcha validation.
 * 
 */

using System;
using System.Text;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace MegOmegle
{
    class Recaptcha
    {
        public Image rcImage;
        public string challenge;

        /// <summary>
        /// Stores the challenge and response for a reCaptcha validation.
        /// </summary>
        /// <param name="key">The public key of the site.</param>
        public Recaptcha(string key)
        {
            challenge = getChallenge(key);
            rcImage = getImage(challenge);
        }

        /// <summary>
        /// Gets the challenge code for the reCaptcha image.
        /// </summary>
        /// <param name="raw">The source of the page google.com/recaptcha/api/challenge?k=[key].</param>
        /// <returns>The challenge value for the recaptcha image.</returns>
        public static string getChallenge(string key)
        {
            //Use key with reCaptcha api, and parse the result for the challenge id
            byte[] response = HTTPMethods.getData("http://google.com/recaptcha/api/challenge?k=" + key);
            string raw = Encoding.ASCII.GetString(response);
            Match element = Regex.Match(raw, "challenge : \'(.*?)\'"); //Literally magic
            return element.Groups[1].ToString();
        }

        /// <summary>
        /// Gets the reCaptcha image to validate.
        /// </summary>
        /// <param name="challenge">The challenge value of the image.</param>
        /// <returns>The reCaptcha image.</returns>
        public static Image getImage(string challenge)
        {
            //Use the challange value to get the captcha image
            string url = "http://google.com/recaptcha/api/image?c=" + challenge;
            Image i;

            using (MemoryStream imgStream = new MemoryStream(HTTPMethods.getData(url)))
                i = Image.FromStream(imgStream);

            return i;
        }
    }
}
