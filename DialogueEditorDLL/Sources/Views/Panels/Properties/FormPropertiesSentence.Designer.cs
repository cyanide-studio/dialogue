namespace DialogueEditor
{
    partial class FormPropertiesSentence
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
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button button9;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label11;
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.labelPostDelay = new System.Windows.Forms.Label();
            this.labelPreDelay = new System.Windows.Forms.Label();
            this.textBoxPostDelay = new System.Windows.Forms.TextBox();
            this.textBoxPreDelay = new System.Windows.Forms.TextBox();
            this.comboBoxIntensity = new System.Windows.Forms.ComboBox();
            this.textBoxComment = new System.Windows.Forms.RichTextBox();
            this.comboBoxAnimListener = new System.Windows.Forms.ComboBox();
            this.comboBoxAnimsetListener = new System.Windows.Forms.ComboBox();
            this.comboBoxAnimSpeaker = new System.Windows.Forms.ComboBox();
            this.comboBoxAnimsetSpeaker = new System.Windows.Forms.ComboBox();
            this.textBoxContext = new System.Windows.Forms.RichTextBox();
            this.labelWordCount = new System.Windows.Forms.Label();
            this.checkBoxHideSubtitle = new System.Windows.Forms.CheckBox();
            this.picturePortraitSpeaker = new System.Windows.Forms.PictureBox();
            this.textBoxWorkstring = new System.Windows.Forms.RichTextBox();
            this.comboBoxListener = new System.Windows.Forms.ComboBox();
            this.comboBoxSpeaker = new System.Windows.Forms.ComboBox();
            this.picturePortraitListener = new System.Windows.Forms.PictureBox();
            this.textBoxCameraBlendTime = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            button9 = new System.Windows.Forms.Button();
            label10 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            this.groupBoxProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePortraitSpeaker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePortraitListener)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(6, 56);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(50, 13);
            label5.TabIndex = 22;
            label5.Text = "Animset :";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(6, 158);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(50, 13);
            label8.TabIndex = 26;
            label8.Text = "Animset :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(6, 185);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(59, 13);
            label7.TabIndex = 27;
            label7.Text = "Animation :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 83);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(59, 13);
            label6.TabIndex = 23;
            label6.Text = "Animation :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 228);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(64, 13);
            label3.TabIndex = 15;
            label3.Text = "Workstring :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 131);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(50, 13);
            label2.TabIndex = 13;
            label2.Text = "Listener :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 29);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(53, 13);
            label1.TabIndex = 12;
            label1.Text = "Speaker :";
            // 
            // button9
            // 
            button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button9.FlatAppearance.BorderSize = 0;
            button9.Image = global::DialogueEditor.Properties.Resources.arrow_refresh_blue;
            button9.Location = new System.Drawing.Point(286, 112);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(24, 24);
            button9.TabIndex = 32;
            button9.Click += new System.EventHandler(this.OnSwapActors);
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(6, 374);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(48, 13);
            label10.TabIndex = 30;
            label10.Text = "Voicing :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 441);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(98, 13);
            label4.TabIndex = 19;
            label4.Text = "Context (directing) :";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(6, 304);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(49, 13);
            label9.TabIndex = 36;
            label9.Text = "Camera :";
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProperties.Controls.Add(label11);
            this.groupBoxProperties.Controls.Add(this.textBoxCameraBlendTime);
            this.groupBoxProperties.Controls.Add(this.comboBoxCamera);
            this.groupBoxProperties.Controls.Add(label9);
            this.groupBoxProperties.Controls.Add(this.labelPostDelay);
            this.groupBoxProperties.Controls.Add(this.labelPreDelay);
            this.groupBoxProperties.Controls.Add(this.textBoxPostDelay);
            this.groupBoxProperties.Controls.Add(this.textBoxPreDelay);
            this.groupBoxProperties.Controls.Add(button9);
            this.groupBoxProperties.Controls.Add(this.comboBoxIntensity);
            this.groupBoxProperties.Controls.Add(label10);
            this.groupBoxProperties.Controls.Add(this.textBoxComment);
            this.groupBoxProperties.Controls.Add(this.comboBoxAnimListener);
            this.groupBoxProperties.Controls.Add(label7);
            this.groupBoxProperties.Controls.Add(label8);
            this.groupBoxProperties.Controls.Add(this.comboBoxAnimsetListener);
            this.groupBoxProperties.Controls.Add(this.comboBoxAnimSpeaker);
            this.groupBoxProperties.Controls.Add(label6);
            this.groupBoxProperties.Controls.Add(label5);
            this.groupBoxProperties.Controls.Add(this.comboBoxAnimsetSpeaker);
            this.groupBoxProperties.Controls.Add(label4);
            this.groupBoxProperties.Controls.Add(this.textBoxContext);
            this.groupBoxProperties.Controls.Add(this.labelWordCount);
            this.groupBoxProperties.Controls.Add(this.checkBoxHideSubtitle);
            this.groupBoxProperties.Controls.Add(this.picturePortraitSpeaker);
            this.groupBoxProperties.Controls.Add(label3);
            this.groupBoxProperties.Controls.Add(this.textBoxWorkstring);
            this.groupBoxProperties.Controls.Add(this.comboBoxListener);
            this.groupBoxProperties.Controls.Add(this.comboBoxSpeaker);
            this.groupBoxProperties.Controls.Add(label2);
            this.groupBoxProperties.Controls.Add(this.picturePortraitListener);
            this.groupBoxProperties.Controls.Add(label1);
            this.groupBoxProperties.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(316, 577);
            this.groupBoxProperties.TabIndex = 16;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Sentence Properties";
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(60, 301);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(250, 21);
            this.comboBoxCamera.TabIndex = 37;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.OnCameraChanged);
            // 
            // labelPostDelay
            // 
            this.labelPostDelay.AutoSize = true;
            this.labelPostDelay.Location = new System.Drawing.Point(6, 543);
            this.labelPostDelay.Name = "labelPostDelay";
            this.labelPostDelay.Size = new System.Drawing.Size(62, 13);
            this.labelPostDelay.TabIndex = 35;
            this.labelPostDelay.Text = "Post delay :";
            // 
            // labelPreDelay
            // 
            this.labelPreDelay.AutoSize = true;
            this.labelPreDelay.Location = new System.Drawing.Point(6, 517);
            this.labelPreDelay.Name = "labelPreDelay";
            this.labelPreDelay.Size = new System.Drawing.Size(57, 13);
            this.labelPreDelay.TabIndex = 34;
            this.labelPreDelay.Text = "Pre delay :";
            // 
            // textBoxPostDelay
            // 
            this.textBoxPostDelay.Location = new System.Drawing.Point(74, 543);
            this.textBoxPostDelay.Name = "textBoxPostDelay";
            this.textBoxPostDelay.Size = new System.Drawing.Size(236, 20);
            this.textBoxPostDelay.TabIndex = 33;
            this.textBoxPostDelay.TextChanged += new System.EventHandler(this.OnPostDelayChanged);
            this.textBoxPostDelay.Validated += new System.EventHandler(this.OnDelayValidated);
            // 
            // textBoxPreDelay
            // 
            this.textBoxPreDelay.Location = new System.Drawing.Point(74, 514);
            this.textBoxPreDelay.Name = "textBoxPreDelay";
            this.textBoxPreDelay.Size = new System.Drawing.Size(236, 20);
            this.textBoxPreDelay.TabIndex = 17;
            this.textBoxPreDelay.TextChanged += new System.EventHandler(this.OnPreDelayChanged);
            this.textBoxPreDelay.Validated += new System.EventHandler(this.OnDelayValidated);
            // 
            // comboBoxIntensity
            // 
            this.comboBoxIntensity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIntensity.FormattingEnabled = true;
            this.comboBoxIntensity.Location = new System.Drawing.Point(60, 371);
            this.comboBoxIntensity.Name = "comboBoxIntensity";
            this.comboBoxIntensity.Size = new System.Drawing.Size(121, 21);
            this.comboBoxIntensity.TabIndex = 31;
            this.comboBoxIntensity.SelectedIndexChanged += new System.EventHandler(this.OnIntensityChanged);
            // 
            // textBoxComment
            // 
            this.textBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComment.Location = new System.Drawing.Point(6, 395);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(304, 35);
            this.textBoxComment.TabIndex = 29;
            this.textBoxComment.Text = "";
            this.textBoxComment.TextChanged += new System.EventHandler(this.OnCommentChanged);
            this.textBoxComment.Validated += new System.EventHandler(this.OnCommentValidated);
            // 
            // comboBoxAnimListener
            // 
            this.comboBoxAnimListener.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnimListener.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnimListener.FormattingEnabled = true;
            this.comboBoxAnimListener.Location = new System.Drawing.Point(71, 182);
            this.comboBoxAnimListener.Name = "comboBoxAnimListener";
            this.comboBoxAnimListener.Size = new System.Drawing.Size(137, 21);
            this.comboBoxAnimListener.TabIndex = 6;
            this.comboBoxAnimListener.SelectedIndexChanged += new System.EventHandler(this.OnAnimListenerChanged);
            // 
            // comboBoxAnimsetListener
            // 
            this.comboBoxAnimsetListener.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnimsetListener.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnimsetListener.FormattingEnabled = true;
            this.comboBoxAnimsetListener.Location = new System.Drawing.Point(71, 155);
            this.comboBoxAnimsetListener.Name = "comboBoxAnimsetListener";
            this.comboBoxAnimsetListener.Size = new System.Drawing.Size(137, 21);
            this.comboBoxAnimsetListener.TabIndex = 5;
            this.comboBoxAnimsetListener.SelectedIndexChanged += new System.EventHandler(this.OnMoodListenerChanged);
            // 
            // comboBoxAnimSpeaker
            // 
            this.comboBoxAnimSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnimSpeaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnimSpeaker.FormattingEnabled = true;
            this.comboBoxAnimSpeaker.Location = new System.Drawing.Point(71, 80);
            this.comboBoxAnimSpeaker.Name = "comboBoxAnimSpeaker";
            this.comboBoxAnimSpeaker.Size = new System.Drawing.Size(137, 21);
            this.comboBoxAnimSpeaker.TabIndex = 3;
            this.comboBoxAnimSpeaker.SelectedIndexChanged += new System.EventHandler(this.OnAnimSpeakerChanged);
            // 
            // comboBoxAnimsetSpeaker
            // 
            this.comboBoxAnimsetSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnimsetSpeaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnimsetSpeaker.FormattingEnabled = true;
            this.comboBoxAnimsetSpeaker.Location = new System.Drawing.Point(71, 53);
            this.comboBoxAnimsetSpeaker.Name = "comboBoxAnimsetSpeaker";
            this.comboBoxAnimsetSpeaker.Size = new System.Drawing.Size(137, 21);
            this.comboBoxAnimsetSpeaker.TabIndex = 2;
            this.comboBoxAnimsetSpeaker.SelectedIndexChanged += new System.EventHandler(this.OnMoodSpeakerChanged);
            // 
            // textBoxContext
            // 
            this.textBoxContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContext.Location = new System.Drawing.Point(6, 457);
            this.textBoxContext.Name = "textBoxContext";
            this.textBoxContext.Size = new System.Drawing.Size(304, 51);
            this.textBoxContext.TabIndex = 10;
            this.textBoxContext.Text = "";
            this.textBoxContext.TextChanged += new System.EventHandler(this.OnContextChanged);
            this.textBoxContext.Validated += new System.EventHandler(this.OnContextValidated);
            // 
            // labelWordCount
            // 
            this.labelWordCount.AutoSize = true;
            this.labelWordCount.Location = new System.Drawing.Point(76, 228);
            this.labelWordCount.Name = "labelWordCount";
            this.labelWordCount.Size = new System.Drawing.Size(58, 13);
            this.labelWordCount.TabIndex = 17;
            this.labelWordCount.Text = "(xxx count)";
            // 
            // checkBoxHideSubtitle
            // 
            this.checkBoxHideSubtitle.AutoSize = true;
            this.checkBoxHideSubtitle.Location = new System.Drawing.Point(149, 227);
            this.checkBoxHideSubtitle.Name = "checkBoxHideSubtitle";
            this.checkBoxHideSubtitle.Size = new System.Drawing.Size(86, 17);
            this.checkBoxHideSubtitle.TabIndex = 8;
            this.checkBoxHideSubtitle.Text = "Hide Subtitle";
            this.checkBoxHideSubtitle.UseVisualStyleBackColor = true;
            this.checkBoxHideSubtitle.CheckedChanged += new System.EventHandler(this.OnHideSubtitleChanged);
            // 
            // picturePortraitSpeaker
            // 
            this.picturePortraitSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picturePortraitSpeaker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePortraitSpeaker.Location = new System.Drawing.Point(214, 26);
            this.picturePortraitSpeaker.Name = "picturePortraitSpeaker";
            this.picturePortraitSpeaker.Size = new System.Drawing.Size(96, 96);
            this.picturePortraitSpeaker.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturePortraitSpeaker.TabIndex = 10;
            this.picturePortraitSpeaker.TabStop = false;
            // 
            // textBoxWorkstring
            // 
            this.textBoxWorkstring.AcceptsTab = true;
            this.textBoxWorkstring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWorkstring.Location = new System.Drawing.Point(6, 244);
            this.textBoxWorkstring.Name = "textBoxWorkstring";
            this.textBoxWorkstring.Size = new System.Drawing.Size(304, 51);
            this.textBoxWorkstring.TabIndex = 7;
            this.textBoxWorkstring.Text = "";
            this.textBoxWorkstring.TextChanged += new System.EventHandler(this.OnWorkstringChanged);
            this.textBoxWorkstring.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnWorkstringKeyDown);
            this.textBoxWorkstring.Validated += new System.EventHandler(this.OnWorkstringValidated);
            // 
            // comboBoxListener
            // 
            this.comboBoxListener.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxListener.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxListener.FormattingEnabled = true;
            this.comboBoxListener.Location = new System.Drawing.Point(71, 128);
            this.comboBoxListener.Name = "comboBoxListener";
            this.comboBoxListener.Size = new System.Drawing.Size(137, 21);
            this.comboBoxListener.TabIndex = 4;
            this.comboBoxListener.SelectedIndexChanged += new System.EventHandler(this.OnListenerChanged);
            // 
            // comboBoxSpeaker
            // 
            this.comboBoxSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSpeaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpeaker.FormattingEnabled = true;
            this.comboBoxSpeaker.Location = new System.Drawing.Point(71, 26);
            this.comboBoxSpeaker.Name = "comboBoxSpeaker";
            this.comboBoxSpeaker.Size = new System.Drawing.Size(137, 21);
            this.comboBoxSpeaker.TabIndex = 1;
            this.comboBoxSpeaker.SelectedIndexChanged += new System.EventHandler(this.OnSpeakerChanged);
            // 
            // picturePortraitListener
            // 
            this.picturePortraitListener.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picturePortraitListener.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePortraitListener.Location = new System.Drawing.Point(214, 128);
            this.picturePortraitListener.Name = "picturePortraitListener";
            this.picturePortraitListener.Size = new System.Drawing.Size(96, 96);
            this.picturePortraitListener.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturePortraitListener.TabIndex = 11;
            this.picturePortraitListener.TabStop = false;
            // 
            // textBoxCameraBlendTime
            // 
            this.textBoxCameraBlendTime.Location = new System.Drawing.Point(111, 328);
            this.textBoxCameraBlendTime.Name = "textBoxCameraBlendTime";
            this.textBoxCameraBlendTime.Size = new System.Drawing.Size(199, 20);
            this.textBoxCameraBlendTime.TabIndex = 38;
            this.textBoxCameraBlendTime.TextChanged += new System.EventHandler(this.OnCameraBlendTimeChanged);
            this.textBoxCameraBlendTime.Validated += new System.EventHandler(this.OnCameraBlendTimeValidated);
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(5, 331);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(100, 13);
            label11.TabIndex = 39;
            label11.Text = "Camera blend time :";
            // 
            // FormPropertiesSentence
            // 
            this.Controls.Add(this.groupBoxProperties);
            this.Name = "FormPropertiesSentence";
            this.Size = new System.Drawing.Size(322, 583);
            this.groupBoxProperties.ResumeLayout(false);
            this.groupBoxProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePortraitSpeaker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePortraitListener)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxListener;
        private System.Windows.Forms.PictureBox picturePortraitListener;
        private System.Windows.Forms.PictureBox picturePortraitSpeaker;
        private System.Windows.Forms.ComboBox comboBoxSpeaker;
        private System.Windows.Forms.RichTextBox textBoxWorkstring;
        private System.Windows.Forms.Label labelWordCount;
        private System.Windows.Forms.RichTextBox textBoxContext;
        private System.Windows.Forms.ComboBox comboBoxAnimSpeaker;
        private System.Windows.Forms.ComboBox comboBoxAnimsetSpeaker;
        private System.Windows.Forms.ComboBox comboBoxAnimListener;
        private System.Windows.Forms.ComboBox comboBoxAnimsetListener;
        private System.Windows.Forms.CheckBox checkBoxHideSubtitle;
        private System.Windows.Forms.RichTextBox textBoxComment;
        private System.Windows.Forms.ComboBox comboBoxIntensity;
        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.Label labelPostDelay;
        private System.Windows.Forms.Label labelPreDelay;
        private System.Windows.Forms.TextBox textBoxPostDelay;
        private System.Windows.Forms.TextBox textBoxPreDelay;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.TextBox textBoxCameraBlendTime;
    }
}