using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesRoot : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected Dialogue dialogue;
        protected DialogueNodeRoot dialogueNode;

        protected bool ready = false;

        List<string> additionalActors = new List<string>();
        protected int currentAdditionalActorIndex = -1;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesRoot()
        {
            InitializeComponent();

            Dock = System.Windows.Forms.DockStyle.Fill;

            EditorHelper.AbsorbMouseWheelEvent(comboBoxPackage);
            EditorHelper.AbsorbMouseWheelEvent(comboBoxSceneType);
            EditorHelper.AbsorbMouseWheelEvent(comboBoxAdditionalActors);
        }

        public void Clear()
        {
            ready = false;

            comboBoxSceneType.DataSource = null;

            Dispose();
        }

        public void ForceFocus()
        {
        }

        public bool IsEditingWorkstring()
        {
            return false;
        }

        public void ValidateEditedWorkstring()
        {
        }

        public void OnResolvePendingDirty()
        {
        }

        public void Init(DocumentDialogue indocument, TreeNode inTreeNode, DialogueNode inDialogueNode)
        {
            document = indocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode as DialogueNodeRoot;
            dialogue = document.Dialogue;

            textBoxVoiceBank.Text = dialogue.VoiceBank;
            textBoxContext.Text = dialogue.Context;
            textBoxComment.Text = dialogue.Comment;

            comboBoxPackage.DataSource = new BindingSource(ResourcesHandler.Project.ListPackages, null);
            comboBoxPackage.DisplayMember = "Name";
            comboBoxPackage.SelectedItem = dialogue.Package;

            comboBoxSceneType.DataSource = new BindingSource(EditorCore.CustomLists["SceneTypes"], null);
            comboBoxSceneType.ValueMember = "Key";
            comboBoxSceneType.DisplayMember = "Value";
            comboBoxSceneType.SelectedValue = dialogue.SceneType;

            foreach (var actorID in dialogue.ListAdditionalActors)
            {
                Actor actor = ResourcesHandler.Project.GetActorFromID(actorID);
                if (actor != null)
                    additionalActors.Add(actor.Name);
                else
                    additionalActors.Add("<Undefined>");
            }

            listBoxAdditionalActors.DataSource = new BindingSource(additionalActors, null);

            var actors = new Dictionary<string, string>();
            actors.Add("", "");
            foreach (Actor actor in ResourcesHandler.Project.ListActors)
            {
                actors.Add(actor.ID, actor.Name);
            }

            comboBoxAdditionalActors.DataSource = new BindingSource(actors, null);
            comboBoxAdditionalActors.ValueMember = "Key";
            comboBoxAdditionalActors.DisplayMember = "Value";

            RefreshAdditionalActorView();

            ready = true;
        }

        public void RefreshAdditionalActorView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            if (currentAdditionalActorIndex > -1)
            {
                comboBoxAdditionalActors.SelectedValue = dialogue.ListAdditionalActors[currentAdditionalActorIndex];
                comboBoxAdditionalActors.Enabled = true;
            }
            else
            {
                comboBoxAdditionalActors.SelectedIndex = 0;
                comboBoxAdditionalActors.Enabled = false;
            }

            ready = setReady;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnPackageChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            Package previous = dialogue.Package;
            dialogue.Package = (sender as ComboBox).SelectedValue as Package;

            document.SetDirty();

            if (EditorCore.ProjectExplorer != null)
                EditorCore.ProjectExplorer.ResyncFile(dialogue, previous, true);
        }

        private void OnSceneTypeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogue.SceneType = (sender as ComboBox).SelectedValue as string;

            document.SetDirty();
        }

        private void OnVoiceBankChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogue.VoiceBank = textBoxVoiceBank.Text;

            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnVoiceBankValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }

        private void OnCommentChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogue.Comment = textBoxComment.Text;

            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnCommentValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }

        private void OnContextChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogue.Context = textBoxContext.Text;

            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnContextValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }

        private void OnAdditionalActorsIndexChanged(object sender, EventArgs e)
        {
            currentAdditionalActorIndex = listBoxAdditionalActors.SelectedIndex;
            RefreshAdditionalActorView();
        }

        private void OnAddAdditionalActor(object sender, EventArgs e)
        {
            dialogue.ListAdditionalActors.Add("");
            additionalActors.Add("<Undefined>");

            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
            listBoxAdditionalActors.SelectedIndex = listBoxAdditionalActors.Items.Count - 1;

            if (currentAdditionalActorIndex == -1)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentAdditionalActorIndex = listBoxAdditionalActors.SelectedIndex;
                RefreshAdditionalActorView();
            }

            document.SetDirty();
        }

        private void OnRemoveAdditionalActor(object sender, EventArgs e)
        {
            int index = listBoxAdditionalActors.SelectedIndex;
            if (dialogue.ListAdditionalActors.Count == 0)
                return;

            currentAdditionalActorIndex = -1;
            dialogue.ListAdditionalActors.RemoveAt(index);
            additionalActors.RemoveAt(index);
            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);

            if (dialogue.ListAdditionalActors.Count > 0)
            {
                listBoxAdditionalActors.SelectedIndex = 0;

                if (currentAdditionalActorIndex == -1)
                {
                    currentAdditionalActorIndex = listBoxAdditionalActors.SelectedIndex;
                    RefreshAdditionalActorView();
                }
            }

            document.SetDirty();
        }

        private void OnMoveAdditionalActorUp(object sender, EventArgs e)
        {
            int index = listBoxAdditionalActors.SelectedIndex;
            if (dialogue.ListAdditionalActors.Count < 2 || index == 0)
                return;

            dialogue.ListAdditionalActors.Reverse(index - 1, 2);
            additionalActors.Reverse(index - 1, 2);
            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
            --listBoxAdditionalActors.SelectedIndex;

            document.SetDirty();
        }

        private void OnMoveAdditionalActorDown(object sender, EventArgs e)
        {
            int index = listBoxAdditionalActors.SelectedIndex;
            if (dialogue.ListAdditionalActors.Count < 2 || index == dialogue.ListAdditionalActors.Count - 1)
                return;

            dialogue.ListAdditionalActors.Reverse(index, 2);
            additionalActors.Reverse(index, 2);
            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
            ++listBoxAdditionalActors.SelectedIndex;

            document.SetDirty();
        }

        private void OnAdditionalActorNameChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            Actor actor = ResourcesHandler.Project.GetActorFromID((sender as ComboBox).SelectedValue as string);
            if (actor != null)
            {
                dialogue.ListAdditionalActors[currentAdditionalActorIndex] = actor.ID;
                additionalActors[currentAdditionalActorIndex] = actor.Name;
                (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);

                document.SetDirty();
            }
        }
    }
}
