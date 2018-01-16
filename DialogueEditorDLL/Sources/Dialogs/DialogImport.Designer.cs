namespace DialogueEditor
{
    partial class DialogImport
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
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button button3;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Button button4;
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.checkedListBoxLanguages = new System.Windows.Forms.CheckedListBox();
            this.checkBoxImportWorkstring = new System.Windows.Forms.CheckBox();
            this.checkBoxImportLocalization = new System.Windows.Forms.CheckBox();
            this.checkedListBoxPackages = new System.Windows.Forms.CheckedListBox();
            this.checkBoxImportInformation = new System.Windows.Forms.CheckBox();
            this.textBoxPathConstants = new System.Windows.Forms.TextBox();
            this.checkBoxImportConstants = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 10);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(117, 13);
            label4.TabIndex = 14;
            label4.Text = "Dialogues Import Path :";
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button1.Location = new System.Drawing.Point(385, 24);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 13;
            button1.Text = "Edit...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.OnEditPathClick);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(205, 63);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 13);
            label2.TabIndex = 11;
            label2.Text = "Select languages :";
            // 
            // button3
            // 
            button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            button3.Location = new System.Drawing.Point(303, 371);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(75, 23);
            button3.TabIndex = 16;
            button3.Text = "Import";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(this.OnImportClick);
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button2.Location = new System.Drawing.Point(385, 371);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 15;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 63);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(93, 13);
            label1.TabIndex = 20;
            label1.Text = "Select packages :";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(12, 26);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(367, 20);
            this.textBoxPath.TabIndex = 12;
            // 
            // checkedListBoxLanguages
            // 
            this.checkedListBoxLanguages.CheckOnClick = true;
            this.checkedListBoxLanguages.FormattingEnabled = true;
            this.checkedListBoxLanguages.Location = new System.Drawing.Point(205, 79);
            this.checkedListBoxLanguages.Name = "checkedListBoxLanguages";
            this.checkedListBoxLanguages.Size = new System.Drawing.Size(143, 79);
            this.checkedListBoxLanguages.TabIndex = 10;
            // 
            // checkBoxImportWorkstring
            // 
            this.checkBoxImportWorkstring.AutoSize = true;
            this.checkBoxImportWorkstring.Location = new System.Drawing.Point(15, 198);
            this.checkBoxImportWorkstring.Name = "checkBoxImportWorkstring";
            this.checkBoxImportWorkstring.Size = new System.Drawing.Size(106, 17);
            this.checkBoxImportWorkstring.TabIndex = 17;
            this.checkBoxImportWorkstring.Text = "Import workstring";
            this.checkBoxImportWorkstring.UseVisualStyleBackColor = true;
            // 
            // checkBoxImportLocalization
            // 
            this.checkBoxImportLocalization.AutoSize = true;
            this.checkBoxImportLocalization.Location = new System.Drawing.Point(15, 175);
            this.checkBoxImportLocalization.Name = "checkBoxImportLocalization";
            this.checkBoxImportLocalization.Size = new System.Drawing.Size(115, 17);
            this.checkBoxImportLocalization.TabIndex = 18;
            this.checkBoxImportLocalization.Text = "Import localizations";
            this.checkBoxImportLocalization.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxPackages
            // 
            this.checkedListBoxPackages.CheckOnClick = true;
            this.checkedListBoxPackages.FormattingEnabled = true;
            this.checkedListBoxPackages.Location = new System.Drawing.Point(12, 79);
            this.checkedListBoxPackages.Name = "checkedListBoxPackages";
            this.checkedListBoxPackages.Size = new System.Drawing.Size(143, 79);
            this.checkedListBoxPackages.TabIndex = 19;
            // 
            // checkBoxImportInformation
            // 
            this.checkBoxImportInformation.AutoSize = true;
            this.checkBoxImportInformation.Location = new System.Drawing.Point(15, 221);
            this.checkBoxImportInformation.Name = "checkBoxImportInformation";
            this.checkBoxImportInformation.Size = new System.Drawing.Size(308, 17);
            this.checkBoxImportInformation.TabIndex = 21;
            this.checkBoxImportInformation.Text = "Import other data (Speaker, Context, SceneType, Voicing...)";
            this.checkBoxImportInformation.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 277);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(117, 13);
            label3.TabIndex = 24;
            label3.Text = "Constants Import Path :";
            // 
            // button4
            // 
            button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button4.Location = new System.Drawing.Point(385, 291);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(75, 23);
            button4.TabIndex = 23;
            button4.Text = "Edit...";
            button4.UseVisualStyleBackColor = true;
            button4.Click += new System.EventHandler(this.OnEditPathConstantsClick);
            // 
            // textBoxPathConstants
            // 
            this.textBoxPathConstants.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPathConstants.Location = new System.Drawing.Point(12, 293);
            this.textBoxPathConstants.Name = "textBoxPathConstants";
            this.textBoxPathConstants.Size = new System.Drawing.Size(367, 20);
            this.textBoxPathConstants.TabIndex = 22;
            // 
            // checkBoxImportConstants
            // 
            this.checkBoxImportConstants.AutoSize = true;
            this.checkBoxImportConstants.Location = new System.Drawing.Point(15, 319);
            this.checkBoxImportConstants.Name = "checkBoxImportConstants";
            this.checkBoxImportConstants.Size = new System.Drawing.Size(105, 17);
            this.checkBoxImportConstants.TabIndex = 25;
            this.checkBoxImportConstants.Text = "Import Constants";
            this.checkBoxImportConstants.UseVisualStyleBackColor = true;
            // 
            // DialogImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 406);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxImportConstants);
            this.Controls.Add(label3);
            this.Controls.Add(button4);
            this.Controls.Add(this.textBoxPathConstants);
            this.Controls.Add(this.checkBoxImportInformation);
            this.Controls.Add(label1);
            this.Controls.Add(this.checkedListBoxPackages);
            this.Controls.Add(this.checkBoxImportLocalization);
            this.Controls.Add(this.checkBoxImportWorkstring);
            this.Controls.Add(button3);
            this.Controls.Add(button2);
            this.Controls.Add(label4);
            this.Controls.Add(button1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(label2);
            this.Controls.Add(this.checkedListBoxLanguages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DialogImport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Dialogues";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.CheckedListBox checkedListBoxLanguages;
        private System.Windows.Forms.CheckBox checkBoxImportWorkstring;
        private System.Windows.Forms.CheckBox checkBoxImportLocalization;
        private System.Windows.Forms.CheckedListBox checkedListBoxPackages;
        private System.Windows.Forms.CheckBox checkBoxImportInformation;
        private System.Windows.Forms.TextBox textBoxPathConstants;
        private System.Windows.Forms.CheckBox checkBoxImportConstants;
    }
}