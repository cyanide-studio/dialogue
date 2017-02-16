namespace DialogueEditor
{
    partial class FormPropertiesRoot
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label19;
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Button button14;
            System.Windows.Forms.Button button15;
            System.Windows.Forms.Button button16;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label8;
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxAdditionalActors = new System.Windows.Forms.ComboBox();
            this.listBoxAdditionalActors = new System.Windows.Forms.ListBox();
            this.textBoxContext = new System.Windows.Forms.RichTextBox();
            this.comboBoxPackage = new System.Windows.Forms.ComboBox();
            this.textBoxVoiceBank = new System.Windows.Forms.TextBox();
            this.comboBoxSceneType = new System.Windows.Forms.ComboBox();
            this.textBoxComment = new System.Windows.Forms.RichTextBox();
            this.textBoxCameraBlendTime = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label7 = new System.Windows.Forms.Label();
            label19 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button14 = new System.Windows.Forms.Button();
            button15 = new System.Windows.Forms.Button();
            button16 = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(7, 56);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 13);
            label2.TabIndex = 4;
            label2.Text = "Scene Type :";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(this.textBoxCameraBlendTime);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(this.comboBoxCamera);
            groupBox1.Controls.Add(this.label6);
            groupBox1.Controls.Add(label19);
            groupBox1.Controls.Add(this.comboBoxAdditionalActors);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(button14);
            groupBox1.Controls.Add(button15);
            groupBox1.Controls.Add(button16);
            groupBox1.Controls.Add(this.listBoxAdditionalActors);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(this.textBoxContext);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(this.comboBoxPackage);
            groupBox1.Controls.Add(this.textBoxVoiceBank);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(this.comboBoxSceneType);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(this.textBoxComment);
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(321, 514);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Dialogue Properties";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(7, 109);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(49, 13);
            label7.TabIndex = 50;
            label7.Text = "Camera :";
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(84, 106);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(231, 21);
            this.comboBoxCamera.TabIndex = 49;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.OnCameraChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 355);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 48;
            this.label6.Text = "Additional Actors :";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(148, 405);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(38, 13);
            label19.TabIndex = 47;
            label19.Text = "Actor :";
            // 
            // comboBoxAdditionalActors
            // 
            this.comboBoxAdditionalActors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAdditionalActors.FormattingEnabled = true;
            this.comboBoxAdditionalActors.Location = new System.Drawing.Point(148, 421);
            this.comboBoxAdditionalActors.Name = "comboBoxAdditionalActors";
            this.comboBoxAdditionalActors.Size = new System.Drawing.Size(163, 21);
            this.comboBoxAdditionalActors.TabIndex = 46;
            this.comboBoxAdditionalActors.SelectedIndexChanged += new System.EventHandler(this.OnAdditionalActorNameChanged);
            // 
            // button1
            // 
            button1.FlatAppearance.BorderSize = 0;
            button1.Image = global::DialogueEditor.Properties.Resources.add;
            button1.Location = new System.Drawing.Point(148, 371);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(24, 24);
            button1.TabIndex = 42;
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.OnAddAdditionalActor);
            // 
            // button14
            // 
            button14.FlatAppearance.BorderSize = 0;
            button14.Image = global::DialogueEditor.Properties.Resources.cross;
            button14.Location = new System.Drawing.Point(260, 371);
            button14.Name = "button14";
            button14.Size = new System.Drawing.Size(24, 24);
            button14.TabIndex = 45;
            button14.UseVisualStyleBackColor = true;
            button14.Click += new System.EventHandler(this.OnRemoveAdditionalActor);
            // 
            // button15
            // 
            button15.FlatAppearance.BorderSize = 0;
            button15.Image = global::DialogueEditor.Properties.Resources.arrow_down;
            button15.Location = new System.Drawing.Point(202, 371);
            button15.Name = "button15";
            button15.Size = new System.Drawing.Size(24, 24);
            button15.TabIndex = 44;
            button15.UseVisualStyleBackColor = true;
            button15.Click += new System.EventHandler(this.OnMoveAdditionalActorDown);
            // 
            // button16
            // 
            button16.FlatAppearance.BorderSize = 0;
            button16.Image = global::DialogueEditor.Properties.Resources.arrow_up;
            button16.Location = new System.Drawing.Point(178, 371);
            button16.Name = "button16";
            button16.Size = new System.Drawing.Size(24, 24);
            button16.TabIndex = 43;
            button16.UseVisualStyleBackColor = true;
            button16.Click += new System.EventHandler(this.OnMoveAdditionalActorUp);
            // 
            // listBoxAdditionalActors
            // 
            this.listBoxAdditionalActors.FormattingEnabled = true;
            this.listBoxAdditionalActors.Location = new System.Drawing.Point(6, 371);
            this.listBoxAdditionalActors.Name = "listBoxAdditionalActors";
            this.listBoxAdditionalActors.Size = new System.Drawing.Size(136, 82);
            this.listBoxAdditionalActors.TabIndex = 41;
            this.listBoxAdditionalActors.SelectedIndexChanged += new System.EventHandler(this.OnAdditionalActorsIndexChanged);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(7, 161);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(49, 13);
            label5.TabIndex = 8;
            label5.Text = "Context :";
            // 
            // textBoxContext
            // 
            this.textBoxContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContext.Location = new System.Drawing.Point(6, 177);
            this.textBoxContext.Name = "textBoxContext";
            this.textBoxContext.Size = new System.Drawing.Size(309, 74);
            this.textBoxContext.TabIndex = 9;
            this.textBoxContext.Text = "";
            this.textBoxContext.TextChanged += new System.EventHandler(this.OnContextChanged);
            this.textBoxContext.Validated += new System.EventHandler(this.OnContextValidated);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 29);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(56, 13);
            label4.TabIndex = 7;
            label4.Text = "Package :";
            // 
            // comboBoxPackage
            // 
            this.comboBoxPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPackage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPackage.FormattingEnabled = true;
            this.comboBoxPackage.Location = new System.Drawing.Point(84, 26);
            this.comboBoxPackage.Name = "comboBoxPackage";
            this.comboBoxPackage.Size = new System.Drawing.Size(231, 21);
            this.comboBoxPackage.TabIndex = 6;
            this.comboBoxPackage.SelectedIndexChanged += new System.EventHandler(this.OnPackageChanged);
            // 
            // textBoxVoiceBank
            // 
            this.textBoxVoiceBank.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVoiceBank.Location = new System.Drawing.Point(84, 80);
            this.textBoxVoiceBank.Name = "textBoxVoiceBank";
            this.textBoxVoiceBank.Size = new System.Drawing.Size(231, 20);
            this.textBoxVoiceBank.TabIndex = 1;
            this.textBoxVoiceBank.TextChanged += new System.EventHandler(this.OnVoiceBankChanged);
            this.textBoxVoiceBank.Validated += new System.EventHandler(this.OnVoiceBankValidated);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 83);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 13);
            label3.TabIndex = 5;
            label3.Text = "Voice Bank :";
            // 
            // comboBoxSceneType
            // 
            this.comboBoxSceneType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSceneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSceneType.FormattingEnabled = true;
            this.comboBoxSceneType.Location = new System.Drawing.Point(84, 53);
            this.comboBoxSceneType.Name = "comboBoxSceneType";
            this.comboBoxSceneType.Size = new System.Drawing.Size(231, 21);
            this.comboBoxSceneType.TabIndex = 0;
            this.comboBoxSceneType.SelectedIndexChanged += new System.EventHandler(this.OnSceneTypeChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 254);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(160, 13);
            label1.TabIndex = 2;
            label1.Text = "Comment (Will not be exported) :";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComment.Location = new System.Drawing.Point(6, 270);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(309, 74);
            this.textBoxComment.TabIndex = 2;
            this.textBoxComment.Text = "";
            this.textBoxComment.TextChanged += new System.EventHandler(this.OnCommentChanged);
            this.textBoxComment.Validated += new System.EventHandler(this.OnCommentValidated);
            // 
            // textBoxCameraBlendTime
            // 
            this.textBoxCameraBlendTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraBlendTime.Location = new System.Drawing.Point(113, 133);
            this.textBoxCameraBlendTime.Name = "textBoxCameraBlendTime";
            this.textBoxCameraBlendTime.Size = new System.Drawing.Size(202, 20);
            this.textBoxCameraBlendTime.TabIndex = 51;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(7, 136);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(100, 13);
            label8.TabIndex = 52;
            label8.Text = "Camera blend time :";
            // 
            // FormPropertiesRoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(groupBox1);
            this.Name = "FormPropertiesRoot";
            this.Size = new System.Drawing.Size(327, 520);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxComment;
        private System.Windows.Forms.ComboBox comboBoxSceneType;
        private System.Windows.Forms.TextBox textBoxVoiceBank;
        private System.Windows.Forms.ComboBox comboBoxPackage;
        private System.Windows.Forms.RichTextBox textBoxContext;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxAdditionalActors;
        private System.Windows.Forms.ListBox listBoxAdditionalActors;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.TextBox textBoxCameraBlendTime;
    }
}
