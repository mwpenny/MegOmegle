namespace MegOmegle
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            this.msgBox = new System.Windows.Forms.TextBox();
            this.sendBtnMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.stopBtnMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.convoField = new MegOmegle.ConvoField(this);
            this.sendBtn = new MegOmegle.DropDownButton();
            this.stopBtn = new MegOmegle.DropDownButton();
            this.interestsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // msgBox
            // 
            this.msgBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msgBox.Enabled = false;
            this.msgBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgBox.Location = new System.Drawing.Point(88, 464);
            this.msgBox.Multiline = true;
            this.msgBox.Name = "msgBox";
            this.msgBox.Size = new System.Drawing.Size(298, 65);
            this.msgBox.TabIndex = 3;
            this.msgBox.TextChanged += new System.EventHandler(this.msgBox_TextChanged);
            this.msgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msgBox_KeyDown);
            // 
            // sendBtnMenu
            // 
            this.sendBtnMenu.Name = "sendBtnMenu";
            this.sendBtnMenu.Size = new System.Drawing.Size(61, 4);
            this.sendBtnMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.btnMenu_ItemClicked);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // stopBtnMenu
            // 
            this.stopBtnMenu.Name = "sendBtnMenu";
            this.stopBtnMenu.Size = new System.Drawing.Size(61, 4);
            this.stopBtnMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.btnMenu_ItemClicked);
            // 
            // convoField
            // 
            this.convoField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.convoField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convoField.Location = new System.Drawing.Point(12, 8);
            this.convoField.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.convoField.Name = "convoField";
            this.convoField.Size = new System.Drawing.Size(450, 450);
            this.convoField.TabIndex = 4;
            // 
            // sendBtn
            // 
            this.sendBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendBtn.ButtonMenu = this.sendBtnMenu;
            this.sendBtn.ButtonText = "Send";
            this.sendBtn.Enabled = false;
            this.sendBtn.Location = new System.Drawing.Point(392, 464);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(70, 65);
            this.sendBtn.TabIndex = 4;
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopBtn.ArrowEnabled = true;
            this.stopBtn.ButtonMenu = this.stopBtnMenu;
            this.stopBtn.ButtonText = "New";
            this.stopBtn.Location = new System.Drawing.Point(12, 464);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(70, 32);
            this.stopBtn.TabIndex = 1;
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            this.stopBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.stopBtn_KeyDown);
            // 
            // interestsBtn
            // 
            this.interestsBtn.Location = new System.Drawing.Point(12, 497);
            this.interestsBtn.Name = "interestsBtn";
            this.interestsBtn.Size = new System.Drawing.Size(70, 32);
            this.interestsBtn.TabIndex = 2;
            this.interestsBtn.Text = "Interests";
            this.interestsBtn.UseVisualStyleBackColor = true;
            this.interestsBtn.Click += new System.EventHandler(this.interestsBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 538);
            this.Controls.Add(this.interestsBtn);
            this.Controls.Add(this.convoField);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.msgBox);
            this.Controls.Add(this.stopBtn);
            this.Name = "Main";
            this.Text = "MegOmegle";
            this.Load += new System.EventHandler(this.MegOmegle_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox msgBox;
        private DropDownButton sendBtn;
        private ConvoField convoField;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ContextMenuStrip sendBtnMenu;
        private DropDownButton stopBtn;
        private System.Windows.Forms.ContextMenuStrip stopBtnMenu;
        private System.Windows.Forms.Button interestsBtn;
    }
}

