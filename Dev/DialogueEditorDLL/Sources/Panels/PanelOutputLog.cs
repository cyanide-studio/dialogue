using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
    };

    public partial class PanelOutputLog : DockContent
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        protected class LogItem
        {
            public LogLevel Level = LogLevel.Info;
            public string Text = "";
            public string Dialogue = "";
            public int Node = DialogueNode.ID_NULL;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public int MaxLines = 2000;

        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        private List<LogItem> listItems = new List<LogItem>();
        private Font font = null;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public PanelOutputLog()
        {
            InitializeComponent();

            font = listBoxLog.Font;
            listBoxLog.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxLog.DrawItem += listBox_DrawItem;

            checkBoxShowInfos.Checked = true;
            checkBoxShowWarnings.Checked = true;
            checkBoxShowErrors.Checked = true;
        }

        public void WriteLine(LogLevel level, string message)
        {
            WriteLine(level, message, "", DialogueNode.ID_NULL);
        }

        public void WriteLine(LogLevel level, string message, string dialogue, int node)
        {
            LogItem item = new LogItem();

            StringBuilder stringBuilder = new StringBuilder();
            bool displayLine = false;
            bool showPanel = false;

            stringBuilder.Append(String.Format("[{0}]  ", Utility.GetCurrentTimeAsString()));

            switch (level)
            {
                case LogLevel.Info:
                    displayLine = checkBoxShowInfos.Checked;
                    stringBuilder.Append("Info : ");
                    break;

                case LogLevel.Warning:
                    displayLine = checkBoxShowWarnings.Checked;
                    showPanel = true;
                    stringBuilder.Append("Warning : "); 
                    break;

                case LogLevel.Error:
                    displayLine = checkBoxShowErrors.Checked;
                    showPanel = true;
                    stringBuilder.Append("Error : "); 
                    break;
            }

            stringBuilder.Append(message);

            item.Level = level;
            item.Text = stringBuilder.ToString();
            item.Dialogue = dialogue;
            item.Node = node;
            listItems.Add(item);

            if (displayLine)
            {
                listBoxLog.Items.Add(item);
            }

            //Handle max limit
            if (listBoxLog.Items.Count > MaxLines)
            {
                listBoxLog.Items.RemoveAt(0);
            }

            //Force scroll to bottom
            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;

            //Force visibility in case of errors
            if (showPanel && !Visible)
            {
                Show();
            }
        }

        private void RefreshItems()
        {
            listBoxLog.BeginUpdate();

            listBoxLog.Items.Clear();
            foreach (var item in listItems)
            {
                bool displayLine = false;
                switch (item.Level)
                {
                    case LogLevel.Info:
                        displayLine = checkBoxShowInfos.Checked;
                        break;

                    case LogLevel.Warning:
                        displayLine = checkBoxShowWarnings.Checked;
                        break;

                    case LogLevel.Error:
                        displayLine = checkBoxShowErrors.Checked;
                        break;
                }

                if (displayLine)
                {
                    listBoxLog.Items.Add(item);
                }
            }

            listBoxLog.EndUpdate();

            //Force scroll to bottom
            if (listBoxLog.Items.Count > 0)
            {
                listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            }
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;

            if (e.Index < 0 || e.Index >= listBox.Items.Count)
                return;

            var g = e.Graphics;
            var item = listBox.Items[e.Index] as LogItem;

            var color = Color.Black;
            switch (item.Level)
            {
                case LogLevel.Info:
                    color = Color.Black;
                    break;

                case LogLevel.Warning:
                    color = Color.Chocolate;
                    break;

                case LogLevel.Error:
                    color = Color.Red;
                    break;
            }

            e.DrawBackground();

            if (e.State.HasFlag(DrawItemState.Selected))
                g.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
            //else
            //    g.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);

            g.DrawString(item.Text, font, new SolidBrush(color), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (EditorCore.MainWindow != null)
                EditorCore.MainWindow.SyncMenuItemFromPanel(this);
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
            listItems.Clear();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (listBoxLog.SelectedItem != null)
            {
                var item = listBoxLog.SelectedItem as LogItem;

                if (EditorCore.MainWindow != null)
                    EditorCore.MainWindow.OpenDocumentDialogue(item.Dialogue, item.Node);
            }
        }

        private void OnCheckDisplayOptions(object sender, EventArgs e)
        {
            RefreshItems();
        }

        private void OnListKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (listBoxLog.SelectedItem != null)
                {
                    var item = listBoxLog.SelectedItem as LogItem;
                    Clipboard.SetText(item.Text);

                    e.Handled = true;
                }
            }
        }
    }
}
