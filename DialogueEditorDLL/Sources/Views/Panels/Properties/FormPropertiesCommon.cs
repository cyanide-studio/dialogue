using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesCommon : FormProperties
    {
        #region Internal vars
        protected DialogueNode dialogueNode;
        protected TreeNode treeNodeRootConditions;
        protected TreeNode treeNodeRootActions;
        protected TreeNode treeNodeRootFlags;
        protected TreeNode selectedTreeNode;
        #endregion

        #region Constructor
        public FormPropertiesCommon()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNode inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            treeAttributes.ImageList = EditorCore.DefaultImageList;

            //Roots
            treeNodeRootConditions = treeAttributes.Nodes.Add("Root", "Conditions");
            treeNodeRootConditions.ContextMenuStrip = menuAttributes;
            EditorHelper.SetNodeIcon(treeNodeRootConditions, ENodeIcon.ListRootConditions);

            treeNodeRootActions = treeAttributes.Nodes.Add("Root", "Actions");
            treeNodeRootActions.ContextMenuStrip = menuAttributes;
            EditorHelper.SetNodeIcon(treeNodeRootActions, ENodeIcon.ListRootActions);

            treeNodeRootFlags = treeAttributes.Nodes.Add("Root", "Flags");
            treeNodeRootFlags.ContextMenuStrip = menuAttributes;
            EditorHelper.SetNodeIcon(treeNodeRootFlags, ENodeIcon.ListRootFlags);

            //Sync Conditions
            foreach (NodeCondition condition in dialogueNode.Conditions)
            {
                AddTreeNodeCondition(condition, treeNodeRootConditions);
            }

            //Sync Actions
            foreach (NodeAction action in dialogueNode.Actions)
            {
                AddTreeNodeAction(action);
            }

            //Sync Flags
            foreach (NodeFlag flag in dialogueNode.Flags)
            {
                AddTreeNodeFlag(flag);
            }

            //Fill Menu Conditions
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = "AND";
                menuItem.Click += delegate
                {
                    AddNodeCondition(Activator.CreateInstance(typeof(NodeConditionAnd)) as NodeCondition);
                };
                menuItemAddCondition.DropDownItems.Add(menuItem);
            }

            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = "OR";
                menuItem.Click += delegate
                {
                    AddNodeCondition(Activator.CreateInstance(typeof(NodeConditionOr)) as NodeCondition);
                };
                menuItemAddCondition.DropDownItems.Add(menuItem);
            }

            menuItemAddCondition.DropDownItems.Add(new ToolStripSeparator());

            foreach (ConditionSlot slot in EditorCore.ConditionSlots)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = slot.Text;
                menuItem.Click += delegate
                {
                    AddNodeCondition(Activator.CreateInstance(slot.ConditionType) as NodeCondition);
                };
                menuItemAddCondition.DropDownItems.Add(menuItem);
            }

            //Fill Menu Actions
            foreach (ActionSlot slot in EditorCore.ActionSlots)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = slot.Text;
                menuItem.Click += delegate
                {
                    AddNodeAction(Activator.CreateInstance(slot.ActionType) as NodeAction);
                };
                menuItemAddAction.DropDownItems.Add(menuItem);
            }

            //Fill Menu Flags
            foreach (FlagSlot slot in EditorCore.FlagSlots)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = slot.Text;
                menuItem.Click += delegate
                {
                    AddNodeFlag(Activator.CreateInstance(slot.FlagType) as NodeFlag);
                };
                menuItemAddFlag.DropDownItems.Add(menuItem);
            }

            ready = true;
        }
        #endregion

        #region Events
        public override void Clear()
        {
            ready = false;
            propertyGridAttributes.SelectedObject = null;
            Dispose();
        }

        private void AddTreeNodeCondition(NodeCondition condition, TreeNode parent)
        {
            TreeNode treeNodeAttribute = parent.Nodes.Add(condition.GetTreeText());
            treeNodeAttribute.ContextMenuStrip = menuAttributes;
            treeNodeAttribute.Tag = condition;

            if (condition is NodeConditionGroup)
            {
                EditorHelper.SetNodeIcon(treeNodeAttribute, ENodeIcon.ListItemConditionGroup);

                foreach (NodeCondition subCondition in ((NodeConditionGroup)condition).Conditions)
                {
                    AddTreeNodeCondition(subCondition, treeNodeAttribute);
                }
            }
            else
            {
                EditorHelper.SetNodeIcon(treeNodeAttribute, ENodeIcon.ListItemCondition);
            }

            parent.ExpandAll();
        }

        private void AddTreeNodeAction(NodeAction action)
        {
            TreeNode treeNodeAttribute = treeNodeRootActions.Nodes.Add(action.GetTreeText());
            treeNodeAttribute.ContextMenuStrip = menuAttributes;
            treeNodeAttribute.Tag = action;
            EditorHelper.SetNodeIcon(treeNodeAttribute, ENodeIcon.ListItemAction);

            treeNodeRootActions.ExpandAll();
        }

        private void AddTreeNodeFlag(NodeFlag flag)
        {
            TreeNode treeNodeAttribute = treeNodeRootFlags.Nodes.Add(flag.GetTreeText());
            treeNodeAttribute.ContextMenuStrip = menuAttributes;
            treeNodeAttribute.Tag = flag;
            EditorHelper.SetNodeIcon(treeNodeAttribute, ENodeIcon.ListItemFlag);

            treeNodeRootFlags.ExpandAll();
        }

        private void AddNodeCondition(NodeCondition condition)
        {
            if (condition != null)
            {
                if (selectedTreeNode == treeNodeRootConditions)
                {
                    dialogueNode.Conditions.Add(condition);
                }
                else if (selectedTreeNode.Tag is NodeConditionGroup)
                {
                    ((NodeConditionGroup)selectedTreeNode.Tag).Conditions.Add(condition);
                }

                AddTreeNodeCondition(condition, selectedTreeNode);
                dialogueController.NotifyModifiedDialogueNode(dialogueNode);
            }
        }

        private void AddNodeAction(NodeAction action)
        {
            if (action != null)
            {
                dialogueNode.Actions.Add(action);
                AddTreeNodeAction(action);
                dialogueController.NotifyModifiedDialogueNode(dialogueNode);
            }
        }

        private void AddNodeFlag(NodeFlag flag)
        {
            if (flag != null)
            {
                dialogueNode.Flags.Add(flag);
                AddTreeNodeFlag(flag);
                dialogueController.NotifyModifiedDialogueNode(dialogueNode);
            }
        }

        private void RemoveTreeNode(TreeNode node)
        {
            if (node == null)
                return;

            if (selectedTreeNode == treeNodeRootConditions)
            {
                RemoveAllConditions();
            }
            else if (selectedTreeNode == treeNodeRootActions)
            {
                RemoveAllActions();
            }
            else if (selectedTreeNode == treeNodeRootFlags)
            {
                RemoveAllFlags();
            }
            else
            {
                NodeCondition condition = node.Tag as NodeCondition;
                NodeAction action = node.Tag as NodeAction;
                NodeFlag flag = node.Tag as NodeFlag;

                var parent = node.Parent;
                node.Remove();

                if (condition != null)
                {
                    if (parent.Tag is NodeConditionGroup)
                    {
                        ((NodeConditionGroup)parent.Tag).Conditions.Remove(condition);
                    }
                    else
                    {
                        dialogueNode.Conditions.Remove(condition);
                    }
                }
                else if (action != null)
                {
                    dialogueNode.Actions.Remove(action);
                }
                else if (flag != null)
                {
                    dialogueNode.Flags.Remove(flag);
                }
            }
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void RemoveAllConditions()
        {
            treeNodeRootConditions.Nodes.Clear();
            dialogueNode.Conditions.Clear();
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void RemoveAllActions()
        {
            treeNodeRootActions.Nodes.Clear();
            dialogueNode.Actions.Clear();
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void RemoveAllFlags()
        {
            treeNodeRootFlags.Nodes.Clear();
            dialogueNode.Flags.Clear();
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnDeleteTreeItem(object sender, EventArgs e)
        {
            propertyGridAttributes.SelectedObject = null;
            RemoveTreeNode(selectedTreeNode);
            selectedTreeNode = null;
        }

        private void OnNodeAttributeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedTreeNode = e.Node;
            propertyGridAttributes.SelectedObject = selectedTreeNode.Tag;
        }

        private void OnContextMenuOpened(object sender, EventArgs e)
        {
            menuItemAddCondition.Visible = false;
            menuItemAddAction.Visible = false;
            menuItemAddFlag.Visible = false;
            menuItemSeparator.Visible = false;
            menuItemDelete.Visible = false;

            if (selectedTreeNode == treeNodeRootConditions)
            {
                menuItemAddCondition.Visible = true;
            }
            else if (selectedTreeNode == treeNodeRootActions)
            {
                menuItemAddAction.Visible = true;
            }
            else if (selectedTreeNode == treeNodeRootFlags)
            {
                menuItemAddFlag.Visible = true;
            }
            else if (selectedTreeNode.Tag is NodeConditionGroup)
            {
                menuItemAddCondition.Visible = true;
                menuItemSeparator.Visible = true;
                menuItemDelete.Visible = true;
            }
            else
            {
                menuItemDelete.Visible = true;
            }
        }

        private void OnPropertyGridAttributesChanged(object s, PropertyValueChangedEventArgs e)
        {
            NodeCondition condition = selectedTreeNode.Tag as NodeCondition;
            NodeAction action = selectedTreeNode.Tag as NodeAction;
            NodeFlag flag = selectedTreeNode.Tag as NodeFlag;

            //Refresh node text
            if (condition != null)
            {
                selectedTreeNode.Text = condition.GetTreeText();
            }
            if (action != null)
            {
                selectedTreeNode.Text = action.GetTreeText();
            }
            if (flag != null)
            {
                selectedTreeNode.Text = flag.GetTreeText();
            }
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnNodeAttributeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (selectedTreeNode == treeNodeRootConditions || selectedTreeNode == treeNodeRootActions || selectedTreeNode == treeNodeRootFlags)
            {
                e.Cancel = true;
            }
        }

        private void OnAttributesKeyDown(object sender, KeyEventArgs e)
        {
            if (selectedTreeNode == null)
                return;

            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;

                propertyGridAttributes.SelectedObject = null;
                RemoveTreeNode(selectedTreeNode);

                if (selectedTreeNode != treeNodeRootConditions && selectedTreeNode != treeNodeRootActions && selectedTreeNode != treeNodeRootFlags)
                {
                    selectedTreeNode = null;
                }
            }
            else if (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.X))
            {
                e.Handled = true;

                if (selectedTreeNode.Tag is NodeCondition)
                {
                    EditorHelper.Clipboard = (selectedTreeNode.Tag as NodeCondition).Clone();
                    EditorHelper.ClipboardInfos = null;
                }
                else if (selectedTreeNode.Tag is NodeAction)
                {
                    EditorHelper.Clipboard = (selectedTreeNode.Tag as NodeAction).Clone();
                    EditorHelper.ClipboardInfos = null;
                }
                else if (selectedTreeNode.Tag is NodeFlag)
                {
                    EditorHelper.Clipboard = (selectedTreeNode.Tag as NodeFlag).Clone();
                    EditorHelper.ClipboardInfos = null;
                }
                else if (selectedTreeNode == treeNodeRootConditions)
                {
                    EditorHelper.Clipboard = dialogueNode.Conditions.Select(item => (NodeCondition)item.Clone()).ToList();
                    EditorHelper.ClipboardInfos = null;
                }
                else if (selectedTreeNode == treeNodeRootActions)
                {
                    EditorHelper.Clipboard = dialogueNode.Actions.Select(item => (NodeAction)item.Clone()).ToList();
                    EditorHelper.ClipboardInfos = null;
                }
                else if (selectedTreeNode == treeNodeRootFlags)
                {
                    EditorHelper.Clipboard = dialogueNode.Flags.Select(item => (NodeFlag)item.Clone()).ToList();
                    EditorHelper.ClipboardInfos = null;
                }

                if (e.KeyCode == Keys.X)
                {
                    propertyGridAttributes.SelectedObject = null;
                    RemoveTreeNode(selectedTreeNode);

                    if (selectedTreeNode != treeNodeRootConditions && selectedTreeNode != treeNodeRootActions && selectedTreeNode != treeNodeRootFlags)
                    {
                        selectedTreeNode = null;
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;

                if ((selectedTreeNode == treeNodeRootConditions || selectedTreeNode.Tag is NodeConditionGroup) && EditorHelper.Clipboard is NodeCondition)
                {
                    AddNodeCondition((EditorHelper.Clipboard as NodeCondition).Clone() as NodeCondition);
                }
                else if (selectedTreeNode == treeNodeRootActions && EditorHelper.Clipboard is NodeAction)
                {
                    AddNodeAction((EditorHelper.Clipboard as NodeAction).Clone() as NodeAction);
                }
                else if (selectedTreeNode == treeNodeRootFlags && EditorHelper.Clipboard is NodeFlag)
                {
                    AddNodeFlag((EditorHelper.Clipboard as NodeFlag).Clone() as NodeFlag);
                }
                else if ((selectedTreeNode == treeNodeRootConditions || selectedTreeNode.Tag is NodeConditionGroup) && EditorHelper.Clipboard is List<NodeCondition>)
                {
                    var newList = ((List<NodeCondition>)EditorHelper.Clipboard).Clone();
                    foreach (var node in newList)
                    {
                        AddNodeCondition(node.Clone() as NodeCondition);
                    }
                }
                else if (selectedTreeNode == treeNodeRootActions && EditorHelper.Clipboard is List<NodeAction>)
                {
                    var newList = ((List<NodeAction>)EditorHelper.Clipboard).Clone();
                    foreach (var node in newList)
                    {
                        AddNodeAction(node.Clone() as NodeAction);
                    }
                }
                else if (selectedTreeNode == treeNodeRootFlags && EditorHelper.Clipboard is List<NodeFlag>)
                {
                    var newList = ((List<NodeFlag>)EditorHelper.Clipboard).Clone();
                    foreach (var node in newList)
                    {
                        AddNodeFlag(node.Clone() as NodeFlag);
                    }
                }
            }
        }
        #endregion
    }
}
