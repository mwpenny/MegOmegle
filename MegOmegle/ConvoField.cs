/* MegOmegle.cs
 * 
 * A RichTextField to nicely format and display conversations in.
 * 
 */

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MegOmegle
{
    public partial class ConvoField : UserControl
    {
        private Form parent;
        public string Status
        {
            get { return statusLabel.Text; }
            set { statusLabel.Text = value; }
        }

        public ConvoField(Form parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void AppendText(Color color, FontStyle style, string text)
        {
            int start = textField.TextLength;
            textField.AppendText(text);
            int end = textField.TextLength;

            //Textbox may transform chars, so (end-start) != text.Length
            textField.Select(start, end - start);
            textField.SelectionColor = color;
            textField.SelectionFont = new Font(textField.Font, style);
            textField.SelectionLength = 0; //clear

            //scroll to end
            textField.SelectionStart = textField.Text.Length;
            textField.ScrollToCaret();

            //New text -> new message -> flash in taskbar
            if (!parent.ContainsFocus)
                WindowFlasher.flash(parent);
        }

        /// <summary>
        /// Says something as the console.
        /// </summary>
        /// <param name="info">The text to print.</param>
        public void sayConsole(string text)
        {
            //Append grey text
            AppendText(Color.FromArgb(85, 85, 85), FontStyle.Bold, text + "\r\n");
        }

        /// <summary>
        /// Says something as a user.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="color">The user's color.</param>
        /// <param name="text">The user's message.</param>
        public void sayUser(string name, Color color, string text)
        {
            //Append text /w colored username
            AppendText(color, FontStyle.Bold, name + ": ");
            AppendText(Color.Black, FontStyle.Regular, text + "\r\n");
        }

        /// <summary>
        /// Inserts an image into the textfield.
        /// </summary>
        /// <param name="image">The image to insert.</param>
        public void insertImage(Image image)
        {
            if (image != null)
            {
                //Doing things the easy way
                textField.ReadOnly = false;

                //Save what was on the clipboard and restore it after the paste
                object old = Clipboard.GetDataObject();
                Clipboard.SetImage(image);
                textField.Paste();
                Clipboard.SetDataObject(old);

                textField.ReadOnly = true;
            }
            else
                sayConsole("[Invalid image]");
            textField.AppendText("\r\n");
        }

        /// <summary>
        /// Clears the conversation field.
        /// </summary>
        public void clear()
        {
            textField.Clear();
        }

        private void textField_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            //Open URL
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
