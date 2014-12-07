/* MegOmegle.cs
 * 
 * The main chat window.
 * Allows toggling between normal conversation and spy mode.
 * 
 */

using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace MegOmegle
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;

            likes = new BindingList<string>();
            likeEditor = new InterestEditor(likes);
            
            client = new OmegleClient("You", Color.Blue, convoField, likes);

            //Init spy mode clients
            strangers = new SpyOmegleClient[2];
            strangers[0] = new SpyOmegleClient("Stranger1", Color.Red, convoField, likes);
            strangers[1] = new SpyOmegleClient("Stranger2", Color.Green, convoField, likes);
            strangers[0].Partner = strangers[1];
            strangers[1].Partner = strangers[0];
        }

        private OmegleClient client;
        private SpyOmegleClient[] strangers;
        private bool spyMode;

        private BindingList<string> likes;
        private InterestEditor likeEditor;

        private void MegOmegle_Load(object sender, EventArgs e)
        {
            //Print header
            Version version = new Version(Application.ProductVersion);
            string ver = version.Major + "." + version.Minor + version.MinorRevision;
            string users = Encoding.ASCII.GetString(HTTPMethods.postData("http://omegle.com/count", ""));

            convoField.sayConsole("MegOmegle " + ver);
            convoField.sayConsole("Matthew Penny 2014");
            convoField.sayConsole(users + " online now.\r\n");

            //Set up button dropdown menus
            stopBtnMenu.Items.Add("Regular");
            stopBtnMenu.Items.Add("Spy mode");
            ((ToolStripMenuItem)stopBtnMenu.Items[0]).Checked = true;
            sendBtnMenu.Items.Add("as " + strangers[0].Partner.getName());
            sendBtnMenu.Items.Add("as " + strangers[1].Partner.getName());
            ((ToolStripMenuItem)sendBtnMenu.Items[0]).Checked = true;
        }

        private void connect(DropDownButton b)
        {
            //Attempt to connect to Omegle
            bool success = spyMode ? strangers[0].connect() && strangers[1].connect() : client.connect();

            if (success)
            {
                //Enable everything need to chat
                msgBox.Enabled = true;
                sendBtn.ArrowEnabled = spyMode;
                convoField.clear();
                msgBox.Select();
                msgBox.Clear();
            }
            else
                disconnect(b);
        }

        private void disconnect(DropDownButton b)
        {
            //Disconnect
            if (spyMode)
            {
                strangers[0].disconnect();
                strangers[1].disconnect();
            }
            else
                client.disconnect();

            //Reset buttons/fields
            sendBtn.Enabled = false;
            msgBox.Enabled = false;
            b.ArrowEnabled = true;
            b.ButtonText = "New";
            b.Font = new Font(b.Font, FontStyle.Regular);
            b.Height /= 2;
            interestsBtn.Visible = true;
            stopBtn.Select();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            DropDownButton b = (DropDownButton)sender;

            if (b.ButtonText.Equals("Stop"))
            {
                //Confirm disconnect
                b.Font = new Font(b.Font, FontStyle.Bold);
                b.ButtonText = "Really?";
            }
            else if (b.ButtonText.Equals("Really?"))
                disconnect(b);

            else if (b.ButtonText.Equals("New"))
            {
                //Set up buttons/fields depending on mode
                int i = stopBtn.getCheckedIndex();
                spyMode = i == 1;
                sendBtn.ArrowEnabled = spyMode;
                b.ButtonText = "Stop";
                b.Height *= 2;
                b.ArrowEnabled = false;
                interestsBtn.Visible = false;
                connect(stopBtn);
            }
        }

        private void stopBtn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                stopBtn.PerformClick();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(msgBox.Text))
            {
                if (spyMode)
                {
                    //Get stranger to spoof and send message
                    int i = sendBtn.getCheckedIndex();
                    if (i >= 0)
                    {
                        strangers[i].send(msgBox.Text);
                        convoField.sayUser(strangers[i].Partner.getName() + " (you)", strangers[i].Partner.getColor(), msgBox.Text);
                    }
                }
                else if (client.needsRecaptcha())
                {
                    //Send recaptcha response
                    client.validateRecaptcha(msgBox.Text);
                }
                else
                {
                    //Send message to stranger
                    client.send(msgBox.Text);
                    convoField.sayUser(client.getName(), client.getColor(), msgBox.Text);
                }
            }
            msgBox.Clear();
        }

        private void btnMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = (ContextMenuStrip)sender;

            //Only the clicked item can be checked
            foreach (ToolStripMenuItem item in menu.Items)
            {
                if (item.Checked)
                    item.Checked = false;
            }
            ((ToolStripMenuItem)e.ClickedItem).Checked = true;
        }

        private void msgBox_TextChanged(object sender, EventArgs e)
        {
            //Toggle typing when going from nothing to something, or vice versa
            bool empty = String.IsNullOrEmpty(((TextBox)sender).Text);
            client.setTyping(!empty);
            sendBtn.Enabled = (!empty);
        }

        private void msgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Shift && e.KeyCode == Keys.Enter)
            {
                sendBtn.PerformClick();
                e.SuppressKeyPress = true;
            }

            else if (e.KeyCode == Keys.Escape)
                stopBtn.PerformClick();

            else if (client.isConnected() && !stopBtn.Text.Equals("Stop"))
            {
                //Reset stop button if it was pressed once or something
                stopBtn.Font = new Font(stopBtn.Font, FontStyle.Regular);
                stopBtn.ButtonText = "Stop";
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            bool connected = spyMode ? strangers[0].isConnected() && strangers[1].isConnected() : client.isConnected();

            //Update buttons on disconnect
            if (!connected && !stopBtn.ButtonText.Equals("New"))
                disconnect(stopBtn);
        }

        private void interestsBtn_Click(object sender, EventArgs e)
        {
            //Edit shared interests
            likeEditor.ShowDialog();
            stopBtn.Select();
        }
    }
}
