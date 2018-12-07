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

        private void UsageForm_Load(object sender, EventArgs e)
        {
            Size = new Size(labelUsage.Right + 18, labelUsage.Bottom + 62);
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}