using System;
using System.Drawing;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    public partial class UsageForm : Form
    {
        public UsageForm()
        {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}