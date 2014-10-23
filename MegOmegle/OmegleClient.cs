/* OmegleClient.cs
 * 
 * Used for connecting to and chatting on Omegle.
 * 
 */

using System;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;

namespace MegOmegle
{
    public class OmegleClient
    {
        private string name, id;
        protected ConvoField console;
        private Color color;
        private Timer updateTimer;
        protected bool connected;
        protected bool typing;

        public bool isConnected() { return connected; }
        public string getName() { return name; }
        public Color getColor() { return color; }

        /// <summary>
        /// A client for the Omegle chat service.
        /// </summary>
        /// <param name="name">The client's name (only visible locally).</param>
        /// <param name="color">The color of the client's name in the chat.</param>
        /// <param name="console">The <see cref="ConvoField"/> to display the chat in.</param>
        public OmegleClient(string name, Color color, ConvoField console)
        {
            this.name = name;
            this.color = color;
            this.console = console;
            updateTimer = new Timer(updateTimer_tick, null, Timeout.Infinite, 1000); //get events every second
        }

        /// <summary>
        /// Attempts to connect to someone on Omegle.
        /// </summary>
        /// <returns>Whether or not the connection was successful.</returns>
        public bool connect()
        {
            //Attempt to join omegle
            if (connected)
                disconnect();
            console.sayConsole("Connecting to server...");
            string response = HTTPMethods.postData("http://omegle.com/start", "");

            if (!response.Equals("null")) //omegle will return the string "null", rather than nothing
            {
                id = response.Substring(1, response.Length - 2); //trim quotes
                updateTimer.Change(0, 1000);
                connected = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends a message to the connected chat partner.
        /// </summary>
        /// <param name="message"></param>
        public void send(string message)
        {
            //Stop typing
            if (typing)
            {
                typing = false;
                toggleTyping();
            }

            //Send the message
            HTTPMethods.postDataAsync("http://omegle.com/send", "id=" + id + "&msg=" + message, null);
        }

        /// <summary>
        /// Sets the typing state of the client.
        /// </summary>
        /// <param name="state">Whether or not this client is typing.</param>
        public void setTyping(bool state)
        {
            //Only toggle typing if the new state is different
            if (state != typing)
            {
                typing = state;
                toggleTyping();
            }
        }

        /// <summary>
        /// Disconnects the client from the Omegle session.
        /// </summary>
        public void disconnect()
        {
            if (connected)
            {
                connected = false;
                typing = false;
                HTTPMethods.postDataAsync("http://omegle.com/disconnect", "id=" + id, null);
                strangerTyping(false);
                console.sayConsole(name + " disconnected.");                    
            }
        }

        private void toggleTyping()
        {
            HTTPMethods.postDataAsync("http://omegle.com/typing", "id=" + id, null);
        }

        private void updateTimer_tick(object o)
        {
            //Stop the timer when disconnected
            if (!connected)
            {
                updateTimer.Change(Timeout.Infinite, 1000);
                return;
            }

            //Prevent an exception for cross-thread operations (modifying the chat view)
            if (console.InvokeRequired)
                console.Invoke((System.Windows.Forms.MethodInvoker)delegate { HTTPMethods.postDataAsync("http://omegle.com/events", "id=" + id, parseEvents); });
            else
            {
                HTTPMethods.postDataAsync("http://omegle.com/events", "id=" + id, parseEvents);
            }
        }

        private void parseEvents(string response)
        {
            if (response != "null")
            {
                response = response.Substring(1, response.Length - 2); //trim the []

                //Get the events
                foreach (Match m in Regex.Matches(response, "\\[(.*?)\\]")) //Everybody stand back!
                {
                    string e = m.Groups[1].Value;
                    
                    if (!string.IsNullOrEmpty(e))
                    {
                        //Get the parameters/arguments, etc. of the event
                        MatchCollection elements = Regex.Matches(e, "\"(.*?)\""); //Now I have 2 problems

                        if (elements[0].Groups[1].Value == "waiting")
                            console.sayConsole("Looking for someone you can chat with...");
                        else if (elements[0].Groups[1].Value == "connected")
                            foundStranger();
                        else if (elements[0].Groups[1].Value == "typing")
                            strangerTyping(true);
                        else if (elements[0].Groups[1].Value == "stoppedTyping")
                            strangerTyping(false);
                        else if (elements[0].Groups[1].Value == "gotMessage")
                            gotMessage(elements[1].Groups[1].Value);
                        else if (elements[0].Groups[1].Value == "strangerDisconnected")
                            strangerDisconnected();
                        else if (elements[0].Groups[1].Value == "recaptchaRequired")
                            console.sayConsole("Oh no! reCaptcha (not implemented)!");
                    }
                }
            }
        }

        protected virtual void foundStranger()
        {
            console.sayConsole("You're now chatting with a random stranger. Say hi!\r\n");
        }

        protected virtual void strangerTyping(bool typing)
        {
            if (typing)
                console.setStatus("Stranger is typing...");
            else
                console.setStatus("");
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
            console.sayConsole("Stranger disconnected.");
        }
    }
}
