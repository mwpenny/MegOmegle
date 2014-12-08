/* OmegleClient.cs
 * 
 * Used for connecting to and chatting on Omegle.
 * 
 */

using System;
using System.Drawing;
using System.Threading;
using System.ComponentModel;

namespace MegOmegle
{
    public class OmegleClient
    {
        private string name, id;

        protected ConvoField console;
        private Color color;
        private Timer updateTimer;
        private Recaptcha rcData;

        protected bool supportsRecaptcha;
        protected bool connected;

        public bool isConnected() { return connected; }
        public bool needsRecaptcha() { return (rcData != null); }
        public string getName() { return name; }
        public Color getColor() { return color; }

        private BindingList<string> likes;

        /// <summary>
        /// A client for the Omegle chat service.
        /// </summary>
        /// <param name="name">The client's name (only visible locally).</param>
        /// <param name="color">The color of the client's name in the chat.</param>
        /// <param name="console">The <see cref="ConvoField"/> to display the chat in.</param>
        /// <param name="likes">The list of common interests to filter partners with.</param>
        public OmegleClient(string name, Color color, ConvoField console, BindingList<string> likes=null)
        {
            this.name = name;
            this.color = color;
            this.console = console;
            this.likes = likes;
            supportsRecaptcha = true;
            rcData = null;
            updateTimer = new Timer(updateTimer_tick, null, Timeout.Infinite, 1000); //get events every second
        }

        /// <summary>
        /// Attempts to connect to someone on Omegle.
        /// </summary>
        /// <param name="monitored">Whether or not to join monitored mode.</param>
        /// <returns>Whether or not the connection was successful.</returns>
        public bool connect(bool monitored=true)
        {
            if (connected)
                disconnect();
            
            //Attempt to join omegle
            byte[] raw = HTTPMethods.getData("http://omegle.com/start?" +
                (supportsRecaptcha ? "rcs=1" : "") +
                (!monitored ? "&group=unmon" : "") +
                (monitored ? "&topics=" + queryFormatLikes(likes) : "")); //Can only share interests when monitored
            string response = HTTPMethods.getASCII(raw);

            //Omegle will return the string "null", rather than nothing, if unsuccessful
            if (!response.Equals("null"))
            {
                id = response.Substring(1, response.Length - 2); //trim quotes
                updateTimer.Change(0, 1000);
                connected = true;
                return true;
            }
            return false;
        }

        private string queryFormatLikes(BindingList<string> likes)
        {
            string likeString = "[";
            if (likes != null)
            {
                //Comma delimit the likes
                if (likes.Count > 0)
                    likeString += "\"" + likes[0] + "\"";
                for (int i = 1; i < likes.Count; i++)
                    likeString += ",\"" + likes[i] + "\"";
            }
            likeString += "]";

            return likeString;
        }

        /// <summary>
        /// Sends a message to the connected chat partner.
        /// </summary>
        /// <param name="message"></param>
        public void send(string message)
        {
            setTyping(false);

            //Send the message
            HTTPMethods.postDataAsync("http://omegle.com/send",
                "id=" + id +
                "&msg=" + message,
                null);
        }

        private void showRecaptcha(string key)
        {
            //Show a reCaptcha image using the provided key
            console.clear();
            rcData = new Recaptcha(key);
            console.sayConsole("Please type the text you see in the image.");
            console.insertImage(rcData.rcImage);
        }

        /// <summary>
        /// Sends a reCaptcha response.
        /// </summary>
        /// <param name="response">The reCaptcha response.</param>
        public void validateRecaptcha(string response)
        {
            HTTPMethods.postDataAsync("http://omegle.com/recaptcha",
                "id=" + id +
                "&challenge=" + rcData.challenge +
                "&response=" + response,
                parseEvents);
            console.sayConsole("Verifying...");
        }

        /// <summary>
        /// Sets the typing state of the client.
        /// </summary>
        /// <param name="state">Whether or not this client is typing.</param>
        public void setTyping(bool typing)
        {
            if (typing)
                HTTPMethods.postDataAsync("http://omegle.com/typing", "id=" + id, null);
            else
                HTTPMethods.postDataAsync("http://omegle.com/stoppedtyping", "id=" + id, null);
        }

        /// <summary>
        /// Disconnects the client from the Omegle session.
        /// </summary>
        public void disconnect()
        {
            if (connected)
            {
                connected = false;
                HTTPMethods.postDataAsync("http://omegle.com/disconnect", "id=" + id, null);
                strangerTyping(false);
                console.sayConsole("\r\n" + name + " disconnected.");
            }
        }

        //private void updateTimer_tick(object sender, EventArgs e)
        private void updateTimer_tick(object o)
        {
            //Stop the timer when disconnected
            if (!connected)
            {
                updateTimer.Change(Timeout.Infinite, 1000);
                return;
            }

            if (console.InvokeRequired)
            {
                console.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate()
                {
                    HTTPMethods.postDataAsync("http://omegle.com/events", "id=" + id, parseEvents);
                });
            }
            else
            {
                HTTPMethods.postDataAsync("http://omegle.com/events", "id=" + id, parseEvents);
            }
        }

        private void parseEvents(byte[] raw)
        {
            string response = HTTPMethods.getASCII(raw);

            if (!response.Equals("null"))
            {
                //Handle events
                foreach (OmegleEvent e in OmegleEvent.eventsFromJSON(response))
                {
                    if (e.oEvent.Equals("waiting"))
                    {
                        console.clear();
                        rcData = null;
                        console.sayConsole("Looking for someone you can chat with...");
                    }
                    else if (e.oEvent.Equals("connected"))
                    {
                        console.clear();
                        foundStranger();
                    }
                    else if (e.oEvent.Equals("commonLikes"))
                        commonLikes(e.values);
                    else if (e.oEvent.Equals("typing"))
                        strangerTyping(true);
                    else if (e.oEvent.Equals("stoppedTyping"))
                        strangerTyping(false);
                    else if (e.oEvent.Equals("gotMessage"))
                        gotMessage(e.values[0]);
                    else if (e.oEvent.Equals("strangerDisconnected"))
                        strangerDisconnected();
                    else if (e.oEvent.Equals("recaptchaRequired") || e.oEvent.Equals("recaptchaRejected"))
                        showRecaptcha(e.values[0]);
                    else if (e.oEvent.Equals("error"))
                    {
                        console.sayConsole("Oh no! There was an error!");
                        console.sayConsole("(" + e.values[0] + ")");
                        disconnect();
                    }
                    else if (e.oEvent.Equals("connectionDied"))
                    {
                        console.sayConsole("Connection died!");
                        disconnect();
                    }
                    else if (e.oEvent.Equals("antinudeBanned"))
                    {
                        console.sayConsole("Banned for bad behavior! Use unmonitored mode.");
                        disconnect();
                    }
                }
            }
        }

        protected virtual void foundStranger()
        {
            console.sayConsole("You're now chatting with a random stranger. Say hi!\r\n");
        }

        protected string delimitLikes(string[] likes)
        {
            //Comma delimit shared interests
            string likesString = likes[0];

            for (int i = 1; i < likes.Length - 1; i++)
                likesString += ", " + likes[i];

            if (likes.Length > 2) likesString += ",";
            if (likes.Length > 1) likesString += " and " + likes[likes.Length - 1];

            return likesString;
        }

        protected virtual void commonLikes(string[] likes)
        {
            console.sayConsole("You both like " + delimitLikes(likes) + ".\r\n");
        }

        protected virtual void strangerTyping(bool typing)
        {
            if (typing)
                console.Status = "Stranger is typing...";
            else
                console.Status = "";
        }

        protected virtual void gotMessage(string message)
        {
            strangerTyping(false);
            console.sayUser("Stranger", Color.Red, message);
        }

        protected virtual void strangerDisconnected()
        {
            strangerTyping(false);
            connected = false;
            console.sayConsole("\r\nStranger disconnected.");
        }
    }
}
