using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                Application.Run(new MainForm());
            }
            else if (args.Length == 2)
            {
                var actionRegex = new Regex(@"(-|--|/)(?'action'ad?d?|ed?i?t?|re?m?o?v?e?)", RegexOptions.IgnoreCase);
                var m = actionRegex.Match(args[0]);
                if (m.Success && m.Groups["action"] != null && m.Groups["action"].Success && !string.IsNullOrEmpty(m.Groups["action"].Value))
                {
                    var action = m.Groups["action"].Value.ToLower();
                    if (action.StartsWith("a"))
                    {
                        Application.Run(new MainForm(NamespaceManagerAction.Add, args[1]));
                    }
                    else if (action.StartsWith("e"))
                    {
                        Application.Run(new MainForm(NamespaceManagerAction.Edit, args[1]));
                    }
                    else
                    {
                        Application.Run(new MainForm(NamespaceManagerAction.Remove, args[1]));
                    }
                }
                else
                {
                    Application.Run(new UsageForm());
                }
            }
            else
            {
                Application.Run(new UsageForm());
            }
        }
    }
}