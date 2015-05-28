namespace Server
{
    partial class ServerForm
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
            this.StartButton = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.CommandBox = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.IPAddressLabel = new System.Windows.Forms.Label();
            this.ConnectionsBox = new System.Windows.Forms.RichTextBox();
            this.ConnLabel = new System.Windows.Forms.Label();
            this.Time = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(12, 435);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(117, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.White;
            this.LogBox.Cursor = System.Windows.Forms.Cursors.No;
            this.LogBox.Location = new System.Drawing.Point(151, 7);
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(278, 417);
            this.LogBox.TabIndex = 1;
            this.LogBox.Text = "";
            this.LogBox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // CommandBox
            // 
            this.CommandBox.Location = new System.Drawing.Point(215, 437);
            this.CommandBox.Name = "CommandBox";
            this.CommandBox.Size = new System.Drawing.Size(214, 20);
            this.CommandBox.TabIndex = 2;
            this.CommandBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.CommandBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(12, 10);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(20, 13);
            this.IPLabel.TabIndex = 4;
            this.IPLabel.Text = "IP:";
            // 
            // IPAddressLabel
            // 
            this.IPAddressLabel.AutoSize = true;
            this.IPAddressLabel.Location = new System.Drawing.Point(32, 10);
            this.IPAddressLabel.Name = "IPAddressLabel";
            this.IPAddressLabel.Size = new System.Drawing.Size(0, 13);
            this.IPAddressLabel.TabIndex = 5;
            // 
            // ConnectionsBox
            // 
            this.ConnectionsBox.Cursor = System.Windows.Forms.Cursors.No;
            this.ConnectionsBox.Location = new System.Drawing.Point(12, 65);
            this.ConnectionsBox.Name = "ConnectionsBox";
            this.ConnectionsBox.ReadOnly = true;
            this.ConnectionsBox.Size = new System.Drawing.Size(117, 96);
            this.ConnectionsBox.TabIndex = 6;
            this.ConnectionsBox.Text = "";
            // 
            // ConnLabel
            // 
            this.ConnLabel.AutoSize = true;
            this.ConnLabel.Location = new System.Drawing.Point(38, 49);
            this.ConnLabel.Name = "ConnLabel";
            this.ConnLabel.Size = new System.Drawing.Size(65, 13);
            this.ConnLabel.TabIndex = 7;
            this.ConnLabel.Text = "-Connected-";
            // 
            // Time
            // 
            this.Time.AutoSize = true;
            this.Time.BackColor = System.Drawing.Color.White;
            this.Time.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Time.Location = new System.Drawing.Point(151, 440);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(32, 15);
            this.Time.TabIndex = 8;
            this.Time.Text = "Time";
            this.Time.Click += new System.EventHandler(this.Time_Click);
            // 
            // timer
            // 
            this.timer.Interval = 900;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 470);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.ConnLabel);
            this.Controls.Add(this.ConnectionsBox);
            this.Controls.Add(this.IPAddressLabel);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.CommandBox);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.StartButton);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Name = "ServerForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.TextBox CommandBox;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.Label IPAddressLabel;
        private System.Windows.Forms.RichTextBox ConnectionsBox;
        private System.Windows.Forms.Label ConnLabel;
        private System.Windows.Forms.Label Time;
        private System.Windows.Forms.Timer timer;
    }
}

