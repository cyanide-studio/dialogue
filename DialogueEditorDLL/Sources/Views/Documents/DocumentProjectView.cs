using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DocumentProjectView : DocumentView
    {
        #region Variables
        //public ProjectController ProjectController;   // Once we make ProjectController non-static, we should do this

        private Language currentLanguage = null;
        private Package currentPackage = null;
        private Actor currentActor = null;
        private VoiceKit currentVoiceKit = null;
        private VoiceActor currentVoiceActor = null;
        private Constant currentConstant = null;

        private bool ready = false;
        #endregion

        #region Constructor
        public DocumentProjectView(/*ProjectController inProjectController*/)
        {
            InitializeComponent();

            //ProjectController = inProjectController;  // Once we make ProjectController non-static, we should do this

            InitDocument();
            RefreshTitle();
        }
        #endregion

        #region Init
        private void InitDocument()
        {
            ready = false;

            //---- General

            //General parameters
            textBoxCharsSentences.Text = ProjectController.Project.MaxLengthSentence.ToString();
            textBoxCharsReplies.Text = ProjectController.Project.MaxLengthReply.ToString();

            //Languages
            listBoxLanguages.DataSource = new BindingSource(ProjectController.Project.ListLanguages, null);
            listBoxLanguages.DisplayMember = "Name";

            //Packages
            listBoxPackages.DataSource = new BindingSource(ProjectController.Project.ListPackages, null);
            listBoxPackages.DisplayMember = "Name";

            //---- Debug

            //Custom Lists (debug view)
            if (EditorCore.CustomLists.Count > 0)
                listBoxCustomLists.DataSource = new BindingSource(EditorCore.CustomLists.Keys, null);

            //---- Actors

            //Species
            comboBoxSpecies.DataSource = new BindingSource(EditorCore.CustomLists["Species"], null);
            comboBoxSpecies.ValueMember = "Key";
            comboBoxSpecies.DisplayMember = "Value";
            //comboBoxSpecies.SelectedValue = "Human";

            //Gender
            comboBoxGender.DataSource = new BindingSource(EditorCore.CustomLists["Genders"], null);
            comboBoxGender.ValueMember = "Key";
            comboBoxGender.DisplayMember = "Value";
            //comboBoxGender.SelectedValue = "Male";

            //Build
            comboBoxBuild.DataSource = new BindingSource(EditorCore.CustomLists["Builds"], null);
            comboBoxBuild.ValueMember = "Key";
            comboBoxBuild.DisplayMember = "Value";
            //comboBoxBuild.SelectedValue = "Normal";

            //Actors
            listBoxActors.DataSource = new BindingSource(ProjectController.Project.ListActors, null);
            listBoxActors.DisplayMember = "Name";

            //---- Voicing

            listBoxVoiceKits.DataSource = new BindingSource(ProjectController.Project.ListVoiceKits, null);
            listBoxVoiceKits.DisplayMember = "Name";

            listBoxVoiceActors.DataSource = new BindingSource(ProjectController.Project.ListVoiceActors, null);
            listBoxVoiceActors.DisplayMember = "Name";

            //---- Constants

            listBoxConstants.DataSource = new BindingSource(ProjectController.Project.ListConstants, null);
            listBoxConstants.DisplayMember = "ID";

            //----

            RefreshLanguageView();
            RefreshPackageView();
            RefreshActorView();
            RefreshVoiceKitView();
            RefreshVoiceActorView();
            RefreshConstantView();

            ready = true;
        }
        #endregion

        #region DocumentView overrides
        public override void RefreshDocument()
        {
            ProjectController.ClearProperties();
        }

        public override void RefreshTitle()
        {
            Text = "Project " + ProjectController.Project.GetName();
            if (ProjectController.Dirty)
                Text += "*";
        }

        public override void OnPostReload()
        {
            InitDocument();
            RefreshTitle();
        }
        #endregion

        #region Misc
        public void SetDirty()
        {
            ProjectController.Dirty = true;
            RefreshTitle();
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            //UserClosing : cross, middle click, Application.Exit
            //MdiFormClosing : app form close, alt+f4
            if (ProjectController.MainWindow != null && e.CloseReason == CloseReason.UserClosing)
            {
                if (!ProjectController.MainWindow.OnDocumentProjectClosed(ForceClose))
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region Languages
        public void RefreshLanguageView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            if (currentLanguage != null)
            {
                textBoxLanguage.Text = currentLanguage.Name;
                textBoxLocalizationCode.Text = currentLanguage.LocalizationCode;
                textBoxVoicingCode.Text = currentLanguage.VoicingCode;

                textBoxLanguage.Enabled = true;
                textBoxLocalizationCode.Enabled = true;
                textBoxVoicingCode.Enabled = true;
            }
            else
            {
                textBoxLanguage.Text = "";
                textBoxLocalizationCode.Text = "";
                textBoxVoicingCode.Text = "";

                textBoxLanguage.Enabled = false;
                textBoxLocalizationCode.Enabled = false;
                textBoxVoicingCode.Enabled = false;
            }

            ready = setReady;
        }

        private void OnListLanguagesIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            currentLanguage = listBox.SelectedValue as Language;
            RefreshLanguageView();
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentLanguage.Name = textBoxLanguage.Text;
            (listBoxLanguages.DataSource as BindingSource).ResetBindings(false);

            SetDirty();
        }

        private void OnLocalizationCodeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentLanguage.LocalizationCode = textBoxLocalizationCode.Text;

            SetDirty();
        }

        private void OnVoicingCodeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentLanguage.VoicingCode = textBoxVoicingCode.Text;

            SetDirty();
        }

        private void OnAddLanguage(object sender, EventArgs e)
        {
            Language language = new Language() { Name = "Undefined" };
            ProjectController.Project.ListLanguages.Add(language);

            (listBoxLanguages.DataSource as BindingSource).ResetBindings(false);
            listBoxLanguages.SelectedIndex = listBoxLanguages.Items.Count - 1;

            if (currentLanguage == null)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentLanguage = listBoxLanguages.SelectedItem as Language;
                RefreshLanguageView();
            }

            SetDirty();
        }

        private void OnRemoveLanguage(object sender, EventArgs e)
        {
            int index = listBoxLanguages.SelectedIndex;
            if (ProjectController.Project.ListLanguages.Count == 0)
                return;

            currentLanguage = null;
            ProjectController.Project.ListLanguages.RemoveAt(index);
            (listBoxLanguages.DataSource as BindingSource).ResetBindings(false);

            if (ProjectController.Project.ListLanguages.Count > 0)
            {
                listBoxLanguages.SelectedIndex = 0;

                if (currentLanguage == null)
                {
                    currentLanguage = listBoxLanguages.SelectedItem as Language;
                    RefreshLanguageView();
                }
            }

            SetDirty();
        }

        private void OnMoveLanguageUp(object sender, EventArgs e)
        {
            int index = listBoxLanguages.SelectedIndex;
            if (ProjectController.Project.ListLanguages.Count < 2 || index == 0)
                return;

            ProjectController.Project.ListLanguages.Reverse(index - 1, 2);
            (listBoxLanguages.DataSource as BindingSource).ResetBindings(false);
            --listBoxLanguages.SelectedIndex;

            SetDirty();
        }

        private void OnMoveLanguageDown(object sender, EventArgs e)
        {
            int index = listBoxLanguages.SelectedIndex;
            if (ProjectController.Project.ListLanguages.Count < 2 || index == ProjectController.Project.ListLanguages.Count - 1)
                return;

            ProjectController.Project.ListLanguages.Reverse(index, 2);
            (listBoxLanguages.DataSource as BindingSource).ResetBindings(false);
            ++listBoxLanguages.SelectedIndex;

            SetDirty();
        }
        #endregion

        #region Packages
        public void RefreshPackageView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            if (currentPackage != null)
            {
                textBoxPackage.Text = currentPackage.Name;
                checkBoxPackageExport.Checked = currentPackage.Export;

                textBoxPackage.Enabled = true;
                checkBoxPackageExport.Enabled = true;
            }
            else
            {
                textBoxPackage.Text = "";
                checkBoxPackageExport.Checked = false;

                textBoxPackage.Enabled = false;
                checkBoxPackageExport.Enabled = false;
            }

            ready = setReady;
        }

        private void OnListPackagesIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            currentPackage = listBox.SelectedValue as Package;
            RefreshPackageView();
        }

        private void OnPackageChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            if (currentPackage.Name == textBoxPackage.Text)     //Since I use the validated event, this may be called even without modifications
                return;

            DialogResult result = MessageBox.Show("Renaming a Package will update all dialogues associated.\nDo you wish to continue ?", "Rename Package...", MessageBoxButtons.OKCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Cancel)
            {
                ready = false;
                textBoxPackage.Text = currentPackage.Name;
                ready = true;
                return;
            }

            currentPackage.Name = textBoxPackage.Text;
            (listBoxPackages.DataSource as BindingSource).ResetBindings(false);

            List<Dialogue> dialogues = ProjectController.GetAllDialogues();
            foreach (Dialogue dialogue in dialogues)
            {
                if (dialogue.Package == currentPackage)
                    ProjectController.SetDirty(dialogue);
            }

            SetDirty();
            ProjectController.RefreshDirtyFlags();
            ProjectController.ResyncAllFiles();
        }

        private void OnAddPackage(object sender, EventArgs e)
        {
            Package package = new Package() { Name = "Undefined" };
            ProjectController.Project.ListPackages.Add(package);

            (listBoxPackages.DataSource as BindingSource).ResetBindings(false);
            listBoxPackages.SelectedIndex = listBoxPackages.Items.Count - 1;

            if (currentPackage == null)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentPackage = listBoxPackages.SelectedItem as Package;
                RefreshPackageView();
            }

            SetDirty();
        }

        private void OnRemovePackage(object sender, EventArgs e)
        {
            int index = listBoxPackages.SelectedIndex;
            if (ProjectController.Project.ListPackages.Count == 0)
                return;

            currentPackage = null;
            ProjectController.Project.ListPackages.RemoveAt(index);
            (listBoxPackages.DataSource as BindingSource).ResetBindings(false);

            if (ProjectController.Project.ListPackages.Count > 0)
            {
                listBoxPackages.SelectedIndex = 0;

                if (currentPackage == null)
                {
                    currentPackage = listBoxPackages.SelectedItem as Package;
                    RefreshPackageView();
                }
            }

            SetDirty();
            ProjectController.ResyncAllFiles();
        }

        private void OnMovePackageUp(object sender, EventArgs e)
        {
            int index = listBoxPackages.SelectedIndex;
            if (ProjectController.Project.ListPackages.Count < 2 || index == 0)
                return;

            ProjectController.Project.ListPackages.Reverse(index - 1, 2);
            (listBoxPackages.DataSource as BindingSource).ResetBindings(false);
            --listBoxPackages.SelectedIndex;

            SetDirty();
        }

        private void OnMovePackageDown(object sender, EventArgs e)
        {
            int index = listBoxPackages.SelectedIndex;
            if (ProjectController.Project.ListPackages.Count < 2 || index == ProjectController.Project.ListPackages.Count - 1)
                return;

            ProjectController.Project.ListPackages.Reverse(index, 2);
            (listBoxPackages.DataSource as BindingSource).ResetBindings(false);
            ++listBoxPackages.SelectedIndex;

            SetDirty();
        }

        private void OnPackageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;

                textBoxPackage.Parent.Focus();
            }
        }

        private void OnPackageExportChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentPackage.Export = checkBoxPackageExport.Checked;

            SetDirty();
        }
        #endregion

        #region Workstrings
        private void OnMaxCharsSentenceChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            int value = 0;
            int.TryParse(textBoxCharsSentences.Text, out value);
            ProjectController.Project.MaxLengthSentence = value;

            SetDirty();
        }

        private void OnMaxCharsReplyChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            int value = 0;
            int.TryParse(textBoxCharsReplies.Text, out value);
            ProjectController.Project.MaxLengthReply = value;

            SetDirty();
        }
        #endregion

        #region Actors
        public void RefreshActorView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            RefreshActorVoiceKitList();

            bool hasActor = currentActor != null;

            textBoxID.Text = hasActor ? currentActor.ID : string.Empty;
            textBoxName.Text = hasActor ? currentActor.Name : string.Empty;
            if (hasActor)
            {
                comboBoxSpecies.SelectedValue = currentActor.Species;
                comboBoxGender.SelectedValue = currentActor.Gender;
                comboBoxBuild.SelectedValue = currentActor.Build;
                comboBoxVoiceKit.SelectedItem = currentActor.VoiceKit;
            }
            else
            {
                comboBoxSpecies.SelectedIndex = 0;
                comboBoxGender.SelectedIndex = 0;
                comboBoxBuild.SelectedIndex = 0;
                comboBoxVoiceKit.SelectedIndex = 0;
            }
            textBoxAge.Text = hasActor ? currentActor.Age.ToString() : string.Empty;
            textBoxHeight.Text = hasActor ? currentActor.Height.ToString() : string.Empty;
            textBoxPersonality.Text = hasActor ? currentActor.Personality : string.Empty;
            textBoxDescription.Text = hasActor ? currentActor.Description : string.Empty;
            textBoxPortrait.Text = hasActor ? currentActor.Portrait : string.Empty;
            pictureBoxColor.BackColor = hasActor ? Color.FromArgb(currentActor.Color) : Color.DarkSlateGray;

            //textBoxID.Enabled = hasActor;
            textBoxName.Enabled = hasActor;
            comboBoxSpecies.Enabled = hasActor;
            comboBoxGender.Enabled = hasActor;
            comboBoxBuild.Enabled = hasActor;
            comboBoxVoiceKit.Enabled = hasActor;
            textBoxAge.Enabled = hasActor;
            textBoxHeight.Enabled = hasActor;
            textBoxPersonality.Enabled = hasActor;
            textBoxDescription.Enabled = hasActor;
            textBoxPortrait.Enabled = hasActor;
            pictureBoxColor.Enabled = hasActor;
            buttonEditID.Enabled = hasActor;
            buttonEditPicture.Enabled = hasActor;
            buttonEditColor.Enabled = hasActor;

            RefreshPortrait();

            ready = setReady;
        }

        public void RefreshPortrait()
        {
            string pathPortrait = Path.Combine(EditorHelper.GetProjectDirectory(), textBoxPortrait.Text);
            if (File.Exists(pathPortrait))
            {
                using (Image temp = Image.FromFile(pathPortrait))
                {
                    pictureBoxPortrait.Image = new Bitmap(temp);
                }
                //pictureBoxPortrait.Image = Image.FromFile(strPathPortrait);
            }
            else
            {
                if (pictureBoxPortrait.Image != null)
                    pictureBoxPortrait.Image.Dispose();
                pictureBoxPortrait.Image = null;
            }
        }

        private void OnCustomListIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            string customListKey = listBox.SelectedItem as string;

            listBoxCustomListData.DataSource = new BindingSource(EditorCore.CustomLists[customListKey], null);
            listBoxCustomListData.ValueMember = "Key";
            listBoxCustomListData.DisplayMember = "Value";
        }

        private void OnAddActor(object sender, EventArgs e)
        {
            DialogRename dialog = new DialogRename(ProjectController.Project.GenerateNewActorID());
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Actor actor = new Actor();
                actor.ID = dialog.NewID;
                actor.Name = dialog.NewID;

                ProjectController.Project.ListActors.Add(actor);

                (listBoxActors.DataSource as BindingSource).ResetBindings(false);
                listBoxActors.SelectedIndex = listBoxActors.Items.Count - 1;

                if (currentActor == null)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
                {
                    currentActor = listBoxActors.SelectedValue as Actor;
                    RefreshActorView();
                }

                SetDirty();
            }
        }

        private void OnRemoveActor(object sender, EventArgs e)
        {
            int index = listBoxActors.SelectedIndex;
            if (ProjectController.Project.ListActors.Count == 0)
                return;

            string oldID = currentActor.ID;

            currentActor = null;
            ProjectController.Project.ListActors.RemoveAt(index);
            (listBoxActors.DataSource as BindingSource).ResetBindings(false);

            if (ProjectController.Project.ListActors.Count > 0)
            {
                listBoxActors.SelectedIndex = 0;

                if (currentActor == null)
                {
                    currentActor = listBoxActors.SelectedItem as Actor;
                    RefreshActorView();
                }
            }

            ProjectController.UpdateActorID(oldID, "");
        }

        private void OnMoveActorUp(object sender, EventArgs e)
        {
            int index = listBoxActors.SelectedIndex;
            if (ProjectController.Project.ListActors.Count < 2 || index == 0)
                return;

            ProjectController.Project.ListActors.Reverse(index - 1, 2);
            (listBoxActors.DataSource as BindingSource).ResetBindings(false);
            --listBoxActors.SelectedIndex;

            SetDirty();
        }

        private void OnMoveActorDown(object sender, EventArgs e)
        {
            int index = listBoxActors.SelectedIndex;
            if (ProjectController.Project.ListActors.Count < 2 || index == ProjectController.Project.ListActors.Count - 1)
                return;

            ProjectController.Project.ListActors.Reverse(index, 2);
            (listBoxActors.DataSource as BindingSource).ResetBindings(false);
            ++listBoxActors.SelectedIndex;

            SetDirty();
        }

        private void OnListActorsIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            currentActor = listBox.SelectedValue as Actor;
            RefreshActorView();
        }

        private void OnPictureChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            if (File.Exists(Path.Combine(EditorHelper.GetProjectDirectory(), textBoxPortrait.Text)))
                EditorCore.Settings.DirectoryPortraits = Path.GetDirectoryName(textBoxPortrait.Text);

            currentActor.Portrait = textBoxPortrait.Text;
            RefreshPortrait();

            SetDirty();
        }

        private void OnEditPictureClicked(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select Picture";
            //dialog.Filter = "Project Files|*.project|Dialogue Files|*.dlg";
            dialog.Filter = "Image Files|*.bmp; *.jpg; *.png; *.gif";

            string pathCurrentPortrait = Path.Combine(EditorHelper.GetProjectDirectory(), textBoxPortrait.Text);
            string dirPortraits = Path.Combine(EditorHelper.GetProjectDirectory(), EditorCore.Settings.DirectoryPortraits);

            dialog.InitialDirectory = EditorHelper.GetProjectDirectory();
            if (File.Exists(pathCurrentPortrait))
                dialog.InitialDirectory = Path.GetDirectoryName(pathCurrentPortrait);
            else if (Directory.Exists(dirPortraits))
                dialog.InitialDirectory = dirPortraits;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxPortrait.Text = Utility.GetRelativePath(dialog.FileName, EditorHelper.GetProjectDirectory());
            }
        }

        private void OnSpeciesChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.Species = (sender as ComboBox).SelectedValue as string;

            SetDirty();
        }

        private void OnGenderChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.Gender = (sender as ComboBox).SelectedValue as string;

            SetDirty();
        }

        private void OnDisplayNameChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.Name = textBoxName.Text;
            (listBoxActors.DataSource as BindingSource).ResetBindings(false);

            SetDirty();
        }

        private void OnPersonalityChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.Personality = textBoxPersonality.Text;

            SetDirty();
        }

        private void OnDescriptionChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.Description = textBoxDescription.Text;

            SetDirty();
        }

        private void OnEditColorClicked(object sender, EventArgs e)
        {
            if (!ready)
                return;

            ColorDialog dialog = new ColorDialog();
            dialog.Color = Color.FromArgb(currentActor.Color);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBoxColor.BackColor = dialog.Color;
                currentActor.Color = dialog.Color.ToArgb();

                SetDirty();
            }
        }

        private void OnVoiceKitChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.VoiceKit = (sender as ComboBox).SelectedValue as string;

            SetDirty();
        }

        private void OnBuildChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentActor.Build = (sender as ComboBox).SelectedValue as string;

            SetDirty();
        }

        private void OnAgeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            int age = 0;
            if (int.TryParse(textBoxAge.Text, out age))
            {
                currentActor.Age = age;
            }
            else
            {
                textBoxAge.Text = "";
            }

            SetDirty();
        }

        private void OnHeightChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            int height = 0;
            if (int.TryParse(textBoxHeight.Text, out height))
            {
                currentActor.Height = height;
            }
            else
            {
                textBoxHeight.Text = "";
            }

            SetDirty();
        }

        private void OnEditIDClicked(object sender, EventArgs e)
        {
            if (!ready)
                return;

            DialogRename dialog = new DialogRename(currentActor.ID);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string oldID = currentActor.ID;
                if (ProjectController.Project.ChangeActorID(currentActor, dialog.NewID))
                {
                    ProjectController.UpdateActorID(oldID, currentActor.ID);
                    textBoxID.Text = currentActor.ID;
                }
            }
        }
        #endregion

        #region Voice Kits
        public void RefreshVoiceKitView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            RefreshVoiceKitVoiceActorList();

            if (currentVoiceKit != null)
            {
                textBoxVoiceKitName.Text = currentVoiceKit.Name;
                comboBoxVoiceKitVoiceActor.SelectedItem = currentVoiceKit.VoiceActor;

                textBoxVoiceKitName.Enabled = true;
                comboBoxVoiceKitVoiceActor.Enabled = true;
            }
            else
            {
                textBoxVoiceKitName.Text = "";
                comboBoxVoiceKitVoiceActor.SelectedIndex = 0;

                textBoxVoiceKitName.Enabled = false;
                comboBoxVoiceKitVoiceActor.Enabled = false;
            }

            ready = setReady;
        }

        private void OnVoiceKitIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            currentVoiceKit = listBox.SelectedValue as VoiceKit;
            RefreshVoiceKitView();
        }

        private void OnVoiceKitNameChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            if (ProjectController.Project.ChangeVoiceKitName(currentVoiceKit, textBoxVoiceKitName.Text))
            {
                (listBoxVoiceKits.DataSource as BindingSource).ResetBindings(false);

                RefreshActorVoiceKitList();

                SetDirty();
            }
        }

        private void OnVoiceKitVoiceActorChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentVoiceKit.VoiceActor = (sender as ComboBox).SelectedValue as string;

            SetDirty();
        }

        private void OnAddVoiceKit(object sender, EventArgs e)
        {
            VoiceKit voiceKit = new VoiceKit() { Name = "Undefined" };
            ProjectController.Project.ListVoiceKits.Add(voiceKit);

            (listBoxVoiceKits.DataSource as BindingSource).ResetBindings(false);
            listBoxVoiceKits.SelectedIndex = listBoxVoiceKits.Items.Count - 1;

            if (currentVoiceKit == null)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentVoiceKit = listBoxVoiceKits.SelectedItem as VoiceKit;
                RefreshVoiceKitView();
            }

            RefreshActorVoiceKitList();

            SetDirty();
        }

        private void OnRemoveVoiceKit(object sender, EventArgs e)
        {
            int index = listBoxVoiceKits.SelectedIndex;
            if (ProjectController.Project.ListVoiceKits.Count == 0)
                return;

            currentVoiceKit = null;
            ProjectController.Project.ListVoiceKits.RemoveAt(index);
            (listBoxVoiceKits.DataSource as BindingSource).ResetBindings(false);

            if (ProjectController.Project.ListVoiceKits.Count > 0)
            {
                listBoxVoiceKits.SelectedIndex = 0;

                if (currentVoiceKit == null)
                {
                    currentVoiceKit = listBoxVoiceKits.SelectedItem as VoiceKit;
                    RefreshVoiceKitView();
                }
            }

            RefreshActorVoiceKitList();

            SetDirty();
        }

        private void OnMoveVoiceKitUp(object sender, EventArgs e)
        {
            int index = listBoxVoiceKits.SelectedIndex;
            if (ProjectController.Project.ListVoiceKits.Count < 2 || index == 0)
                return;

            ProjectController.Project.ListVoiceKits.Reverse(index - 1, 2);
            (listBoxVoiceKits.DataSource as BindingSource).ResetBindings(false);
            --listBoxVoiceKits.SelectedIndex;

            RefreshActorVoiceKitList();

            SetDirty();
        }

        private void OnMoveVoiceKitDown(object sender, EventArgs e)
        {
            int index = listBoxVoiceKits.SelectedIndex;
            if (ProjectController.Project.ListVoiceKits.Count < 2 || index == ProjectController.Project.ListVoiceKits.Count - 1)
                return;

            ProjectController.Project.ListVoiceKits.Reverse(index, 2);
            (listBoxVoiceKits.DataSource as BindingSource).ResetBindings(false);
            ++listBoxVoiceKits.SelectedIndex;

            RefreshActorVoiceKitList();

            SetDirty();
        }
        #endregion

        #region Voice Actors
        public void RefreshVoiceActorView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            if (currentVoiceActor != null)
            {
                textBoxVoiceActorName.Text = currentVoiceActor.Name;

                textBoxVoiceActorName.Enabled = true;
                buttonEditLocalizedVoiceActors.Enabled = true;
            }
            else
            {
                textBoxVoiceActorName.Text = "";

                textBoxVoiceActorName.Enabled = false;
                buttonEditLocalizedVoiceActors.Enabled = false;
            }

            ready = setReady;
        }
        
        public void RefreshActorVoiceKitList()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            var listVoiceKits = new List<string>();
            listVoiceKits.Add("");
            listVoiceKits.AddRange(from item in ProjectController.Project.ListVoiceKits select item.Name);
            comboBoxVoiceKit.DataSource = new BindingSource(listVoiceKits, null);

            if (currentActor != null)
            {
                comboBoxVoiceKit.SelectedItem = currentActor.VoiceKit;
            }

            ready = setReady;
        }

        public void RefreshVoiceKitVoiceActorList()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            var listVoiceActors = new List<string>();
            listVoiceActors.Add("");
            listVoiceActors.AddRange(from item in ProjectController.Project.ListVoiceActors select item.Name);
            comboBoxVoiceKitVoiceActor.DataSource = new BindingSource(listVoiceActors, null);

            if (currentVoiceKit != null)
            {
                comboBoxVoiceKitVoiceActor.SelectedItem = currentVoiceKit.VoiceActor;
            }

            ready = setReady;
        }

        private void OnVoiceActorIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            currentVoiceActor = listBox.SelectedValue as VoiceActor;
            RefreshVoiceActorView();
        }

        private void OnVoiceActorNameChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            if (ProjectController.Project.ChangeVoiceActorName(currentVoiceActor, textBoxVoiceActorName.Text))
            {
                (listBoxVoiceActors.DataSource as BindingSource).ResetBindings(false);

                RefreshVoiceKitVoiceActorList();

                SetDirty();
            }
        }

        private void OnLocalizedVoiceActorsClicked(object sender, EventArgs e)
        {
            var dialog = new DialogVoiceActors(this, currentVoiceActor);
            DialogResult result = dialog.ShowDialog();
        }

        private void OnAddVoiceActor(object sender, EventArgs e)
        {
            VoiceActor voiceActor = new VoiceActor() { Name = "Undefined" };
            ProjectController.Project.ListVoiceActors.Add(voiceActor);

            (listBoxVoiceActors.DataSource as BindingSource).ResetBindings(false);
            listBoxVoiceActors.SelectedIndex = listBoxVoiceActors.Items.Count - 1;

            if (currentVoiceActor == null)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentVoiceActor = listBoxVoiceActors.SelectedItem as VoiceActor;
                RefreshVoiceActorView();
            }

            RefreshVoiceKitVoiceActorList();

            SetDirty();
        }

        private void OnRemoveVoiceActor(object sender, EventArgs e)
        {
            int index = listBoxVoiceActors.SelectedIndex;
            if (ProjectController.Project.ListVoiceActors.Count == 0)
                return;

            currentVoiceActor = null;
            ProjectController.Project.ListVoiceActors.RemoveAt(index);
            (listBoxVoiceActors.DataSource as BindingSource).ResetBindings(false);

            if (ProjectController.Project.ListVoiceActors.Count > 0)
            {
                listBoxVoiceActors.SelectedIndex = 0;

                if (currentVoiceActor == null)
                {
                    currentVoiceActor = listBoxVoiceActors.SelectedItem as VoiceActor;
                    RefreshVoiceActorView();
                }
            }

            RefreshVoiceKitVoiceActorList();

            SetDirty();
        }

        private void OnMoveVoiceActorUp(object sender, EventArgs e)
        {
            int index = listBoxVoiceActors.SelectedIndex;
            if (ProjectController.Project.ListVoiceActors.Count < 2 || index == 0)
                return;

            ProjectController.Project.ListVoiceActors.Reverse(index - 1, 2);
            (listBoxVoiceActors.DataSource as BindingSource).ResetBindings(false);
            --listBoxVoiceActors.SelectedIndex;

            RefreshVoiceKitVoiceActorList();

            SetDirty();
        }

        private void OnMoveVoiceActorDown(object sender, EventArgs e)
        {
            int index = listBoxVoiceActors.SelectedIndex;
            if (ProjectController.Project.ListVoiceActors.Count < 2 || index == ProjectController.Project.ListVoiceActors.Count - 1)
                return;

            ProjectController.Project.ListVoiceActors.Reverse(index, 2);
            (listBoxVoiceActors.DataSource as BindingSource).ResetBindings(false);
            ++listBoxVoiceActors.SelectedIndex;

            RefreshVoiceKitVoiceActorList();

            SetDirty();
        }
        #endregion

        #region Constants
        public void RefreshConstantView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            if (currentConstant != null)
            {
                textBoxConstantID.Text = currentConstant.ID;
                textBoxConstantWorkstring.Text = currentConstant.Workstring;
                textBoxConstantComment.Text = currentConstant.Comment;

                textBoxConstantID.Enabled = true;
                textBoxConstantWorkstring.Enabled = true;
                textBoxConstantComment.Enabled = true;
            }
            else
            {
                textBoxConstantID.Text = "";
                textBoxConstantWorkstring.Text = "";
                textBoxConstantComment.Text = "";

                textBoxConstantID.Enabled = false;
                textBoxConstantWorkstring.Enabled = false;
                textBoxConstantComment.Enabled = false;
            }

            ready = setReady;
        }

        private void OnConstantIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            currentConstant = listBox.SelectedValue as Constant;
            RefreshConstantView();
        }

        private void OnConstantIDChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentConstant.ID = textBoxConstantID.Text;

            (listBoxConstants.DataSource as BindingSource).ResetBindings(false);

            SetDirty();
        }

        private void OnConstantWorkstringChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentConstant.Workstring = textBoxConstantWorkstring.Text;
            currentConstant.LastEditDate = Utility.GetCurrentTime();

            SetDirty();
        }

        private void OnConstantCommentChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            currentConstant.Comment = textBoxConstantComment.Text;

            SetDirty();
        }

        private void OnAddConstant(object sender, EventArgs e)
        {
            Constant constant = new Constant() { ID = "Undefined" };
            ProjectController.Project.ListConstants.Add(constant);

            (listBoxConstants.DataSource as BindingSource).ResetBindings(false);
            listBoxConstants.SelectedIndex = listBoxConstants.Items.Count - 1;

            if (currentConstant == null)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentConstant = listBoxConstants.SelectedItem as Constant;
                RefreshConstantView();
            }

            SetDirty();
        }

        private void OnRemoveConstant(object sender, EventArgs e)
        {
            int index = listBoxConstants.SelectedIndex;
            if (ProjectController.Project.ListConstants.Count == 0)
                return;

            currentConstant = null;
            ProjectController.Project.ListConstants.RemoveAt(index);
            (listBoxConstants.DataSource as BindingSource).ResetBindings(false);

            if (ProjectController.Project.ListConstants.Count > 0)
            {
                listBoxConstants.SelectedIndex = 0;

                if (currentConstant == null)
                {
                    currentConstant = listBoxConstants.SelectedItem as Constant;
                    RefreshConstantView();
                }
            }

            SetDirty();
        }

        private void OnMoveConstantUp(object sender, EventArgs e)
        {
            int index = listBoxConstants.SelectedIndex;
            if (ProjectController.Project.ListConstants.Count < 2 || index == 0)
                return;

            ProjectController.Project.ListConstants.Reverse(index - 1, 2);
            (listBoxConstants.DataSource as BindingSource).ResetBindings(false);
            --listBoxConstants.SelectedIndex;

            SetDirty();
        }

        private void OnMoveConstantDown(object sender, EventArgs e)
        {
            int index = listBoxConstants.SelectedIndex;
            if (ProjectController.Project.ListConstants.Count < 2 || index == ProjectController.Project.ListConstants.Count - 1)
                return;

            ProjectController.Project.ListConstants.Reverse(index, 2);
            (listBoxConstants.DataSource as BindingSource).ResetBindings(false);
            ++listBoxConstants.SelectedIndex;

            SetDirty();
        }
        #endregion
    }
}
