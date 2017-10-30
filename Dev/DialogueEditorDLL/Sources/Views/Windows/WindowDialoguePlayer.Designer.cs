namespace DialogueEditor
{
    partial class WindowDialoguePlayer
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
            System.Windows.Forms.Button button2;
            System.Windows.Forms.Button button3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Button button4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowDialoguePlayer));
            this.comboBoxOptionConditions = new System.Windows.Forms.ComboBox();
            this.labelSentence = new System.Windows.Forms.Label();
            this.labelSpeaker = new System.Windows.Forms.Label();
            this.labelListener = new System.Windows.Forms.Label();
            this.labelChoice = new System.Windows.Forms.Label();
            this.listBoxReplies = new System.Windows.Forms.ListBox();
            this.radioButtonConditionsTrue = new System.Windows.Forms.RadioButton();
            this.radioButtonConditionsFalse = new System.Windows.Forms.RadioButton();
            this.groupBoxChoice = new System.Windows.Forms.GroupBox();
            this.labelRepliesConditions = new System.Windows.Forms.Label();
            this.radioButtonReplyConditionsTrue = new System.Windows.Forms.RadioButton();
            this.radioButtonReplyConditionsFalse = new System.Windows.Forms.RadioButton();
            this.checkBoxShowReplyConditions = new System.Windows.Forms.CheckBox();
            this.groupBoxConditions = new System.Windows.Forms.GroupBox();
            this.listBoxConditions = new System.Windows.Forms.ListBox();
            this.groupBoxGoto = new System.Windows.Forms.GroupBox();
            this.labelGoto = new System.Windows.Forms.Label();
            this.pictureBoxListener = new System.Windows.Forms.PictureBox();
            this.pictureBoxSpeaker = new System.Windows.Forms.PictureBox();
            this.checkBoxOptionConstants = new System.Windows.Forms.CheckBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            button4 = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            this.groupBoxChoice.SuspendLayout();
            this.groupBoxConditions.SuspendLayout();
            this.groupBoxGoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxListener)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpeaker)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            button1.Location = new System.Drawing.Point(12, 413);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 10;
            button1.Text = "Restart";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.OnRestart);
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            button2.Location = new System.Drawing.Point(352, 413);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "Next >";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.OnNextNode);
            // 
            // button3
            // 
            button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            button3.Location = new System.Drawing.Point(271, 413);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(75, 23);
            button3.TabIndex = 12;
            button3.Text = "< Back";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(this.OnPreviousNode);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(62, 13);
            label1.TabIndex = 12;
            label1.Text = "Conditions :";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.checkBoxOptionConstants);
            groupBox1.Controls.Add(this.comboBoxOptionConditions);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(586, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(209, 76);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Options";
            // 
            // comboBoxOptionConditions
            // 
            this.comboBoxOptionConditions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOptionConditions.FormattingEnabled = true;
            this.comboBoxOptionConditions.Location = new System.Drawing.Point(74, 22);
            this.comboBoxOptionConditions.Name = "comboBoxOptionConditions";
            this.comboBoxOptionConditions.Size = new System.Drawing.Size(129, 21);
            this.comboBoxOptionConditions.TabIndex = 13;
            this.comboBoxOptionConditions.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnComboBoxOptionConditionsFormat);
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(119, 18);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(236, 13);
            label3.TabIndex = 17;
            label3.Text = "(If conditions are False, the node will be skipped)";
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(93, 413);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(75, 23);
            button4.TabIndex = 11;
            button4.Text = "Edit Node";
            button4.UseVisualStyleBackColor = true;
            button4.Click += new System.EventHandler(this.OnEditNode);
            // 
            // labelSentence
            // 
            this.labelSentence.AutoEllipsis = true;
            this.labelSentence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSentence.Location = new System.Drawing.Point(12, 144);
            this.labelSentence.Name = "labelSentence";
            this.labelSentence.Padding = new System.Windows.Forms.Padding(3);
            this.labelSentence.Size = new System.Drawing.Size(415, 77);
            this.labelSentence.TabIndex = 4;
            // 
            // labelSpeaker
            // 
            this.labelSpeaker.Location = new System.Drawing.Point(12, 112);
            this.labelSpeaker.Name = "labelSpeaker";
            this.labelSpeaker.Size = new System.Drawing.Size(193, 23);
            this.labelSpeaker.TabIndex = 5;
            this.labelSpeaker.Text = "Speaker";
            this.labelSpeaker.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelListener
            // 
            this.labelListener.Location = new System.Drawing.Point(234, 112);
            this.labelListener.Name = "labelListener";
            this.labelListener.Size = new System.Drawing.Size(193, 23);
            this.labelListener.TabIndex = 6;
            this.labelListener.Text = "Listener";
            this.labelListener.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelChoice
            // 
            this.labelChoice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelChoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.labelChoice.Location = new System.Drawing.Point(6, 16);
            this.labelChoice.Name = "labelChoice";
            this.labelChoice.Size = new System.Drawing.Size(403, 16);
            this.labelChoice.TabIndex = 9;
            this.labelChoice.Text = "Choice >";
            this.labelChoice.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxReplies
            // 
            this.listBoxReplies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxReplies.FormattingEnabled = true;
            this.listBoxReplies.Location = new System.Drawing.Point(6, 35);
            this.listBoxReplies.Name = "listBoxReplies";
            this.listBoxReplies.Size = new System.Drawing.Size(404, 69);
            this.listBoxReplies.TabIndex = 10;
            // 
            // radioButtonConditionsTrue
            // 
            this.radioButtonConditionsTrue.AutoSize = true;
            this.radioButtonConditionsTrue.Location = new System.Drawing.Point(9, 16);
            this.radioButtonConditionsTrue.Name = "radioButtonConditionsTrue";
            this.radioButtonConditionsTrue.Size = new System.Drawing.Size(47, 17);
            this.radioButtonConditionsTrue.TabIndex = 14;
            this.radioButtonConditionsTrue.TabStop = true;
            this.radioButtonConditionsTrue.Text = "True";
            this.radioButtonConditionsTrue.UseVisualStyleBackColor = true;
            this.radioButtonConditionsTrue.CheckedChanged += new System.EventHandler(this.OnConditionsChecked);
            // 
            // radioButtonConditionsFalse
            // 
            this.radioButtonConditionsFalse.AutoSize = true;
            this.radioButtonConditionsFalse.Location = new System.Drawing.Point(62, 16);
            this.radioButtonConditionsFalse.Name = "radioButtonConditionsFalse";
            this.radioButtonConditionsFalse.Size = new System.Drawing.Size(50, 17);
            this.radioButtonConditionsFalse.TabIndex = 15;
            this.radioButtonConditionsFalse.TabStop = true;
            this.radioButtonConditionsFalse.Text = "False";
            this.radioButtonConditionsFalse.UseVisualStyleBackColor = true;
            // 
            // groupBoxChoice
            // 
            this.groupBoxChoice.Controls.Add(this.labelRepliesConditions);
            this.groupBoxChoice.Controls.Add(this.radioButtonReplyConditionsTrue);
            this.groupBoxChoice.Controls.Add(this.radioButtonReplyConditionsFalse);
            this.groupBoxChoice.Controls.Add(this.checkBoxShowReplyConditions);
            this.groupBoxChoice.Controls.Add(this.labelChoice);
            this.groupBoxChoice.Controls.Add(this.listBoxReplies);
            this.groupBoxChoice.Location = new System.Drawing.Point(12, 228);
            this.groupBoxChoice.Name = "groupBoxChoice";
            this.groupBoxChoice.Size = new System.Drawing.Size(415, 134);
            this.groupBoxChoice.TabIndex = 16;
            this.groupBoxChoice.TabStop = false;
            // 
            // labelRepliesConditions
            // 
            this.labelRepliesConditions.AutoSize = true;
            this.labelRepliesConditions.Location = new System.Drawing.Point(200, 111);
            this.labelRepliesConditions.Name = "labelRepliesConditions";
            this.labelRepliesConditions.Size = new System.Drawing.Size(100, 13);
            this.labelRepliesConditions.TabIndex = 18;
            this.labelRepliesConditions.Text = "Replies Conditions :";
            // 
            // radioButtonReplyConditionsTrue
            // 
            this.radioButtonReplyConditionsTrue.AutoSize = true;
            this.radioButtonReplyConditionsTrue.Location = new System.Drawing.Point(306, 109);
            this.radioButtonReplyConditionsTrue.Name = "radioButtonReplyConditionsTrue";
            this.radioButtonReplyConditionsTrue.Size = new System.Drawing.Size(47, 17);
            this.radioButtonReplyConditionsTrue.TabIndex = 16;
            this.radioButtonReplyConditionsTrue.TabStop = true;
            this.radioButtonReplyConditionsTrue.Text = "True";
            this.radioButtonReplyConditionsTrue.UseVisualStyleBackColor = true;
            this.radioButtonReplyConditionsTrue.CheckedChanged += new System.EventHandler(this.OnReplyConditionsChecked);
            // 
            // radioButtonReplyConditionsFalse
            // 
            this.radioButtonReplyConditionsFalse.AutoSize = true;
            this.radioButtonReplyConditionsFalse.Location = new System.Drawing.Point(359, 109);
            this.radioButtonReplyConditionsFalse.Name = "radioButtonReplyConditionsFalse";
            this.radioButtonReplyConditionsFalse.Size = new System.Drawing.Size(50, 17);
            this.radioButtonReplyConditionsFalse.TabIndex = 17;
            this.radioButtonReplyConditionsFalse.TabStop = true;
            this.radioButtonReplyConditionsFalse.Text = "False";
            this.radioButtonReplyConditionsFalse.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowReplyConditions
            // 
            this.checkBoxShowReplyConditions.AutoSize = true;
            this.checkBoxShowReplyConditions.Location = new System.Drawing.Point(6, 110);
            this.checkBoxShowReplyConditions.Name = "checkBoxShowReplyConditions";
            this.checkBoxShowReplyConditions.Size = new System.Drawing.Size(143, 17);
            this.checkBoxShowReplyConditions.TabIndex = 0;
            this.checkBoxShowReplyConditions.Text = "Show Replies Conditions";
            this.checkBoxShowReplyConditions.UseVisualStyleBackColor = true;
            this.checkBoxShowReplyConditions.CheckedChanged += new System.EventHandler(this.OnReplyConditionsChanged);
            // 
            // groupBoxConditions
            // 
            this.groupBoxConditions.Controls.Add(label3);
            this.groupBoxConditions.Controls.Add(this.listBoxConditions);
            this.groupBoxConditions.Controls.Add(this.radioButtonConditionsTrue);
            this.groupBoxConditions.Controls.Add(this.radioButtonConditionsFalse);
            this.groupBoxConditions.Location = new System.Drawing.Point(433, 228);
            this.groupBoxConditions.Name = "groupBoxConditions";
            this.groupBoxConditions.Size = new System.Drawing.Size(362, 98);
            this.groupBoxConditions.TabIndex = 17;
            this.groupBoxConditions.TabStop = false;
            this.groupBoxConditions.Text = "Node Conditions";
            // 
            // listBoxConditions
            // 
            this.listBoxConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxConditions.FormattingEnabled = true;
            this.listBoxConditions.Location = new System.Drawing.Point(6, 35);
            this.listBoxConditions.Name = "listBoxConditions";
            this.listBoxConditions.Size = new System.Drawing.Size(349, 56);
            this.listBoxConditions.TabIndex = 16;
            // 
            // groupBoxGoto
            // 
            this.groupBoxGoto.Controls.Add(this.labelGoto);
            this.groupBoxGoto.Location = new System.Drawing.Point(435, 332);
            this.groupBoxGoto.Name = "groupBoxGoto";
            this.groupBoxGoto.Size = new System.Drawing.Size(415, 54);
            this.groupBoxGoto.TabIndex = 17;
            this.groupBoxGoto.TabStop = false;
            // 
            // labelGoto
            // 
            this.labelGoto.AutoEllipsis = true;
            this.labelGoto.Location = new System.Drawing.Point(6, 16);
            this.labelGoto.Name = "labelGoto";
            this.labelGoto.Size = new System.Drawing.Size(403, 30);
            this.labelGoto.TabIndex = 19;
            this.labelGoto.Text = "Goto > ";
            // 
            // pictureBoxListener
            // 
            this.pictureBoxListener.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxListener.Location = new System.Drawing.Point(331, 12);
            this.pictureBoxListener.Name = "pictureBoxListener";
            this.pictureBoxListener.Size = new System.Drawing.Size(96, 96);
            this.pictureBoxListener.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxListener.TabIndex = 1;
            this.pictureBoxListener.TabStop = false;
            // 
            // pictureBoxSpeaker
            // 
            this.pictureBoxSpeaker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSpeaker.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxSpeaker.Name = "pictureBoxSpeaker";
            this.pictureBoxSpeaker.Size = new System.Drawing.Size(96, 96);
            this.pictureBoxSpeaker.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSpeaker.TabIndex = 0;
            this.pictureBoxSpeaker.TabStop = false;
            // 
            // checkBoxOptionConstants
            // 
            this.checkBoxOptionConstants.AutoSize = true;
            this.checkBoxOptionConstants.Location = new System.Drawing.Point(9, 49);
            this.checkBoxOptionConstants.Name = "checkBoxOptionConstants";
            this.checkBoxOptionConstants.Size = new System.Drawing.Size(73, 17);
            this.checkBoxOptionConstants.TabIndex = 14;
            this.checkBoxOptionConstants.Text = "Constants";
            this.checkBoxOptionConstants.UseVisualStyleBackColor = true;
            this.checkBoxOptionConstants.CheckedChanged += new System.EventHandler(this.OnOptionConstantsChanged);
            // 
            // WindowDialoguePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 448);
            this.Controls.Add(button4);
            this.Controls.Add(this.groupBoxConditions);
            this.Controls.Add(this.groupBoxGoto);
            this.Controls.Add(this.groupBoxChoice);
            this.Controls.Add(groupBox1);
            this.Controls.Add(button3);
            this.Controls.Add(button2);
            this.Controls.Add(button1);
            this.Controls.Add(this.labelListener);
            this.Controls.Add(this.labelSpeaker);
            this.Controls.Add(this.labelSentence);
            this.Controls.Add(this.pictureBoxListener);
            this.Controls.Add(this.pictureBoxSpeaker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindowDialoguePlayer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dialogue Player";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.groupBoxChoice.ResumeLayout(false);
            this.groupBoxChoice.PerformLayout();
            this.groupBoxConditions.ResumeLayout(false);
            this.groupBoxConditions.PerformLayout();
            this.groupBoxGoto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxListener)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpeaker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxSpeaker;
        private System.Windows.Forms.PictureBox pictureBoxListener;
        private System.Windows.Forms.Label labelSentence;
        private System.Windows.Forms.Label labelSpeaker;
        private System.Windows.Forms.Label labelListener;
        private System.Windows.Forms.Label labelChoice;
        private System.Windows.Forms.ComboBox comboBoxOptionConditions;
        private System.Windows.Forms.RadioButton radioButtonConditionsTrue;
        private System.Windows.Forms.CheckBox checkBoxShowReplyConditions;
        private System.Windows.Forms.GroupBox groupBoxChoice;
        private System.Windows.Forms.GroupBox groupBoxConditions;
        private System.Windows.Forms.GroupBox groupBoxGoto;
        private System.Windows.Forms.Label labelGoto;
        private System.Windows.Forms.Label labelRepliesConditions;
        private System.Windows.Forms.RadioButton radioButtonReplyConditionsTrue;
        private System.Windows.Forms.RadioButton radioButtonConditionsFalse;
        private System.Windows.Forms.RadioButton radioButtonReplyConditionsFalse;
        private System.Windows.Forms.ListBox listBoxReplies;
        private System.Windows.Forms.ListBox listBoxConditions;
        private System.Windows.Forms.CheckBox checkBoxOptionConstants;

    }
}