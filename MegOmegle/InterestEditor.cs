/* InterestEditor.cs
 * 
 * Allows editing of common interests for use when finding partners.
 * 
 */

using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace MegOmegle
{
    public partial class InterestEditor : Form
    {
        private BindingList<string> likes; //Reference to shared interest list

        public InterestEditor(BindingList<string> likes)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
            this.likes = likes;
            likeList.DataSource = this.likes;
        }

        private void InterestEditor_Shown(object sender, EventArgs e)
        {
            newLikeField.Select();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(newLikeField.Text))
            {
                //Don't allow duplicate interests
                int index = likes.IndexOf(newLikeField.Text.ToLower());
                if (index == -1)
                {
                    //Add new interest
                    likes.Add(newLikeField.Text.ToLower());
                    index = likes.Count - 1;
                }

                //WTF, .NET!? SelectedIndexChanged doesn't fire for 0 the first time...
                likeList.SelectedIndex = -1;
                likeList.SelectedIndex = index;
                newLikeField.Clear();
            }
        }

        private void newLikeField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addBtn.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void newLikeField_TextChanged(object sender, EventArgs e)
        {
            addBtn.Enabled = !String.IsNullOrEmpty(((TextBox)sender).Text);
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            //Remove selected item and update index
            int index = likeList.SelectedIndex;
            likes.RemoveAt(index);
            likeList.SelectedIndex = (index == 0 && likeList.Items.Count > 0) ? index : index - 1;
        }

        private void likeList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeBtn.PerformClick();
        }

        private void likeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update remove button clickability
            ListBox lv = (ListBox)sender;
            removeBtn.Enabled = (lv.SelectedIndex >= 0 && lv.SelectedIndex < lv.Items.Count);
        }

        private void InterestEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Hide();
        }
    }
}
