namespace DialogueEditor
{
    partial class DialogSearch
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
            this.checkedListBoxCommonProperties = new System.Windows.Forms.CheckedListBox();
            this.propertyGridCommon = new System.Windows.Forms.PropertyGrid();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxWorkstring = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxSentences = new System.Windows.Forms.CheckBox();
            this.checkBoxReplies = new System.Windows.Forms.CheckBox();
            this.checkBoxChoices = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkedListBoxCommonProperties
            // 
            this.checkedListBoxCommonProperties.FormattingEnabled = true;
            this.checkedListBoxCommonProperties.Location = new System.Drawing.Point(12, 12);
            this.checkedListBoxCommonProperties.Name = "checkedListBoxCommonProperties";
            this.checkedListBoxCommonProperties.Size = new System.Drawing.Size(194, 139);
            this.checkedListBoxCommonProperties.TabIndex = 0;
            this.checkedListBoxCommonProperties.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // propertyGridCommon
            // 
            this.propertyGridCommon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridCommon.HelpVisible = false;
            this.propertyGridCommon.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGridCommon.Location = new System.Drawing.Point(212, 12);
            this.propertyGridCommon.Name = "propertyGridCommon";
            this.propertyGridCommon.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGridCommon.Size = new System.Drawing.Size(264, 139);
            this.propertyGridCommon.TabIndex = 101;
            this.propertyGridCommon.ToolbarVisible = false;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(401, 267);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 102;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.SearchClick);
            // 
            // textBoxWorkstring
            // 
            this.textBoxWorkstring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWorkstring.Location = new System.Drawing.Point(74, 155);
            this.textBoxWorkstring.Name = "textBoxWorkstring";
            this.textBoxWorkstring.Size = new System.Drawing.Size(402, 20);
            this.textBoxWorkstring.TabIndex = 107;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 108;
            this.label1.Text = "Workstring :";
            // 
            // checkBoxSentences
            // 
            this.checkBoxSentences.AutoSize = true;
            this.checkBoxSentences.Checked = true;
            this.checkBoxSentences.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSentences.Location = new System.Drawing.Point(74, 181);
            this.checkBoxSentences.Name = "checkBoxSentences";
            this.checkBoxSentences.Size = new System.Drawing.Size(77, 17);
            this.checkBoxSentences.TabIndex = 111;
            this.checkBoxSentences.Text = "Sentences";
            this.checkBoxSentences.UseVisualStyleBackColor = true;
            // 
            // checkBoxReplies
            // 
            this.checkBoxReplies.AutoSize = true;
            this.checkBoxReplies.Location = new System.Drawing.Point(157, 181);
            this.checkBoxReplies.Name = "checkBoxReplies";
            this.checkBoxReplies.Size = new System.Drawing.Size(61, 17);
            this.checkBoxReplies.TabIndex = 112;
            this.checkBoxReplies.Text = "Replies";
            this.checkBoxReplies.UseVisualStyleBackColor = true;
            // 
            // checkBoxChoices
            // 
            this.checkBoxChoices.AutoSize = true;
            this.checkBoxChoices.Location = new System.Drawing.Point(224, 181);
            this.checkBoxChoices.Name = "checkBoxChoices";
            this.checkBoxChoices.Size = new System.Drawing.Size(64, 17);
            this.checkBoxChoices.TabIndex = 113;
            this.checkBoxChoices.Text = "Choices";
            this.checkBoxChoices.UseVisualStyleBackColor = true;
            // 
            // DialogSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(488, 302);
            this.Controls.Add(this.checkBoxChoices);
            this.Controls.Add(this.checkBoxReplies);
            this.Controls.Add(this.checkBoxSentences);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxWorkstring);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.propertyGridCommon);
            this.Controls.Add(this.checkedListBoxCommonProperties);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogSearch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxCommonProperties;
        private System.Windows.Forms.PropertyGrid propertyGridCommon;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxWorkstring;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSentences;
        private System.Windows.Forms.CheckBox checkBoxReplies;
        private System.Windows.Forms.CheckBox checkBoxChoices;
    }
}