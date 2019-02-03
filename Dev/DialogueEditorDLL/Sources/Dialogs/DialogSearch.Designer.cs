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
            this.textBoxSentence = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxReply = new System.Windows.Forms.TextBox();
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
            this.propertyGridCommon.Size = new System.Drawing.Size(272, 139);
            this.propertyGridCommon.TabIndex = 101;
            this.propertyGridCommon.ToolbarVisible = false;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(13, 212);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 102;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.SearchClick);
            // 
            // textBoxSentence
            // 
            this.textBoxSentence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSentence.Location = new System.Drawing.Point(74, 155);
            this.textBoxSentence.Name = "textBoxSentence";
            this.textBoxSentence.Size = new System.Drawing.Size(410, 20);
            this.textBoxSentence.TabIndex = 107;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 108;
            this.label1.Text = "Sentence :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 109;
            this.label2.Text = "Reply :";
            // 
            // textBoxReply
            // 
            this.textBoxReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReply.Location = new System.Drawing.Point(74, 181);
            this.textBoxReply.Name = "textBoxReply";
            this.textBoxReply.Size = new System.Drawing.Size(410, 20);
            this.textBoxReply.TabIndex = 110;
            // 
            // DialogSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(496, 312);
            this.Controls.Add(this.textBoxReply);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSentence);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.propertyGridCommon);
            this.Controls.Add(this.checkedListBoxCommonProperties);
            this.Name = "DialogSearch";
            this.Text = "Search";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxCommonProperties;
        private System.Windows.Forms.PropertyGrid propertyGridCommon;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxSentence;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxReply;
    }
}