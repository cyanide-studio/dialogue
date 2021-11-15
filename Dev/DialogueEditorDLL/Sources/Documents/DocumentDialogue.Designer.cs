namespace DialogueEditor
{
    partial class DocumentDialogue
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
            System.Windows.Forms.ToolStripMenuItem addSentenceToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addChoiceToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addGotoToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addBranchToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentDialogue));
            this.tree = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemOpenDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyName = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorRoot = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAddReply = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorReply = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorBranch = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAddSentence = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAddChoice = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAddGoto = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAddBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorReference = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCopyReference = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPasteReference = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyID = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorMove = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemMoveNodeUp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMoveNodeDown = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxDisplaySpeaker = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayListener = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayID = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayConditions = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayActions = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayFlags = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayText = new System.Windows.Forms.CheckBox();
            this.checkBoxUseActorColors = new System.Windows.Forms.CheckBox();
            this.comboBoxLanguages = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelFont = new System.Windows.Forms.Label();
            this.checkBoxUseConstants = new System.Windows.Forms.CheckBox();
            addSentenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addChoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addGotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // addSentenceToolStripMenuItem
            // 
            addSentenceToolStripMenuItem.Name = "addSentenceToolStripMenuItem";
            addSentenceToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            addSentenceToolStripMenuItem.Text = "Add Sentence";
            addSentenceToolStripMenuItem.Click += new System.EventHandler(this.OnBranchNodeSentence);
            // 
            // addChoiceToolStripMenuItem
            // 
            addChoiceToolStripMenuItem.Name = "addChoiceToolStripMenuItem";
            addChoiceToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            addChoiceToolStripMenuItem.Text = "Add Choice";
            addChoiceToolStripMenuItem.Click += new System.EventHandler(this.OnBranchNodeChoice);
            // 
            // addGotoToolStripMenuItem
            // 
            addGotoToolStripMenuItem.Name = "addGotoToolStripMenuItem";
            addGotoToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            addGotoToolStripMenuItem.Text = "Add Goto";
            addGotoToolStripMenuItem.Click += new System.EventHandler(this.OnBranchNodeGoto);
            // 
            // addBranchToolStripMenuItem
            // 
            addBranchToolStripMenuItem.Name = "addBranchToolStripMenuItem";
            addBranchToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            addBranchToolStripMenuItem.Text = "Add Branch";
            addBranchToolStripMenuItem.Click += new System.EventHandler(this.OnBranchNodeBranch);
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tree.HideSelection = false;
            this.tree.Location = new System.Drawing.Point(-1, 44);
            this.tree.Name = "tree";
            this.tree.ShowRootLines = false;
            this.tree.Size = new System.Drawing.Size(777, 419);
            this.tree.TabIndex = 0;
            this.tree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnNodeCollapse);
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnTreeItemDrag);
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelect);
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeMouseClick);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeMouseDoubleClick);
            this.tree.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTreeDragDrop);
            this.tree.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTreeDragEnter);
            this.tree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpenDirectory,
            this.menuItemCopyName,
            this.separatorRoot,
            this.menuItemAddReply,
            this.separatorReply,
            this.menuItemBranch,
            this.separatorBranch,
            this.menuItemAddSentence,
            this.menuItemAddChoice,
            this.menuItemAddGoto,
            this.menuItemAddBranch,
            this.separatorReference,
            this.menuItemCopyReference,
            this.menuItemPasteReference,
            this.menuItemCopyID,
            this.separatorMove,
            this.menuItemMoveNodeUp,
            this.menuItemMoveNodeDown,
            this.separatorDelete,
            this.menuItemDelete});
            this.contextMenu.Name = "m_pContextMenu";
            this.contextMenu.Size = new System.Drawing.Size(158, 348);
            this.contextMenu.Opened += new System.EventHandler(this.OnContextMenuOpened);
            // 
            // menuItemOpenDirectory
            // 
            this.menuItemOpenDirectory.Name = "menuItemOpenDirectory";
            this.menuItemOpenDirectory.Size = new System.Drawing.Size(157, 22);
            this.menuItemOpenDirectory.Text = "Open Directory";
            this.menuItemOpenDirectory.Click += new System.EventHandler(this.OnOpenDirectory);
            // 
            // menuItemCopyName
            // 
            this.menuItemCopyName.Name = "menuItemCopyName";
            this.menuItemCopyName.Size = new System.Drawing.Size(157, 22);
            this.menuItemCopyName.Text = "Copy Name";
            this.menuItemCopyName.Click += new System.EventHandler(this.OnCopyName);
            // 
            // separatorRoot
            // 
            this.separatorRoot.Name = "separatorRoot";
            this.separatorRoot.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemAddReply
            // 
            this.menuItemAddReply.Name = "menuItemAddReply";
            this.menuItemAddReply.Size = new System.Drawing.Size(157, 22);
            this.menuItemAddReply.Text = "Add Reply";
            this.menuItemAddReply.Click += new System.EventHandler(this.OnAddNodeReply);
            // 
            // separatorReply
            // 
            this.separatorReply.Name = "separatorReply";
            this.separatorReply.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemBranch
            // 
            this.menuItemBranch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            addSentenceToolStripMenuItem,
            addChoiceToolStripMenuItem,
            addGotoToolStripMenuItem,
            addBranchToolStripMenuItem});
            this.menuItemBranch.Name = "menuItemBranch";
            this.menuItemBranch.Size = new System.Drawing.Size(157, 22);
            this.menuItemBranch.Text = "Branch";
            // 
            // separatorBranch
            // 
            this.separatorBranch.Name = "separatorBranch";
            this.separatorBranch.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemAddSentence
            // 
            this.menuItemAddSentence.Name = "menuItemAddSentence";
            this.menuItemAddSentence.Size = new System.Drawing.Size(157, 22);
            this.menuItemAddSentence.Text = "Add Sentence";
            this.menuItemAddSentence.Click += new System.EventHandler(this.OnAddNodeSentence);
            // 
            // menuItemAddChoice
            // 
            this.menuItemAddChoice.Name = "menuItemAddChoice";
            this.menuItemAddChoice.Size = new System.Drawing.Size(157, 22);
            this.menuItemAddChoice.Text = "Add Choice";
            this.menuItemAddChoice.Click += new System.EventHandler(this.OnAddNodeChoice);
            // 
            // menuItemAddGoto
            // 
            this.menuItemAddGoto.Name = "menuItemAddGoto";
            this.menuItemAddGoto.Size = new System.Drawing.Size(157, 22);
            this.menuItemAddGoto.Text = "Add Goto";
            this.menuItemAddGoto.Click += new System.EventHandler(this.OnAddNodeGoto);
            // 
            // menuItemAddBranch
            // 
            this.menuItemAddBranch.Name = "menuItemAddBranch";
            this.menuItemAddBranch.Size = new System.Drawing.Size(157, 22);
            this.menuItemAddBranch.Text = "Add Branch";
            this.menuItemAddBranch.Click += new System.EventHandler(this.OnAddNodeBranch);
            // 
            // separatorReference
            // 
            this.separatorReference.Name = "separatorReference";
            this.separatorReference.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemCopyReference
            // 
            this.menuItemCopyReference.Name = "menuItemCopyReference";
            this.menuItemCopyReference.Size = new System.Drawing.Size(157, 22);
            this.menuItemCopyReference.Text = "Copy Reference";
            this.menuItemCopyReference.Click += new System.EventHandler(this.OnCopyReference);
            // 
            // menuItemPasteReference
            // 
            this.menuItemPasteReference.Name = "menuItemPasteReference";
            this.menuItemPasteReference.Size = new System.Drawing.Size(157, 22);
            this.menuItemPasteReference.Text = "Paste Reference";
            this.menuItemPasteReference.Click += new System.EventHandler(this.OnPasteReference);
            // 
            // menuItemCopyID
            // 
            this.menuItemCopyID.Name = "menuItemCopyID";
            this.menuItemCopyID.Size = new System.Drawing.Size(157, 22);
            this.menuItemCopyID.Text = "Copy ID";
            this.menuItemCopyID.Click += new System.EventHandler(this.OnCopyID);
            // 
            // separatorMove
            // 
            this.separatorMove.Name = "separatorMove";
            this.separatorMove.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemMoveNodeUp
            // 
            this.menuItemMoveNodeUp.Name = "menuItemMoveNodeUp";
            this.menuItemMoveNodeUp.Size = new System.Drawing.Size(157, 22);
            this.menuItemMoveNodeUp.Text = "Move Up";
            this.menuItemMoveNodeUp.Click += new System.EventHandler(this.OnMoveNodeUp);
            // 
            // menuItemMoveNodeDown
            // 
            this.menuItemMoveNodeDown.Name = "menuItemMoveNodeDown";
            this.menuItemMoveNodeDown.Size = new System.Drawing.Size(157, 22);
            this.menuItemMoveNodeDown.Text = "Move Down";
            this.menuItemMoveNodeDown.Click += new System.EventHandler(this.OnMoveNodeDown);
            // 
            // separatorDelete
            // 
            this.separatorDelete.Name = "separatorDelete";
            this.separatorDelete.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Name = "menuItemDelete";
            this.menuItemDelete.Size = new System.Drawing.Size(157, 22);
            this.menuItemDelete.Text = "Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.OnDeleteNode);
            // 
            // checkBoxDisplaySpeaker
            // 
            this.checkBoxDisplaySpeaker.AutoSize = true;
            this.checkBoxDisplaySpeaker.Location = new System.Drawing.Point(5, 5);
            this.checkBoxDisplaySpeaker.Name = "checkBoxDisplaySpeaker";
            this.checkBoxDisplaySpeaker.Size = new System.Drawing.Size(66, 17);
            this.checkBoxDisplaySpeaker.TabIndex = 10;
            this.checkBoxDisplaySpeaker.Text = "Speaker";
            this.checkBoxDisplaySpeaker.UseVisualStyleBackColor = true;
            this.checkBoxDisplaySpeaker.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxDisplayListener
            // 
            this.checkBoxDisplayListener.AutoSize = true;
            this.checkBoxDisplayListener.Location = new System.Drawing.Point(5, 23);
            this.checkBoxDisplayListener.Name = "checkBoxDisplayListener";
            this.checkBoxDisplayListener.Size = new System.Drawing.Size(63, 17);
            this.checkBoxDisplayListener.TabIndex = 11;
            this.checkBoxDisplayListener.Text = "Listener";
            this.checkBoxDisplayListener.UseVisualStyleBackColor = true;
            this.checkBoxDisplayListener.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxDisplayID
            // 
            this.checkBoxDisplayID.AutoSize = true;
            this.checkBoxDisplayID.Location = new System.Drawing.Point(74, 23);
            this.checkBoxDisplayID.Name = "checkBoxDisplayID";
            this.checkBoxDisplayID.Size = new System.Drawing.Size(76, 17);
            this.checkBoxDisplayID.TabIndex = 13;
            this.checkBoxDisplayID.Text = "ID (debug)";
            this.checkBoxDisplayID.UseVisualStyleBackColor = true;
            this.checkBoxDisplayID.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxDisplayConditions
            // 
            this.checkBoxDisplayConditions.AutoSize = true;
            this.checkBoxDisplayConditions.Location = new System.Drawing.Point(74, 5);
            this.checkBoxDisplayConditions.Name = "checkBoxDisplayConditions";
            this.checkBoxDisplayConditions.Size = new System.Drawing.Size(75, 17);
            this.checkBoxDisplayConditions.TabIndex = 12;
            this.checkBoxDisplayConditions.Text = "Conditions";
            this.checkBoxDisplayConditions.UseVisualStyleBackColor = true;
            this.checkBoxDisplayConditions.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxDisplayActions
            // 
            this.checkBoxDisplayActions.AutoSize = true;
            this.checkBoxDisplayActions.Location = new System.Drawing.Point(155, 5);
            this.checkBoxDisplayActions.Name = "checkBoxDisplayActions";
            this.checkBoxDisplayActions.Size = new System.Drawing.Size(61, 17);
            this.checkBoxDisplayActions.TabIndex = 14;
            this.checkBoxDisplayActions.Text = "Actions";
            this.checkBoxDisplayActions.UseVisualStyleBackColor = true;
            this.checkBoxDisplayActions.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxDisplayFlags
            // 
            this.checkBoxDisplayFlags.AutoSize = true;
            this.checkBoxDisplayFlags.Location = new System.Drawing.Point(222, 5);
            this.checkBoxDisplayFlags.Name = "checkBoxDisplayFlags";
            this.checkBoxDisplayFlags.Size = new System.Drawing.Size(51, 17);
            this.checkBoxDisplayFlags.TabIndex = 16;
            this.checkBoxDisplayFlags.Text = "Flags";
            this.checkBoxDisplayFlags.UseVisualStyleBackColor = true;
            this.checkBoxDisplayFlags.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxDisplayText
            // 
            this.checkBoxDisplayText.AutoSize = true;
            this.checkBoxDisplayText.Location = new System.Drawing.Point(155, 23);
            this.checkBoxDisplayText.Name = "checkBoxDisplayText";
            this.checkBoxDisplayText.Size = new System.Drawing.Size(47, 17);
            this.checkBoxDisplayText.TabIndex = 15;
            this.checkBoxDisplayText.Text = "Text";
            this.checkBoxDisplayText.UseVisualStyleBackColor = true;
            this.checkBoxDisplayText.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // checkBoxUseActorColors
            // 
            this.checkBoxUseActorColors.AutoSize = true;
            this.checkBoxUseActorColors.Location = new System.Drawing.Point(297, 5);
            this.checkBoxUseActorColors.Name = "checkBoxUseActorColors";
            this.checkBoxUseActorColors.Size = new System.Drawing.Size(83, 17);
            this.checkBoxUseActorColors.TabIndex = 17;
            this.checkBoxUseActorColors.Text = "Actor Colors";
            this.checkBoxUseActorColors.UseVisualStyleBackColor = true;
            this.checkBoxUseActorColors.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // comboBoxLanguages
            // 
            this.comboBoxLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguages.FormattingEnabled = true;
            this.comboBoxLanguages.Location = new System.Drawing.Point(633, 12);
            this.comboBoxLanguages.Name = "comboBoxLanguages";
            this.comboBoxLanguages.Size = new System.Drawing.Size(130, 21);
            this.comboBoxLanguages.TabIndex = 18;
            this.comboBoxLanguages.SelectedIndexChanged += new System.EventHandler(this.OnLanguageChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(289, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(2, 36);
            this.label1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(625, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(2, 36);
            this.label2.TabIndex = 22;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(524, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Font...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnChangeFont);
            // 
            // labelFont
            // 
            this.labelFont.Location = new System.Drawing.Point(395, 5);
            this.labelFont.Name = "labelFont";
            this.labelFont.Size = new System.Drawing.Size(123, 35);
            this.labelFont.TabIndex = 24;
            this.labelFont.Text = "labelFont";
            this.labelFont.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxUseConstants
            // 
            this.checkBoxUseConstants.AutoSize = true;
            this.checkBoxUseConstants.Location = new System.Drawing.Point(297, 23);
            this.checkBoxUseConstants.Name = "checkBoxUseConstants";
            this.checkBoxUseConstants.Size = new System.Drawing.Size(73, 17);
            this.checkBoxUseConstants.TabIndex = 25;
            this.checkBoxUseConstants.Text = "Constants";
            this.checkBoxUseConstants.UseVisualStyleBackColor = true;
            this.checkBoxUseConstants.CheckedChanged += new System.EventHandler(this.OnCheckDisplayOptions);
            // 
            // DocumentDialogue
            // 
            this.ClientSize = new System.Drawing.Size(775, 462);
            this.Controls.Add(this.checkBoxUseConstants);
            this.Controls.Add(this.labelFont);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxLanguages);
            this.Controls.Add(this.checkBoxUseActorColors);
            this.Controls.Add(this.checkBoxDisplayText);
            this.Controls.Add(this.checkBoxDisplayFlags);
            this.Controls.Add(this.checkBoxDisplayActions);
            this.Controls.Add(this.checkBoxDisplayConditions);
            this.Controls.Add(this.checkBoxDisplayID);
            this.Controls.Add(this.checkBoxDisplayListener);
            this.Controls.Add(this.checkBoxDisplaySpeaker);
            this.Controls.Add(this.tree);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DocumentDialogue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dialogue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClose);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddSentence;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddChoice;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddGoto;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddReply;
        private System.Windows.Forms.ToolStripSeparator separatorReference;
        private System.Windows.Forms.ToolStripMenuItem menuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyReference;
        private System.Windows.Forms.ToolStripSeparator separatorDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemPasteReference;
        private System.Windows.Forms.ToolStripSeparator separatorMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemMoveNodeUp;
        private System.Windows.Forms.ToolStripMenuItem menuItemMoveNodeDown;
        private System.Windows.Forms.CheckBox checkBoxDisplaySpeaker;
        private System.Windows.Forms.CheckBox checkBoxDisplayListener;
        private System.Windows.Forms.CheckBox checkBoxDisplayID;
        private System.Windows.Forms.CheckBox checkBoxDisplayConditions;
        private System.Windows.Forms.CheckBox checkBoxDisplayActions;
        private System.Windows.Forms.CheckBox checkBoxDisplayFlags;
        private System.Windows.Forms.CheckBox checkBoxDisplayText;
        private System.Windows.Forms.CheckBox checkBoxUseActorColors;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyID;
        private System.Windows.Forms.ToolStripSeparator separatorReply;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenDirectory;
        private System.Windows.Forms.ToolStripSeparator separatorRoot;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyName;
        private System.Windows.Forms.ComboBox comboBoxLanguages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelFont;
        private System.Windows.Forms.CheckBox checkBoxUseConstants;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddBranch;
        private System.Windows.Forms.ToolStripMenuItem menuItemBranch;
        private System.Windows.Forms.ToolStripSeparator separatorBranch;
    }
}