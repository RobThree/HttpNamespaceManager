using HttpNamespaceManager.Lib;
using HttpNamespaceManager.Lib.AccessControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    public partial class MainForm : Form
    {
        private HttpApi nsManager;
        private Dictionary<string, SecurityDescriptor> nsTable;

        private NamespaceManagerAction action = NamespaceManagerAction.None;
        private string initialUrl = null;

        public MainForm()
        {
            nsManager = new HttpApi();
            InitializeComponent();
        }

        public MainForm(NamespaceManagerAction action, string url)
        {
            nsManager = new HttpApi();
            InitializeComponent();

            this.action = action;
            initialUrl = url;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            nsTable = nsManager.QueryHttpNamespaceAcls();

            ListHttpNamespaces.Items.AddRange(nsTable.Keys.OrderBy(k => k).ToArray());

            if (initialUrl != null) ListHttpNamespaces.SelectedItem = initialUrl;
            else ListHttpNamespaces.SelectedIndex = 0;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (action != NamespaceManagerAction.None)
            {
                switch (action)
                {
                    case NamespaceManagerAction.Add:
                        ButtonAdd_Click(this, new EventArgs());
                        break;
                    case NamespaceManagerAction.Edit:
                        ButtonEdit_Click(this, new EventArgs());
                        break;
                    case NamespaceManagerAction.Remove:
                        ButtonRemove_Click(this, new EventArgs());
                        break;
                }
            }
        }

        private void Elevate(NamespaceManagerAction action, string url)
        {
            if (!Util.IsUserAnAdmin())
            {
                var procInfo = new ProcessStartInfo(Application.ExecutablePath, string.Format("-{0} {1}", action.ToString(), url))
                {
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Normal
                };

                Process.Start(procInfo);

                Application.Exit();
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)ListHttpNamespaces.SelectedItem))
            {
                Elevate(NamespaceManagerAction.Add, (string)ListHttpNamespaces.SelectedItem);


                InputBox.Show("Enter URL", "Enter the URL to add:", out var url);

                if (!string.IsNullOrEmpty(url))
                {
                    var newSd = new SecurityDescriptor
                    {
                        DACL = new AccessControlList()
                    };

                    var aclDlg = new AccessControlListDialog(url,
                        newSd.DACL,
                        new List<AceRights>(new AceRights[] { AceRights.GenericAll, AceRights.GenericExecute, AceRights.GenericRead, AceRights.GenericWrite }),
                        new List<AceType>(new AceType[] { AceType.AccessAllowed }));

                    if (aclDlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            nsManager.SetHttpNamespaceAcl(url, newSd);
                            nsTable.Add(url, newSd);
                            ListHttpNamespaces.Items.Add(url);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Adding ACL. " + ex.Message, "Error Adding ACL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            DoEdit();
        }


        private void DoEdit()
        {
            if (!string.IsNullOrEmpty((string)ListHttpNamespaces.SelectedItem))
            {
                Elevate(NamespaceManagerAction.Edit, (string)ListHttpNamespaces.SelectedItem);

                var original = nsTable[(string)ListHttpNamespaces.SelectedItem].DACL;

                var aclDialog = new AccessControlListDialog((string)ListHttpNamespaces.SelectedItem,
                    original != null ? new AccessControlList(original) : new AccessControlList(),
                    new List<AceRights>(new AceRights[] { AceRights.GenericAll, AceRights.GenericExecute, AceRights.GenericRead, AceRights.GenericWrite }),
                    new List<AceType>(new AceType[] { AceType.AccessAllowed }));
                if (aclDialog.ShowDialog() == DialogResult.OK)
                {
                    var removed = false;
                    try
                    {
                        nsManager.RemoveHttpHamespaceAcl((string)ListHttpNamespaces.SelectedItem);
                        removed = true;
                        nsTable[(string)ListHttpNamespaces.SelectedItem].DACL = aclDialog.ACL;
                        nsManager.SetHttpNamespaceAcl((string)ListHttpNamespaces.SelectedItem, nsTable[(string)ListHttpNamespaces.SelectedItem]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Setting ACL. " + ex.Message, "Error Setting ACL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (removed)
                        {
                            try
                            {
                                nsTable[(string)ListHttpNamespaces.SelectedItem].DACL = original;
                                nsManager.SetHttpNamespaceAcl((string)ListHttpNamespaces.SelectedItem, nsTable[(string)ListHttpNamespaces.SelectedItem]);
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show("Unable to Restore Original ACL, ACL may be corrupt. " + ex2.Message, "Error Retoring ACL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)ListHttpNamespaces.SelectedItem))
            {
                Elevate(NamespaceManagerAction.Remove, (string)ListHttpNamespaces.SelectedItem);

                try
                {
                    nsManager.RemoveHttpHamespaceAcl((string)ListHttpNamespaces.SelectedItem);
                    nsTable.Remove((string)ListHttpNamespaces.SelectedItem);
                    ListHttpNamespaces.Items.Remove((string)ListHttpNamespaces.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Removing ACL. " + ex.Message, "Error Removing ACL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (nsManager != null) nsManager.Dispose();
        }

        private void ListHttpNamespaces_DoubleClick(object sender, EventArgs e)
        {
            DoEdit();
        }
    }

    public enum NamespaceManagerAction
    {
        None,
        Add,
        Edit,
        Remove
    }
}