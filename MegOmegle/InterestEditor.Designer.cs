namespace MegOmegle
{
    partial class InterestEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.likeList = new System.Windows.Forms.ListBox();
            this.newLikeField = new System.Windows.Forms.TextBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // likeList
            // 
            this.likeList.FormattingEnabled = true;
            this.likeList.Location = new System.Drawing.Point(12, 39);
            this.likeList.Name = "likeList";
            this.likeList.Size = new System.Drawing.Size(160, 95);
            this.likeList.TabIndex = 1;
            this.likeList.SelectedIndexChanged += new System.EventHandler(this.likeList_SelectedIndexChanged);
            this.likeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.likeList_KeyDown);
            // 
            // newLikeField
            // 
            this.newLikeField.Location = new System.Drawing.Point(12, 12);
            this.newLikeField.Name = "newLikeField";
            this.newLikeField.Size = new System.Drawing.Size(160, 20);
            this.newLikeField.TabIndex = 0;
            this.newLikeField.TextChanged += new System.EventHandler(this.newLikeField_TextChanged);
            this.newLikeField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.newLikeField_KeyDown);
            // 
            // addBtn
            // 
            this.addBtn.Enabled = false;
            this.addBtn.Location = new System.Drawing.Point(179, 10);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(68, 23);
            this.addBtn.TabIndex = 1;
            this.addBtn.Text = "&Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Enabled = false;
            this.removeBtn.Location = new System.Drawing.Point(179, 39);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(68, 23);
            this.removeBtn.TabIndex = 2;
            this.removeBtn.Text = "&Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // InterestEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 146);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.newLikeField);
            this.Controls.Add(this.likeList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "InterestEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit interests";
            this.Shown += new System.EventHandler(this.InterestEditor_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InterestEditor_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox likeList;
        private System.Windows.Forms.TextBox newLikeField;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button removeBtn;
    }
}