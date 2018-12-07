using System;
using System.Drawing;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public InputBox(string title, string prompt)
        {
            InitializeComponent();
            Text = title;
            labelPrompt.Text = prompt;
        }

        public static DialogResult Show(string title, string prompt, out string result)
        {
            var input = new InputBox(title, prompt);
            var retval = input.ShowDialog();
            result = input.textInput.Text;
            return retval;
        }
    }
}