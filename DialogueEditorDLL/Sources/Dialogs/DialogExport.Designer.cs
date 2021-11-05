namespace DialogueEditor
{
    partial class DialogExport
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Button button3;
            this.checkedListBoxPackages = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxLanguages = new System.Windows.Forms.CheckedListBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.checkBoxDateDirectory = new System.Windows.Forms.CheckBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.checkBoxExportFromDate = new System.Windows.Forms.CheckBox();
            this.labelNoDateDirectory = new System.Windows.Forms.Label();
            this.checkBoxPackagesDirectory = new System.Windows.Forms.CheckBox();
            this.checkBoxWorkstringOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxWorkstringFallback = new System.Windows.Forms.CheckBox();
            this.buttonChooseDialogues = new System.Windows.Forms.Button();
            this.checkBoxCustomSelection = new System.Windows.Forms.CheckBox();
            this.checkBoxConstants = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(93, 13);
            label1.TabIndex = 2;
            label1.Text = "Select packages :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 121);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 13);
            label2.TabIndex = 4;
            label2.Text = "Select languages :";
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button1.Location = new System.Drawing.Point(364, 255);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "Edit...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.OnEditPathClick);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 241);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(35, 13);
            label4.TabIndex = 9;
            label4.Text = "Path :";
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button2.Location = new System.Drawing.Point(364, 414);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 10;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            button3.Location = new System.Drawing.Point(282, 414);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(75, 23);
            button3.TabIndex = 11;
            button3.Text = "Export";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(this.OnExportClick);
            // 
            // checkedListBoxPackages
            // 
            this.checkedListBoxPackages.CheckOnClick = true;
            this.checkedListBoxPackages.FormattingEnabled = true;
            this.checkedListBoxPackages.Location = new System.Drawing.Point(12, 25);
            this.checkedListBoxPackages.Name = "checkedListBoxPackages";
            this.checkedListBoxPackages.Size = new System.Drawing.Size(143, 79);
            this.checkedListBoxPackages.TabIndex = 1;
            // 
            // checkedListBoxLanguages
            // 
            this.checkedListBoxLanguages.CheckOnClick = true;
            this.checkedListBoxLanguages.FormattingEnabled = true;
            this.checkedListBoxLanguages.Location = new System.Drawing.Point(12, 137);
            this.checkedListBoxLanguages.Name = "checkedListBoxLanguages";
            this.checkedListBoxLanguages.Size = new System.Drawing.Size(143, 79);
            this.checkedListBoxLanguages.TabIndex = 3;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(12, 257);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(346, 20);
            this.textBoxPath.TabIndex = 5;
            // 
            // checkBoxDateDirectory
            // 
            this.checkBoxDateDirectory.AutoSize = true;
            this.checkBoxDateDirectory.Location = new System.Drawing.Point(15, 283);
            this.checkBoxDateDirectory.Name = "checkBoxDateDirectory";
            this.checkBoxDateDirectory.Size = new System.Drawing.Size(185, 17);
            this.checkBoxDateDirectory.TabIndex = 12;
            this.checkBoxDateDirectory.Text = "Create subfolder with current date";
            this.checkBoxDateDirectory.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(266, 362);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(173, 20);
            this.dateTimePicker.TabIndex = 13;
            this.dateTimePicker.Value = new System.DateTime(1753, 2, 1, 0, 0, 0, 0);
            // 
            // checkBoxExportFromDate
            // 
            this.checkBoxExportFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxExportFromDate.AutoSize = true;
            this.checkBoxExportFromDate.Location = new System.Drawing.Point(269, 344);
            this.checkBoxExportFromDate.Name = "checkBoxExportFromDate";
            this.checkBoxExportFromDate.Size = new System.Drawing.Size(103, 17);
            this.checkBoxExportFromDate.TabIndex = 14;
            this.checkBoxExportFromDate.Text = "Export from date";
            this.checkBoxExportFromDate.UseVisualStyleBackColor = true;
            this.checkBoxExportFromDate.CheckedChanged += new System.EventHandler(this.OnExportFromDateChanged);
            // 
            // labelNoDateDirectory
            // 
            this.labelNoDateDirectory.AutoSize = true;
            this.labelNoDateDirectory.Location = new System.Drawing.Point(206, 284);
            this.labelNoDateDirectory.Name = "labelNoDateDirectory";
            this.labelNoDateDirectory.Size = new System.Drawing.Size(144, 13);
            this.labelNoDateDirectory.TabIndex = 15;
            this.labelNoDateDirectory.Text = "(Existing files will be updated)";
            // 
            // checkBoxPackagesDirectory
            // 
            this.checkBoxPackagesDirectory.AutoSize = true;
            this.checkBoxPackagesDirectory.Location = new System.Drawing.Point(15, 302);
            this.checkBoxPackagesDirectory.Name = "checkBoxPackagesDirectory";
            this.checkBoxPackagesDirectory.Size = new System.Drawing.Size(180, 17);
            this.checkBoxPackagesDirectory.TabIndex = 16;
            this.checkBoxPackagesDirectory.Text = "Create subfolders with packages";
            this.checkBoxPackagesDirectory.UseVisualStyleBackColor = true;
            // 
            // checkBoxWorkstringOnly
            // 
            this.checkBoxWorkstringOnly.AutoSize = true;
            this.checkBoxWorkstringOnly.Location = new System.Drawing.Point(161, 137);
            this.checkBoxWorkstringOnly.Name = "checkBoxWorkstringOnly";
            this.checkBoxWorkstringOnly.Size = new System.Drawing.Size(99, 17);
            this.checkBoxWorkstringOnly.TabIndex = 17;
            this.checkBoxWorkstringOnly.Text = "Workstring only";
            this.checkBoxWorkstringOnly.UseVisualStyleBackColor = true;
            this.checkBoxWorkstringOnly.CheckedChanged += new System.EventHandler(this.OnExportWorkstringOnlyChanged);
            // 
            // checkBoxWorkstringFallback
            // 
            this.checkBoxWorkstringFallback.AutoSize = true;
            this.checkBoxWorkstringFallback.Location = new System.Drawing.Point(161, 160);
            this.checkBoxWorkstringFallback.Name = "checkBoxWorkstringFallback";
            this.checkBoxWorkstringFallback.Size = new System.Drawing.Size(219, 17);
            this.checkBoxWorkstringFallback.TabIndex = 18;
            this.checkBoxWorkstringFallback.Text = "Workstring fallback for empty translations";
            this.checkBoxWorkstringFallback.UseVisualStyleBackColor = true;
            // 
            // buttonChooseDialogues
            // 
            this.buttonChooseDialogues.Location = new System.Drawing.Point(161, 48);
            this.buttonChooseDialogues.Name = "buttonChooseDialogues";
            this.buttonChooseDialogues.Size = new System.Drawing.Size(99, 23);
            this.buttonChooseDialogues.TabIndex = 19;
            this.buttonChooseDialogues.Text = "Choose dialogues";
            this.buttonChooseDialogues.UseVisualStyleBackColor = true;
            this.buttonChooseDialogues.Click += new System.EventHandler(this.buttonChooseDialogues_Click);
            // 
            // checkBoxCustomSelection
            // 
            this.checkBoxCustomSelection.AutoSize = true;
            this.checkBoxCustomSelection.Location = new System.Drawing.Point(161, 25);
            this.checkBoxCustomSelection.Name = "checkBoxCustomSelection";
            this.checkBoxCustomSelection.Size = new System.Drawing.Size(108, 17);
            this.checkBoxCustomSelection.TabIndex = 20;
            this.checkBoxCustomSelection.Text = "Custom Selection";
            this.checkBoxCustomSelection.UseVisualStyleBackColor = true;
            this.checkBoxCustomSelection.CheckedChanged += new System.EventHandler(this.checkBoxCustomSelection_CheckedChanged);
            // 
            // checkBoxConstants
            // 
            this.checkBoxConstants.AutoSize = true;
            this.checkBoxConstants.Location = new System.Drawing.Point(161, 87);
            this.checkBoxConstants.Name = "checkBoxConstants";
            this.checkBoxConstants.Size = new System.Drawing.Size(139, 17);
            this.checkBoxConstants.TabIndex = 21;
            this.checkBoxConstants.Text = "Constants (separate file)";
            this.checkBoxConstants.UseVisualStyleBackColor = true;
            // 
            // DialogExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 449);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxConstants);
            this.Controls.Add(this.checkBoxCustomSelection);
            this.Controls.Add(this.buttonChooseDialogues);
            this.Controls.Add(this.checkBoxWorkstringFallback);
            this.Controls.Add(this.checkBoxWorkstringOnly);
            this.Controls.Add(this.checkBoxPackagesDirectory);
            this.Controls.Add(this.labelNoDateDirectory);
            this.Controls.Add(this.checkBoxExportFromDate);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.checkBoxDateDirectory);
            this.Controls.Add(button3);
            this.Controls.Add(button2);
            this.Controls.Add(label4);
            this.Controls.Add(button1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(label2);
            this.Controls.Add(this.checkedListBoxLanguages);
            this.Controls.Add(label1);
            this.Controls.Add(this.checkedListBoxPackages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DialogExport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Dialogues";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxPackages;
        private System.Windows.Forms.CheckedListBox checkedListBoxLanguages;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.CheckBox checkBoxDateDirectory;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.CheckBox checkBoxExportFromDate;
        private System.Windows.Forms.Label labelNoDateDirectory;
        private System.Windows.Forms.CheckBox checkBoxPackagesDirectory;
        private System.Windows.Forms.CheckBox checkBoxWorkstringOnly;
        private System.Windows.Forms.CheckBox checkBoxWorkstringFallback;
        private System.Windows.Forms.Button buttonChooseDialogues;
        private System.Windows.Forms.CheckBox checkBoxCustomSelection;
        private System.Windows.Forms.CheckBox checkBoxConstants;
    }
}