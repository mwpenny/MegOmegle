﻿/* MegOmegle.cs
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
        public void setStatus(string text) { statusLabel.Text = text; }
        public string getStatus() { return statusLabel.Text; }

        public ConvoField()
        {
            InitializeComponent();
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
        }

        /// <summary>
        /// Say something as the console.
        /// </summary>
        /// <param name="info"></param>
        public void sayConsole(string info)
        {
            //Append grey text
            AppendText(Color.FromArgb(85, 85, 85), FontStyle.Bold, info + "\r\n");
        }

        /// <summary>
        /// Say something as a user.
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
        /// Clears the conversation field.
        /// </summary>
        public void clear()
        {
            textField.Clear();
        }
    }
}