using HttpNamespaceManager.Lib.AccessControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    public partial class AccessControlRightsListBox : UserControl
    {
        private List<AceRights> supportedRights;
        private List<AceType> supportedTypes;
        private AccessControlList acl;
        private SecurityIdentity selectedUser;

        public List<AceRights> SupportedRights
        {
            get { return supportedRights; }
        }

        public List<AceType> SupportedTypes
        {
            get { return supportedTypes; }
        }

        public AccessControlList ACL
        {
            get
            {
                return acl;
            }
            set
            {
                acl = value;
            }
        }

        public SecurityIdentity SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
            }
        }

        public AccessControlRightsListBox()
        {
            InitializeComponent();

            supportedRights = new List<AceRights>();
            supportedTypes = new List<AceType>();
        }

        public void UpdateList()
        {
            tableRights.SuspendLayout();

            tableHeader.Controls.Clear();
            tableHeader.ColumnCount = supportedTypes.Count + 1;
            tableHeader.ColumnStyles.Clear();

            tableRights.Controls.Clear();
            tableRights.ColumnCount = supportedTypes.Count + 1;
            tableRights.RowCount = supportedRights.Count;
            tableRights.RowStyles.Clear();
            tableRights.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableRights.ColumnStyles.Clear();
            tableRights.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var maxsize = 0;
            
            var col = 1;
            foreach (var type in supportedTypes)
            {

                var labelAceType = new Label
                {
                    Name = string.Format("labelAceType{0}", col.ToString()),
                    Text = type.ToString(),
                    TextAlign = ContentAlignment.TopCenter,
                    Dock = DockStyle.Fill
                };
                tableRights.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, labelAceType.Width));
                tableHeader.Controls.Add(labelAceType, col++, 0);
            }

            var row = 0;
            foreach (var right in supportedRights)
            {
                if (selectedUser != null)
                {
                    col = 1;
                    foreach (var type in supportedTypes)
                    {
                        var checkAceType = new CheckBox
                        {
                            Name = string.Format("checkAceType{0}", col.ToString()),
                            Text = "",
                            Size = new Size(15, 14),
                            CheckAlign = ContentAlignment.TopCenter,
                            Dock = DockStyle.Fill,
                            Checked = GetRightValue(type, right),
                            TabStop = true
                        };
                        var checkedHandler = new CheckAceTypeCheckedHandler(this, type, right);
                        checkAceType.CheckedChanged += new EventHandler(checkedHandler.CheckAceType_Checked);
                        tableRights.Controls.Add(checkAceType, col++, row);
                    }
                }

                var labelRight = new Label
                {
                    Name = string.Format("labelRight{0}", row.ToString()),
                    Text = right.ToString(),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                maxsize = Math.Max(maxsize, labelRight.Width);

                tableRights.Controls.Add(labelRight, 0, row++);
            }

            tableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, maxsize));
            tableHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            tableRights.ResumeLayout(true);
        }

        private class CheckAceTypeCheckedHandler
        {
            private AccessControlRightsListBox parent;
            private readonly AceType aceType;
            private readonly AceRights aceRight;

            public CheckAceTypeCheckedHandler(AccessControlRightsListBox parent, AceType aceType, AceRights aceRight)
            {
                this.parent = parent;
                this.aceType = aceType;
                this.aceRight = aceRight;
            }

            public void CheckAceType_Checked(object sender, EventArgs e)
            {
                parent.HandleCheckBoxClick((CheckBox)sender, aceType, aceRight);
            }
        }

        private bool GetRightValue(AceType aceType, AceRights aceRight)
        {
            foreach (var ace in acl)
            {
                if (ace.AceType == aceType && ace.AccountSID == selectedUser)
                {
                    foreach (var right in ace)
                    {
                        if (right == aceRight) return true;
                    }
                }
            }
            return false;
        }

        private void HandleCheckBoxClick(CheckBox source, AceType aceType, AceRights aceRight)
        {
            foreach (var ace in acl)
            {
                if (ace.AceType == aceType && ace.AccountSID == selectedUser)
                {
                    if (source.Checked)
                    {
                        ace.Add(aceRight);
                    }
                    else
                    {
                        ace.Remove(aceRight);
                    }
                    return;
                }
            }

            // The ace type doesn't exist

            var newAce = new AccessControlEntry(selectedUser)
            {
                AceType = aceType
            };
            newAce.Add(aceRight);
            acl.Add(newAce);
        }
    }
}
