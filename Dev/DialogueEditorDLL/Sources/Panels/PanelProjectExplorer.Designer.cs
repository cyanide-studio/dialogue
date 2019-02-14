namespace DialogueEditor
{
    partial class PanelProjectExplorer
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.ToolStripMenuItem menuItemOpenDirectory;
            System.Windows.Forms.ToolStripMenuItem menuItemCopyName;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripMenuItem menuItemDelete;
            this.tree = new System.Windows.Forms.TreeView();
            this.textBoxSearchName = new System.Windows.Forms.TextBox();
            this.listBoxSearchResults = new System.Windows.Forms.ListBox();
            this.menuFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newDialogueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportStatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPackage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newDialogueToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showStatsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxSearchSceneType = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchActor = new System.Windows.Forms.ComboBox();
            this.showStatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDocument = new System.Windows.Forms.ContextMenuStrip(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            menuItemOpenDirectory = new System.Windows.Forms.ToolStripMenuItem();
            menuItemCopyName = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolder.SuspendLayout();
            this.menuPackage.SuspendLayout();
            this.menuDocument.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(2, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 13);
            label1.TabIndex = 3;
            label1.Text = "Search File :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(2, 35);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(71, 13);
            label2.TabIndex = 6;
            label2.Text = "Scene Type :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(2, 62);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(38, 13);
            label3.TabIndex = 8;
            label3.Text = "Actor :";
            // 
            // menuItemOpenDirectory
            // 
            menuItemOpenDirectory.Name = "menuItemOpenDirectory";
            menuItemOpenDirectory.Size = new System.Drawing.Size(154, 22);
            menuItemOpenDirectory.Text = "Open Directory";
            menuItemOpenDirectory.Click += new System.EventHandler(this.OnDocumentOpenDirectory);
            // 
            // menuItemCopyName
            // 
            menuItemCopyName.Name = "menuItemCopyName";
            menuItemCopyName.Size = new System.Drawing.Size(154, 22);
            menuItemCopyName.Text = "Copy Name";
            menuItemCopyName.Click += new System.EventHandler(this.OnDocumentCopyName);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // menuItemDelete
            // 
            menuItemDelete.Name = "menuItemDelete";
            menuItemDelete.Size = new System.Drawing.Size(154, 22);
            menuItemDelete.Text = "Delete";
            menuItemDelete.Click += new System.EventHandler(this.OnDocumentDelete);
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.Location = new System.Drawing.Point(-1, 86);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(231, 330);
            this.tree.TabIndex = 0;
            this.tree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnNodeCollapse);
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeDoubleClick);
            // 
            // textBoxSearchName
            // 
            this.textBoxSearchName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchName.Location = new System.Drawing.Point(79, 6);
            this.textBoxSearchName.Name = "textBoxSearchName";
            this.textBoxSearchName.Size = new System.Drawing.Size(144, 20);
            this.textBoxSearchName.TabIndex = 1;
            this.textBoxSearchName.TextChanged += new System.EventHandler(this.OnTextSearchChanged);
            // 
            // listBoxSearchResults
            // 
            this.listBoxSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSearchResults.FormattingEnabled = true;
            this.listBoxSearchResults.IntegralHeight = false;
            this.listBoxSearchResults.Location = new System.Drawing.Point(61, 196);
            this.listBoxSearchResults.Name = "listBoxSearchResults";
            this.listBoxSearchResults.Size = new System.Drawing.Size(109, 69);
            this.listBoxSearchResults.TabIndex = 2;
            this.listBoxSearchResults.Visible = false;
            this.listBoxSearchResults.DoubleClick += new System.EventHandler(this.OnSearchResultDoubleClick);
            // 
            // menuFolder
            // 
            this.menuFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDialogueToolStripMenuItem,
            this.toolStripSeparator3,
            this.openDirectoryToolStripMenuItem,
            this.exportStatsToolStripMenuItem,
            this.toolStripSeparator6,
            this.expandAllToolStripMenuItem1,
            this.collapseAllToolStripMenuItem1});
            this.menuFolder.Name = "menuFolder";
            this.menuFolder.Size = new System.Drawing.Size(155, 126);
            // 
            // newDialogueToolStripMenuItem
            // 
            this.newDialogueToolStripMenuItem.Name = "newDialogueToolStripMenuItem";
            this.newDialogueToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.newDialogueToolStripMenuItem.Text = "New Dialogue";
            this.newDialogueToolStripMenuItem.Click += new System.EventHandler(this.OnFolderNewDialogue);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
            // 
            // openDirectoryToolStripMenuItem
            // 
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.openDirectoryToolStripMenuItem.Text = "Open Directory";
            this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OnFolderOpenDirectory);
            // 
            // exportStatsToolStripMenuItem
            // 
            this.exportStatsToolStripMenuItem.Name = "exportStatsToolStripMenuItem";
            this.exportStatsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exportStatsToolStripMenuItem.Text = "Show Stats";
            this.exportStatsToolStripMenuItem.Click += new System.EventHandler(this.OnFolderShowStats);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(151, 6);
            // 
            // expandAllToolStripMenuItem1
            // 
            this.expandAllToolStripMenuItem1.Name = "expandAllToolStripMenuItem1";
            this.expandAllToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.expandAllToolStripMenuItem1.Text = "Expand All";
            this.expandAllToolStripMenuItem1.Click += new System.EventHandler(this.OnExpandAll);
            // 
            // collapseAllToolStripMenuItem1
            // 
            this.collapseAllToolStripMenuItem1.Name = "collapseAllToolStripMenuItem1";
            this.collapseAllToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.collapseAllToolStripMenuItem1.Text = "Collapse All";
            this.collapseAllToolStripMenuItem1.Click += new System.EventHandler(this.OnCollapseAll);
            // 
            // menuPackage
            // 
            this.menuPackage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDialogueToolStripMenuItem1,
            this.toolStripSeparator2,
            this.showStatsToolStripMenuItem1,
            this.toolStripSeparator5,
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem});
            this.menuPackage.Name = "menuPackage";
            this.menuPackage.Size = new System.Drawing.Size(149, 104);
            // 
            // newDialogueToolStripMenuItem1
            // 
            this.newDialogueToolStripMenuItem1.Name = "newDialogueToolStripMenuItem1";
            this.newDialogueToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.newDialogueToolStripMenuItem1.Text = "New Dialogue";
            this.newDialogueToolStripMenuItem1.Click += new System.EventHandler(this.OnPackageNewDialogue);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // showStatsToolStripMenuItem1
            // 
            this.showStatsToolStripMenuItem1.Name = "showStatsToolStripMenuItem1";
            this.showStatsToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.showStatsToolStripMenuItem1.Text = "Show Stats";
            this.showStatsToolStripMenuItem1.Click += new System.EventHandler(this.OnPackageShowStats);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(145, 6);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.OnExpandAll);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.OnCollapseAll);
            // 
            // comboBoxSearchSceneType
            // 
            this.comboBoxSearchSceneType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchSceneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchSceneType.FormattingEnabled = true;
            this.comboBoxSearchSceneType.Location = new System.Drawing.Point(79, 32);
            this.comboBoxSearchSceneType.Name = "comboBoxSearchSceneType";
            this.comboBoxSearchSceneType.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSearchSceneType.TabIndex = 5;
            this.comboBoxSearchSceneType.SelectedIndexChanged += new System.EventHandler(this.OnSearchSceneTypeChanged);
            // 
            // comboBoxSearchActor
            // 
            this.comboBoxSearchActor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchActor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchActor.FormattingEnabled = true;
            this.comboBoxSearchActor.Location = new System.Drawing.Point(79, 59);
            this.comboBoxSearchActor.Name = "comboBoxSearchActor";
            this.comboBoxSearchActor.Size = new System.Drawing.Size(144, 21);
            this.comboBoxSearchActor.TabIndex = 7;
            this.comboBoxSearchActor.SelectedIndexChanged += new System.EventHandler(this.OnSearchActorChanged);
            // 
            // showStatsToolStripMenuItem
            // 
            this.showStatsToolStripMenuItem.Name = "showStatsToolStripMenuItem";
            this.showStatsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.showStatsToolStripMenuItem.Text = "Show Stats";
            this.showStatsToolStripMenuItem.Click += new System.EventHandler(this.OnDocumentShowStats);
            // 
            // menuDocument
            // 
            this.menuDocument.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuItemOpenDirectory,
            menuItemCopyName,
            this.showStatsToolStripMenuItem,
            toolStripSeparator1,
            menuItemDelete});
            this.menuDocument.Name = "m_pContextMenuDocument";
            this.menuDocument.Size = new System.Drawing.Size(155, 98);
            // 
            // PanelProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 415);
            this.Controls.Add(label3);
            this.Controls.Add(this.comboBoxSearchActor);
            this.Controls.Add(label2);
            this.Controls.Add(this.comboBoxSearchSceneType);
            this.Controls.Add(label1);
            this.Controls.Add(this.listBoxSearchResults);
            this.Controls.Add(this.textBoxSearchName);
            this.Controls.Add(this.tree);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "PanelProjectExplorer";
            this.Text = "Project Explorer";
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.menuFolder.ResumeLayout(false);
            this.menuPackage.ResumeLayout(false);
            this.menuDocument.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.TextBox textBoxSearchName;
        private System.Windows.Forms.ListBox listBoxSearchResults;
        private System.Windows.Forms.ContextMenuStrip menuFolder;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportStatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ContextMenuStrip menuPackage;
        private System.Windows.Forms.ToolStripMenuItem showStatsToolStripMenuItem1;
        private System.Windows.Forms.ComboBox comboBoxSearchSceneType;
        private System.Windows.Forms.ComboBox comboBoxSearchActor;
        private System.Windows.Forms.ToolStripMenuItem newDialogueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDialogueToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem showStatsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuDocument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem1;
    }
}