namespace DialogueEditor
{
    partial class FormPropertiesChoice
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
            System.Windows.Forms.GroupBox groupBox1;
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBlueprint = new System.Windows.Forms.ComboBox();
            this.checkBoxTimer = new System.Windows.Forms.CheckBox();
            this.textBoxTimer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxWorkstring = new System.Windows.Forms.RichTextBox();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 26);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(64, 13);
            label3.TabIndex = 19;
            label3.Text = "Workstring :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 112);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(121, 13);
            label1.TabIndex = 20;
            label1.Text = "(Will not appear ingame)";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.label4);
            groupBox1.Controls.Add(this.comboBoxBlueprint);
            groupBox1.Controls.Add(this.checkBoxTimer);
            groupBox1.Controls.Add(this.textBoxTimer);
            groupBox1.Controls.Add(this.label2);
            groupBox1.Controls.Add(this.textBoxWorkstring);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(321, 238);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Choice Properties";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Blueprint :";
            // 
            // comboBoxBlueprint
            // 
            this.comboBoxBlueprint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBlueprint.FormattingEnabled = true;
            this.comboBoxBlueprint.Location = new System.Drawing.Point(66, 179);
            this.comboBoxBlueprint.Name = "comboBoxBlueprint";
            this.comboBoxBlueprint.Size = new System.Drawing.Size(249, 21);
            this.comboBoxBlueprint.TabIndex = 24;
            this.comboBoxBlueprint.SelectedIndexChanged += new System.EventHandler(this.OnBlueprintChanged);
            // 
            // checkBoxTimer
            // 
            this.checkBoxTimer.AutoSize = true;
            this.checkBoxTimer.Location = new System.Drawing.Point(9, 155);
            this.checkBoxTimer.Name = "checkBoxTimer";
            this.checkBoxTimer.Size = new System.Drawing.Size(73, 17);
            this.checkBoxTimer.TabIndex = 23;
            this.checkBoxTimer.Text = "Hide timer";
            this.checkBoxTimer.UseVisualStyleBackColor = true;
            this.checkBoxTimer.CheckedChanged += new System.EventHandler(this.OnHideTimerChanged);
            // 
            // textBoxTimer
            // 
            this.textBoxTimer.Location = new System.Drawing.Point(51, 129);
            this.textBoxTimer.Name = "textBoxTimer";
            this.textBoxTimer.Size = new System.Drawing.Size(76, 20);
            this.textBoxTimer.TabIndex = 22;
            this.textBoxTimer.TextChanged += new System.EventHandler(this.OnTimerChanged);
            this.textBoxTimer.Validated += new System.EventHandler(this.OnTimerValidated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Timer :";
            // 
            // textBoxWorkstring
            // 
            this.textBoxWorkstring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWorkstring.Location = new System.Drawing.Point(6, 42);
            this.textBoxWorkstring.Name = "textBoxWorkstring";
            this.textBoxWorkstring.Size = new System.Drawing.Size(309, 67);
            this.textBoxWorkstring.TabIndex = 0;
            this.textBoxWorkstring.Text = "";
            this.textBoxWorkstring.TextChanged += new System.EventHandler(this.OnWorkstringChanged);
            this.textBoxWorkstring.Validated += new System.EventHandler(this.OnWorkstringValidated);
            // 
            // FormPropertiesChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(groupBox1);
            this.Name = "FormPropertiesChoice";
            this.Size = new System.Drawing.Size(327, 257);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxWorkstring;
        private System.Windows.Forms.TextBox textBoxTimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxTimer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxBlueprint;
    }
}
