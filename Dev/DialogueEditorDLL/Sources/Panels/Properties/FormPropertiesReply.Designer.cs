namespace DialogueEditor
{
    partial class FormPropertiesReply
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.labelWordCount = new System.Windows.Forms.Label();
            this.textBoxWorkstring = new System.Windows.Forms.RichTextBox();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.groupBoxProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 26);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(64, 13);
            label3.TabIndex = 17;
            label3.Text = "Workstring :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 112);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(98, 13);
            label1.TabIndex = 21;
            label1.Text = "(Will not be voiced)";
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProperties.Controls.Add(this.labelWordCount);
            this.groupBoxProperties.Controls.Add(this.textBoxWorkstring);
            this.groupBoxProperties.Controls.Add(label1);
            this.groupBoxProperties.Controls.Add(label3);
            this.groupBoxProperties.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(363, 139);
            this.groupBoxProperties.TabIndex = 22;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Reply Properties";
            // 
            // labelWordCount
            // 
            this.labelWordCount.AutoSize = true;
            this.labelWordCount.Location = new System.Drawing.Point(76, 26);
            this.labelWordCount.Name = "labelWordCount";
            this.labelWordCount.Size = new System.Drawing.Size(40, 13);
            this.labelWordCount.TabIndex = 22;
            this.labelWordCount.Text = "(count)";
            // 
            // textBoxWorkstring
            // 
            this.textBoxWorkstring.AcceptsTab = true;
            this.textBoxWorkstring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWorkstring.Location = new System.Drawing.Point(6, 42);
            this.textBoxWorkstring.Name = "textBoxWorkstring";
            this.textBoxWorkstring.Size = new System.Drawing.Size(351, 67);
            this.textBoxWorkstring.TabIndex = 0;
            this.textBoxWorkstring.Text = "";
            this.textBoxWorkstring.TextChanged += new System.EventHandler(this.OnWorkstringChanged);
            this.textBoxWorkstring.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnWorkstringKeyDown);
            this.textBoxWorkstring.Validated += new System.EventHandler(this.OnWorkstringValidated);
            // 
            // FormPropertiesReply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxProperties);
            this.Name = "FormPropertiesReply";
            this.Size = new System.Drawing.Size(369, 145);
            this.groupBoxProperties.ResumeLayout(false);
            this.groupBoxProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxWorkstring;
        private System.Windows.Forms.Label labelWordCount;
        private System.Windows.Forms.GroupBox groupBoxProperties;
    }
}
