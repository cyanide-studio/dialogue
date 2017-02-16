namespace DialogueEditor
{
    partial class OutputLog
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
            System.Windows.Forms.Button buttonClear;
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.checkBoxShowInfos = new System.Windows.Forms.CheckBox();
            this.checkBoxShowErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxShowWarnings = new System.Windows.Forms.CheckBox();
            buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            buttonClear.Location = new System.Drawing.Point(255, 4);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new System.Drawing.Size(75, 23);
            buttonClear.TabIndex = 1;
            buttonClear.Text = "Clear";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += new System.EventHandler(this.OnClearClicked);
            // 
            // listBoxLog
            // 
            this.listBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.IntegralHeight = false;
            this.listBoxLog.Location = new System.Drawing.Point(0, 31);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(584, 151);
            this.listBoxLog.TabIndex = 0;
            this.listBoxLog.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.listBoxLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnListKeyDown);
            // 
            // checkBoxShowInfos
            // 
            this.checkBoxShowInfos.AutoSize = true;
            this.checkBoxShowInfos.Location = new System.Drawing.Point(8, 8);
            this.checkBoxShowInfos.Name = "checkBoxShowInfos";
            this.checkBoxShowInfos.Size = new System.Drawing.Size(49, 17);
            this.checkBoxShowInfos.TabIndex = 2;
            this.checkBoxShowInfos.Text = "Infos";
            this.checkBoxShowInfos.UseVisualStyleBackColor = true;
            this.checkBoxShowInfos.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxShowErrors
            // 
            this.checkBoxShowErrors.AutoSize = true;
            this.checkBoxShowErrors.Location = new System.Drawing.Point(63, 8);
            this.checkBoxShowErrors.Name = "checkBoxShowErrors";
            this.checkBoxShowErrors.Size = new System.Drawing.Size(53, 17);
            this.checkBoxShowErrors.TabIndex = 3;
            this.checkBoxShowErrors.Text = "Errors";
            this.checkBoxShowErrors.UseVisualStyleBackColor = true;
            this.checkBoxShowErrors.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxShowWarnings
            // 
            this.checkBoxShowWarnings.AutoSize = true;
            this.checkBoxShowWarnings.Location = new System.Drawing.Point(122, 8);
            this.checkBoxShowWarnings.Name = "checkBoxShowWarnings";
            this.checkBoxShowWarnings.Size = new System.Drawing.Size(71, 17);
            this.checkBoxShowWarnings.TabIndex = 4;
            this.checkBoxShowWarnings.Text = "Warnings";
            this.checkBoxShowWarnings.UseVisualStyleBackColor = true;
            this.checkBoxShowWarnings.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // OutputLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(584, 182);
            this.Controls.Add(this.checkBoxShowWarnings);
            this.Controls.Add(this.checkBoxShowErrors);
            this.Controls.Add(this.checkBoxShowInfos);
            this.Controls.Add(buttonClear);
            this.Controls.Add(this.listBoxLog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "OutputLog";
            this.Text = "Output Log";
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.CheckBox checkBoxShowInfos;
        private System.Windows.Forms.CheckBox checkBoxShowErrors;
        private System.Windows.Forms.CheckBox checkBoxShowWarnings;
    }
}