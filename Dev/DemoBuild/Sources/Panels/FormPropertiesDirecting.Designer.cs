
namespace DemoBuild
{
    partial class FormPropertiesDirecting
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
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label11;
            this.groupBoxProperties = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxApplyOrbitalMove = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxApplyZoom = new System.Windows.Forms.ComboBox();
            this.textBoxCameraBlendTime = new System.Windows.Forms.TextBox();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.labelPreDelay = new System.Windows.Forms.Label();
            this.textBoxCameraDelay = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            this.groupBoxProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(6, 29);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(49, 13);
            label9.TabIndex = 40;
            label9.Text = "Camera :";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(6, 59);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(100, 13);
            label11.TabIndex = 43;
            label11.Text = "Camera blend time :";
            // 
            // groupBoxProperties
            // 
            this.groupBoxProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProperties.Controls.Add(this.label2);
            this.groupBoxProperties.Controls.Add(this.comboBoxApplyOrbitalMove);
            this.groupBoxProperties.Controls.Add(this.label1);
            this.groupBoxProperties.Controls.Add(this.comboBoxApplyZoom);
            this.groupBoxProperties.Controls.Add(label11);
            this.groupBoxProperties.Controls.Add(this.textBoxCameraBlendTime);
            this.groupBoxProperties.Controls.Add(this.comboBoxCamera);
            this.groupBoxProperties.Controls.Add(label9);
            this.groupBoxProperties.Controls.Add(this.labelPreDelay);
            this.groupBoxProperties.Controls.Add(this.textBoxCameraDelay);
            this.groupBoxProperties.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(316, 174);
            this.groupBoxProperties.TabIndex = 0;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Directing Properties";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Apply orbital move :";
            // 
            // comboBoxApplyOrbitalMove
            // 
            this.comboBoxApplyOrbitalMove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxApplyOrbitalMove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplyOrbitalMove.FormattingEnabled = true;
            this.comboBoxApplyOrbitalMove.Location = new System.Drawing.Point(113, 135);
            this.comboBoxApplyOrbitalMove.Name = "comboBoxApplyOrbitalMove";
            this.comboBoxApplyOrbitalMove.Size = new System.Drawing.Size(198, 21);
            this.comboBoxApplyOrbitalMove.TabIndex = 47;
            this.comboBoxApplyOrbitalMove.SelectedIndexChanged += new System.EventHandler(this.OnApplyOrbitalMoveChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Apply zoom :";
            // 
            // comboBoxApplyZoom
            // 
            this.comboBoxApplyZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxApplyZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplyZoom.FormattingEnabled = true;
            this.comboBoxApplyZoom.Location = new System.Drawing.Point(113, 108);
            this.comboBoxApplyZoom.Name = "comboBoxApplyZoom";
            this.comboBoxApplyZoom.Size = new System.Drawing.Size(198, 21);
            this.comboBoxApplyZoom.TabIndex = 45;
            this.comboBoxApplyZoom.SelectedIndexChanged += new System.EventHandler(this.OnApplyZoomChanged);
            // 
            // textBoxCameraBlendTime
            // 
            this.textBoxCameraBlendTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraBlendTime.Location = new System.Drawing.Point(112, 56);
            this.textBoxCameraBlendTime.Name = "textBoxCameraBlendTime";
            this.textBoxCameraBlendTime.Size = new System.Drawing.Size(199, 20);
            this.textBoxCameraBlendTime.TabIndex = 42;
            this.textBoxCameraBlendTime.TextChanged += new System.EventHandler(this.OnBlendTimeChanged);
            this.textBoxCameraBlendTime.Validated += new System.EventHandler(this.OnBlendTimeValidated);
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(60, 26);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(250, 21);
            this.comboBoxCamera.TabIndex = 41;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.OnCameraChanged);
            // 
            // labelPreDelay
            // 
            this.labelPreDelay.AutoSize = true;
            this.labelPreDelay.Location = new System.Drawing.Point(6, 85);
            this.labelPreDelay.Name = "labelPreDelay";
            this.labelPreDelay.Size = new System.Drawing.Size(77, 13);
            this.labelPreDelay.TabIndex = 38;
            this.labelPreDelay.Text = "Camera delay :";
            // 
            // textBoxCameraDelay
            // 
            this.textBoxCameraDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraDelay.Location = new System.Drawing.Point(113, 82);
            this.textBoxCameraDelay.Name = "textBoxCameraDelay";
            this.textBoxCameraDelay.Size = new System.Drawing.Size(198, 20);
            this.textBoxCameraDelay.TabIndex = 43;
            this.textBoxCameraDelay.TextChanged += new System.EventHandler(this.OnDelayChanged);
            this.textBoxCameraDelay.Validated += new System.EventHandler(this.OnDelayValidated);
            // 
            // FormPropertiesDirecting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxProperties);
            this.Name = "FormPropertiesDirecting";
            this.Size = new System.Drawing.Size(322, 180);
            this.groupBoxProperties.ResumeLayout(false);
            this.groupBoxProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.Label labelPreDelay;
        private System.Windows.Forms.TextBox textBoxCameraDelay;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.TextBox textBoxCameraBlendTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxApplyOrbitalMove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxApplyZoom;
    }
}
