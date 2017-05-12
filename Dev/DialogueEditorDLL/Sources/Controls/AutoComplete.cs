using System;
using System.Drawing;
using System.Windows.Forms;

namespace DialogueEditor
{
    public class AutoComplete
    {
        //--------------------------------------------------------------------------------------------------------------
        // Delegates

        public delegate void DelegateValidate();
        public delegate void DelegateClose();
        public delegate string DelegateDrawItem(object item);

        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public DelegateValidate OnValidate;
        public DelegateClose OnClose;
        public DelegateDrawItem OnDrawItem;

        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected Form form;
        protected ListBox listBoxAutoComplete;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public AutoComplete(Control parent, BindingSource source)
        {
            // InitializeComponent
            form = new Form();
            //form.Size = new System.Drawing.Size(200, 200);
            form.Visible = false;
            form.ShowInTaskbar = false;
            form.ControlBox = false;
            form.TopMost = true;

            //form.TopLevel = false;
            //ProjectController.MainWindow.Controls.Add(form);

            listBoxAutoComplete = new ListBox();
            listBoxAutoComplete.BorderStyle = BorderStyle.FixedSingle;
            //listBoxAutoComplete.ImageList = this.imageList1;
            //listBoxAutoComplete.Name = "listBoxAutoComplete";
            //listBoxAutoComplete.Size = new System.Drawing.Size(170, 170);
            //listBoxAutoComplete.Visible = false;
            listBoxAutoComplete.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            listBoxAutoComplete.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxAutoComplete.IntegralHeight = false;
            listBoxAutoComplete.DrawItem += new DrawItemEventHandler(OnListBoxAutoCompleteDrawItem);
            listBoxAutoComplete.KeyDown += new KeyEventHandler(OnListBoxAutoCompleteKeyDown);
            listBoxAutoComplete.DoubleClick += new EventHandler(OnListBoxAutoCompleteDoubleClick);
            listBoxAutoComplete.LostFocus += new EventHandler(OnListBoxAutoCompleteLostFocus);
            //listBoxAutoComplete.SelectedIndexChanged += new System.EventHandler(this.listBoxAutoComplete_SelectedIndexChanged);

            //parent.Controls.Add(listBoxAutoComplete);
            form.Controls.Add(listBoxAutoComplete);

            // Size & Position
            listBoxAutoComplete.Dock = DockStyle.Fill; //Use left-bottom-right when adding a textbox for search
            form.Size = new Size(240, 320);

            // Data
            listBoxAutoComplete.DataSource = source;
        }

        public void Dispose()
        {
            if (form != null)
            {
                form.Dispose();
                form = null;
            }
        }

        public void Open(RichTextBox textBox)
        {
            Point point = textBox.GetPositionFromCharIndex(textBox.SelectionStart);

            //point.Offset(textBox.Location);
            //point.Offset(textBox.TopLevelControl.Location);

            Control parent = textBox;
            while (parent != null)
            {
                point.Offset(parent.Location);
                parent = parent.Parent;
            }

            point.Y += 50;
            //point.Y += (int)Math.Ceiling(textBox.Font.GetHeight()) + 2;
            point.X = Math.Min(point.X, ProjectController.MainWindow.Left + ProjectController.MainWindow.Width - listBoxAutoComplete.Size.Width - 50);

            form.Show();
            form.BringToFront();
            form.Location = point;

            listBoxAutoComplete.Focus();
        }

        public bool Close()
        {
            if (form.Visible)
            {
                form.Hide();
                return true;
            }
            return false;
        }

        public object GetSelectedItem()
        {
            return listBoxAutoComplete.SelectedItem;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnListBoxAutoCompleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (OnValidate != null)
                    OnValidate();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            //else if (e.KeyCode == Keys.OemQuotes)   // ²
            else if (e.KeyCode == Keys.Tab)
            {
                if (OnClose != null)
                    OnClose();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void OnListBoxAutoCompleteDoubleClick(object sender, EventArgs e)
        {
            if (OnValidate != null)
                OnValidate();
        }

        private void OnListBoxAutoCompleteLostFocus(object sender, EventArgs e)
        {
            if (OnClose != null)
                OnClose();
        }

        private void OnListBoxAutoCompleteDrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;

            if (e.Index < 0 || e.Index >= listBox.Items.Count)
                return;

            string text = "";
            if (OnDrawItem != null)
                text = OnDrawItem(listBox.Items[e.Index]);

            var g = e.Graphics;
            var color = Color.Black;

            e.DrawBackground();

            if (e.State.HasFlag(DrawItemState.Selected))
                g.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
            //else
            //    g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            g.DrawString(text, listBox.Font, new SolidBrush(color), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }
    }
}
