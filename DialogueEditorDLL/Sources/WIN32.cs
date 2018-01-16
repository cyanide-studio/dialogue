using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DialogueEditor
{
    public static class WIN32
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        private const int WM_SETREDRAW = 11;

        public static void StopRedraw(Form form)
        {
            SendMessage(form.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeRedraw(Form form)
        {
            SendMessage(form.Handle, WM_SETREDRAW, true, 0);
        }

        public static void SetDoubleBuffered(Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (SystemInformation.TerminalServerSession)
                return;

            PropertyInfo property =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance);

            property.SetValue(c, true, null);
        }

        public static void ShowCrashMessage(object sender, ThreadExceptionEventArgs t)
        {
            string errorMsg = "A crash occurred...\nPlease send a screenshot of this message to the devs !\n\n";
            errorMsg = errorMsg + t.Exception.Message;
            errorMsg = errorMsg + "\n\nStack Trace:\n";
            errorMsg = errorMsg + t.Exception.StackTrace;
            MessageBox.Show(errorMsg, "Dialogue Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            Application.Exit();
        }
    }
}
