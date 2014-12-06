/* OmegleClient.cs
 * 
 * Used for connecting one stranger to another.
 * 
 */

using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace MegOmegle
{
    class SpyOmegleClient : OmegleClient
    {
        public SpyOmegleClient Partner { get; set; }
        public SpyOmegleClient(string name, Color color, ConvoField console, BindingList<string> likes = null) : base(name, color, console, likes) { }

        protected override void foundStranger()
        {
            console.sayConsole(getName() + " connected.\r\n");
        }

        protected override void commonLikes(string[] likes)
        {
            console.sayConsole("The stranger likes " + delimitLikes(likes) + ".");
        }

        protected override void strangerTyping(bool typing)
        {
            if (typing)
            {
                //Relay typing state to partner
                console.setStatus(getName() + " is typing...");
                Partner.setTyping(!Partner.typing);
            }
            else
                console.setStatus("");
        }

        protected override void gotMessage(string message)
        {
            //Relay message to partner
            strangerTyping(false);
            console.sayUser(getName(), getColor(), message);
            Partner.send(message);
        }

        protected override void strangerDisconnected()
        {
            //Disconnect from partner as well
            strangerTyping(false);
            connected = false;
            console.sayConsole(getName() + " disconnected.");
            Partner.disconnect();
        }
    }
}
