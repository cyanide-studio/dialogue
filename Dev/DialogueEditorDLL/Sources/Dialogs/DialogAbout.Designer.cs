namespace DialogueEditor
{
    partial class DialogAbout
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
            System.Windows.Forms.Button button1;
            System.Windows.Forms.PictureBox pictureBox1;
            System.Windows.Forms.Label label1;
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelVersionProject = new System.Windows.Forms.Label();
            this.labelVersionNet = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            button1.Location = new System.Drawing.Point(183, 159);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Close";
            button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = global::DialogueEditor.Properties.Resources.Doge;
            pictureBox1.Location = new System.Drawing.Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(32, 32);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(50, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(54, 26);
            label1.TabIndex = 3;
            label1.Text = "Author : \r\nLegulysse";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(9, 82);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(106, 13);
            this.labelVersion.TabIndex = 1;
            this.labelVersion.Text = "Core Editor Version : ";
            // 
            // labelVersionProject
            // 
            this.labelVersionProject.AutoSize = true;
            this.labelVersionProject.Location = new System.Drawing.Point(9, 60);
            this.labelVersionProject.Name = "labelVersionProject";
            this.labelVersionProject.Size = new System.Drawing.Size(87, 13);
            this.labelVersionProject.TabIndex = 4;
            this.labelVersionProject.Text = "Project Version : ";
            // 
            // labelVersionNet
            // 
            this.labelVersionNet.AutoSize = true;
            this.labelVersionNet.Location = new System.Drawing.Point(9, 116);
            this.labelVersionNet.Name = "labelVersionNet";
            this.labelVersionNet.Size = new System.Drawing.Size(74, 13);
            this.labelVersionNet.TabIndex = 5;
            this.labelVersionNet.Text = ".Net Version : ";
            // 
            // DialogAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 194);
            this.ControlBox = false;
            this.Controls.Add(this.labelVersionNet);
            this.Controls.Add(this.labelVersionProject);
            this.Controls.Add(label1);
            this.Controls.Add(pictureBox1);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DialogAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelVersionProject;
        private System.Windows.Forms.Label labelVersionNet;
    }
}