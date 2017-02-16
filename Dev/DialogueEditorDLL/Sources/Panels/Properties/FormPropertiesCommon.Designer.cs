namespace DialogueEditor
{
    partial class FormPropertiesCommon
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox1;
            this.treeAttributes = new System.Windows.Forms.TreeView();
            this.propertyGridAttributes = new System.Windows.Forms.PropertyGrid();
            this.menuAttributes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddCondition = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAddAction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAddFlag = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator = new System.Windows.Forms.ToolStripSeparator();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            this.menuAttributes.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.treeAttributes);
            groupBox1.Controls.Add(this.propertyGridAttributes);
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(317, 241);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Common Properties";
            // 
            // treeAttributes
            // 
            this.treeAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeAttributes.Location = new System.Drawing.Point(6, 19);
            this.treeAttributes.Name = "treeAttributes";
            this.treeAttributes.ShowRootLines = false;
            this.treeAttributes.Size = new System.Drawing.Size(305, 105);
            this.treeAttributes.TabIndex = 100;
            this.treeAttributes.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnNodeAttributeCollapse);
            this.treeAttributes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeAttributeClicked);
            this.treeAttributes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnAttributesKeyDown);
            // 
            // propertyGridAttributes
            // 
            this.propertyGridAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridAttributes.HelpVisible = false;
            this.propertyGridAttributes.Location = new System.Drawing.Point(6, 130);
            this.propertyGridAttributes.Name = "propertyGridAttributes";
            this.propertyGridAttributes.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGridAttributes.Size = new System.Drawing.Size(305, 105);
            this.propertyGridAttributes.TabIndex = 101;
            this.propertyGridAttributes.ToolbarVisible = false;
            this.propertyGridAttributes.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnPropertyGridAttributesChanged);
            // 
            // menuAttributes
            // 
            this.menuAttributes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddCondition,
            this.menuItemAddAction,
            this.menuItemAddFlag,
            this.menuItemSeparator,
            this.menuItemDelete});
            this.menuAttributes.Name = "m_pContextMenu";
            this.menuAttributes.Size = new System.Drawing.Size(153, 120);
            this.menuAttributes.Opened += new System.EventHandler(this.OnContextMenuOpened);
            // 
            // menuItemAddCondition
            // 
            this.menuItemAddCondition.Name = "menuItemAddCondition";
            this.menuItemAddCondition.Size = new System.Drawing.Size(152, 22);
            this.menuItemAddCondition.Text = "Add";
            // 
            // menuItemAddAction
            // 
            this.menuItemAddAction.Name = "menuItemAddAction";
            this.menuItemAddAction.Size = new System.Drawing.Size(152, 22);
            this.menuItemAddAction.Text = "Add";
            // 
            // menuItemAddFlag
            // 
            this.menuItemAddFlag.Name = "menuItemAddFlag";
            this.menuItemAddFlag.Size = new System.Drawing.Size(152, 22);
            this.menuItemAddFlag.Text = "Add";
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Name = "menuItemDelete";
            this.menuItemDelete.Size = new System.Drawing.Size(152, 22);
            this.menuItemDelete.Text = "Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.OnDeleteTreeItem);
            // 
            // menuItemSeparator
            // 
            this.menuItemSeparator.Name = "menuItemSeparator";
            this.menuItemSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // FormPropertiesCommon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(groupBox1);
            this.Name = "FormPropertiesCommon";
            this.Size = new System.Drawing.Size(323, 247);
            groupBox1.ResumeLayout(false);
            this.menuAttributes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeAttributes;
        private System.Windows.Forms.PropertyGrid propertyGridAttributes;
        private System.Windows.Forms.ContextMenuStrip menuAttributes;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddCondition;
        private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddAction;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddFlag;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator;

    }
}
