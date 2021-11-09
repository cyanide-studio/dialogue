
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
            this.textBoxCameraBlendTime = new System.Windows.Forms.TextBox();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.labelPostDelay = new System.Windows.Forms.Label();
            this.labelPreDelay = new System.Windows.Forms.Label();
            this.textBoxPostDelay = new System.Windows.Forms.TextBox();
            this.textBoxPreDelay = new System.Windows.Forms.TextBox();
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
            this.groupBoxProperties.Controls.Add(label11);
            this.groupBoxProperties.Controls.Add(this.textBoxCameraBlendTime);
            this.groupBoxProperties.Controls.Add(this.comboBoxCamera);
            this.groupBoxProperties.Controls.Add(label9);
            this.groupBoxProperties.Controls.Add(this.labelPostDelay);
            this.groupBoxProperties.Controls.Add(this.labelPreDelay);
            this.groupBoxProperties.Controls.Add(this.textBoxPostDelay);
            this.groupBoxProperties.Controls.Add(this.textBoxPreDelay);
            this.groupBoxProperties.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProperties.Name = "groupBoxProperties";
            this.groupBoxProperties.Size = new System.Drawing.Size(316, 247);
            this.groupBoxProperties.TabIndex = 0;
            this.groupBoxProperties.TabStop = false;
            this.groupBoxProperties.Text = "Directing Properties";
            // 
            // textBoxCameraBlendTime
            // 
            this.textBoxCameraBlendTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCameraBlendTime.Location = new System.Drawing.Point(112, 56);
            this.textBoxCameraBlendTime.Name = "textBoxCameraBlendTime";
            this.textBoxCameraBlendTime.Size = new System.Drawing.Size(199, 20);
            this.textBoxCameraBlendTime.TabIndex = 42;
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
            // labelPostDelay
            // 
            this.labelPostDelay.AutoSize = true;
            this.labelPostDelay.Location = new System.Drawing.Point(6, 129);
            this.labelPostDelay.Name = "labelPostDelay";
            this.labelPostDelay.Size = new System.Drawing.Size(62, 13);
            this.labelPostDelay.TabIndex = 39;
            this.labelPostDelay.Text = "Post delay :";
            // 
            // labelPreDelay
            // 
            this.labelPreDelay.AutoSize = true;
            this.labelPreDelay.Location = new System.Drawing.Point(6, 103);
            this.labelPreDelay.Name = "labelPreDelay";
            this.labelPreDelay.Size = new System.Drawing.Size(57, 13);
            this.labelPreDelay.TabIndex = 38;
            this.labelPreDelay.Text = "Pre delay :";
            // 
            // textBoxPostDelay
            // 
            this.textBoxPostDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPostDelay.Location = new System.Drawing.Point(74, 129);
            this.textBoxPostDelay.Name = "textBoxPostDelay";
            this.textBoxPostDelay.Size = new System.Drawing.Size(236, 20);
            this.textBoxPostDelay.TabIndex = 44;
            this.textBoxPostDelay.Validated += new System.EventHandler(this.OnPostDelayValidated);
            // 
            // textBoxPreDelay
            // 
            this.textBoxPreDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPreDelay.Location = new System.Drawing.Point(74, 100);
            this.textBoxPreDelay.Name = "textBoxPreDelay";
            this.textBoxPreDelay.Size = new System.Drawing.Size(236, 20);
            this.textBoxPreDelay.TabIndex = 43;
            this.textBoxPreDelay.Validated += new System.EventHandler(this.OnPreDelayValidated);
            // 
            // FormPropertiesDirecting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxProperties);
            this.Name = "FormPropertiesDirecting";
            this.Size = new System.Drawing.Size(322, 311);
            this.groupBoxProperties.ResumeLayout(false);
            this.groupBoxProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProperties;
        private System.Windows.Forms.Label labelPostDelay;
        private System.Windows.Forms.Label labelPreDelay;
        private System.Windows.Forms.TextBox textBoxPostDelay;
        private System.Windows.Forms.TextBox textBoxPreDelay;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.TextBox textBoxCameraBlendTime;
    }
}
