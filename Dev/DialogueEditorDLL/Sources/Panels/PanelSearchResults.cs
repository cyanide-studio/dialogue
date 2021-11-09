using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public partial class PanelSearchResults : DockContent
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        protected class LogItem
        {
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
        private string labelResultsCountText;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public PanelSearchResults()
        {
            InitializeComponent();

            font = listBoxLog.Font;
            listBoxLog.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxLog.DrawItem += listBox_DrawItem;

            labelResultsCountText = labelResultsCount.Text;

            RefreshResultsCount();
        }

        public void RefreshResultsCount()
        {
            labelResultsCount.Text = String.Format(labelResultsCountText, listItems.Count);
        }

        public void Clear()
        {
            listBoxLog.Items.Clear();
            listItems.Clear();

            RefreshResultsCount();
        }

        public void WriteStartSearch()
        {
            Clear();
        }

        public void WriteEndSearch()
        {
            RefreshResultsCount();
        }

        public void WriteLine(string message, string dialogue, int node)
        {
            LogItem item = new LogItem();

            StringBuilder stringBuilder = new StringBuilder();
            bool displayLine = true;
            bool showPanel = true;
            
            stringBuilder.Append(message);
            
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

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;

            if (e.Index < 0 || e.Index >= listBox.Items.Count)
                return;

            var g = e.Graphics;
            var item = listBox.Items[e.Index] as LogItem;

            var color = Color.Black;

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
            EditorCore.MainWindow.SyncMenuItemFromPanel(this);
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            Clear();
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

        private void OnListKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (listBoxLog.SelectedItem != null)
                {
                    var item = listBoxLog.SelectedItem as LogItem;
                    Clipboard.SetText(item.Text);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }
    }
}
