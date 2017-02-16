using System;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class WIN32
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        static public void StopRedraw(Form form)
        {
            SendMessage(form.Handle, WM_SETREDRAW, false, 0);
        }

        static public void ResumeRedraw(Form form)
        {
            SendMessage(form.Handle, WM_SETREDRAW, true, 0);
        }

        public static void SetDoubleBuffered(Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo property =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            property.SetValue(c, true, null);
        }

        static public void ShowCrashMessage(object sender, System.Threading.ThreadExceptionEventArgs t)
        {
            string errorMsg = "An crash occurred...\nPlease send a screenshot of this message to the devs !\n\n";
            errorMsg = errorMsg + t.Exception.Message;
            errorMsg = errorMsg + "\n\nStack Trace:\n";
            errorMsg = errorMsg + t.Exception.StackTrace;
            MessageBox.Show(errorMsg, "Dialogue Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            Application.Exit();
        }
    }
}
