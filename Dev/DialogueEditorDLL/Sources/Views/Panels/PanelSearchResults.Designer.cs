namespace DialogueEditor
{
    partial class PanelSearchResults
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
            this.labelResultsCount = new System.Windows.Forms.Label();
            buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            buttonClear.Location = new System.Drawing.Point(12, 2);
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
            // labelResultsCount
            // 
            this.labelResultsCount.AutoSize = true;
            this.labelResultsCount.Location = new System.Drawing.Point(105, 7);
            this.labelResultsCount.Name = "labelResultsCount";
            this.labelResultsCount.Size = new System.Drawing.Size(65, 13);
            this.labelResultsCount.TabIndex = 2;
            this.labelResultsCount.Text = "Results : {0}";
            // 
            // PanelSearchResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(584, 182);
            this.Controls.Add(this.labelResultsCount);
            this.Controls.Add(buttonClear);
            this.Controls.Add(this.listBoxLog);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "PanelSearchResults";
            this.Text = "Search Results";
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Label labelResultsCount;
    }
}