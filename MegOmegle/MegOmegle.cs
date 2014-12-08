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
        enum ConvoMode
        {
            REG,
            UNMON,
            SPY,
            UNMONSPY
        };

        private int maxStopHeight, minStopHeight;
        private ConvoMode mode;

        private OmegleClient client;
        private SpyOmegleClient[] strangers;

        private BindingList<string> likes;
        private InterestEditor likeEditor;

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

        private void MegOmegle_Load(object sender, EventArgs e)
        {
            minStopHeight = stopBtn.Height;
            maxStopHeight = stopBtn.Height * 2;

            //Print header
            Version version = new Version(Application.ProductVersion);
            string ver = version.Major + "." + version.Minor + version.MinorRevision;
            string users = HTTPMethods.getASCII(HTTPMethods.postData("http://omegle.com/count", ""));
            if (users.Equals("null")) users = "0"; //In case the server could not be reached

            convoField.sayConsole("MegOmegle " + ver);
            convoField.sayConsole("Matthew Penny 2014");
            convoField.sayConsole(users + " online now.");

            //Set up button dropdown menus
            stopBtnMenu.Items.Add("Regular");
            stopBtnMenu.Items.Add("Unmonitored");
            stopBtnMenu.Items.Add("Spy mode");
            stopBtnMenu.Items.Add("Unmonitored spy mode");
            ((ToolStripMenuItem)stopBtnMenu.Items[0]).Checked = true;
            sendBtnMenu.Items.Add("as " + strangers[0].Partner.getName());
            sendBtnMenu.Items.Add("as " + strangers[1].Partner.getName());
            ((ToolStripMenuItem)sendBtnMenu.Items[0]).Checked = true;
        }

        private void connect(DropDownButton b)
        {
            convoField.clear();
            convoField.sayConsole("Connecting to server...");
            Application.DoEvents();

            bool monMode = (mode != ConvoMode.UNMON && mode != ConvoMode.UNMONSPY);
            bool spyMode = (mode == ConvoMode.SPY || mode == ConvoMode.UNMONSPY);

            //Attempt to connect to Omegle (blocking)
            bool success;
            if (spyMode)
                success = strangers[0].connect(monMode) && strangers[1].connect(monMode);
            else
                success = client.connect(monMode);
            convoField.clear();

            if (success)
            {
                //Enable everything need to chat, as necessary
                b.ButtonText = "Stop";
                b.Height = maxStopHeight;
                b.ArrowEnabled = false;
                interestsBtn.Visible = false;
                msgBox.Enabled = true;
                msgBox.Select();
                msgBox.Clear();
            }
            else
            {
                convoField.sayConsole("Could not connect to server.");
                disconnect(b);
            }
        }

        private void disconnect(DropDownButton b)
        {
            //Disconnect
            if (mode == ConvoMode.SPY)
            {
                strangers[0].disconnect();
                strangers[1].disconnect();
            }
            else
                client.disconnect();

            //Reset buttons/fields as necessary
            sendBtn.Enabled = false;
            msgBox.Enabled = false;
            b.ArrowEnabled = true;
            interestsBtn.Visible = (mode != ConvoMode.UNMON && mode != ConvoMode.UNMONSPY);
            b.ButtonText = "New";
            b.Font = new Font(b.Font, FontStyle.Regular);
            b.Height = minStopHeight;
            b.Select();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            DropDownButton b = (DropDownButton)sender;
            b.ButtonMenu.Hide();

            if (b.ButtonText.Equals("Stop"))
            {
                //Confirm disconnect
                b.Font = new Font(b.Font, FontStyle.Bold);
                b.ButtonText = "Really?";
            }
            else if (b.ButtonText.Equals("Really?"))
                disconnect(b);

            else if (b.ButtonText.Equals("New"))
                connect(b);
        }

        private void stopBtn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                stopBtn.PerformClick();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            DropDownButton b = (DropDownButton)sender;
            b.ButtonMenu.Hide();

            if (!String.IsNullOrEmpty(msgBox.Text))
            {
                if (mode == ConvoMode.SPY || mode == ConvoMode.UNMONSPY)
                {
                    //Get stranger to spoof and send message
                    int i = b.getCheckedIndex();
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

            //Only the clicked should be checked
            foreach (ToolStripMenuItem item in menu.Items)
            {
                if (item.Checked)
                    item.Checked = false;
            }
            ((ToolStripMenuItem)e.ClickedItem).Checked = true;
            mode = (ConvoMode)stopBtn.getCheckedIndex();

            //Update buttons based on the new mode
            bool monMode = (mode != ConvoMode.UNMON && mode != ConvoMode.UNMONSPY);
            bool spyMode = (mode == ConvoMode.SPY || mode == ConvoMode.UNMONSPY);
            sendBtn.ArrowEnabled = spyMode;
            sendBtn.ArrowEnabled = spyMode;
            stopBtn.Height = minStopHeight = (monMode ? maxStopHeight / 2 : maxStopHeight);
            interestsBtn.Visible = monMode;
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
            bool connected;
            if (mode == ConvoMode.SPY)
                connected = strangers[0].isConnected() && strangers[1].isConnected();
            else
                connected = client.isConnected();

            //Update UI on disconnect
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
