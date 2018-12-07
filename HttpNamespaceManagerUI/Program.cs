using System;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                Application.Run(new MainForm());
            }
            else if (args.Length == 2 && args[0].StartsWith("-"))
            {
                if (Enum.TryParse(args[0].TrimStart('-'), true, out NamespaceManagerAction action))
                    Application.Run(new MainForm(action, args[1]));
            }
            else
            {
                Application.Run(new UsageForm());
            }
        }
    }
}