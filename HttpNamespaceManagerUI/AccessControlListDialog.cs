using HttpNamespaceManager.Lib.AccessControl;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HttpNamespaceManager.UI
{
    public partial class AccessControlListDialog : Form
    {
        private string objectName;
        private AccessControlList acl;
        private List<SecurityIdentity> userList;

        public string ObjectName
        {
            get { return objectName; }
        }

        public AccessControlList ACL
        {
            get { return acl; }
        }

        public AccessControlListDialog()
        {
            InitializeComponent();
        }

        public AccessControlListDialog(string objectName, AccessControlList acl, List<AceRights> supportedRights, List<AceType> supportedTypes)
        {
            InitializeComponent();
            this.objectName = objectName ?? throw new ArgumentNullException("objectName");
            this.acl = acl ?? throw new ArgumentNullException("acl");
            labelObjectName.Text = objectName;

            userList = new List<SecurityIdentity>();

            foreach (var ace in this.acl)
            {
                if (!userList.Contains(ace.AccountSID))
                {
                    userList.Add(ace.AccountSID);
                }
            }

            AclListPermissions.SupportedRights.AddRange(supportedRights);
            AclListPermissions.SupportedTypes.AddRange(supportedTypes);
            AclListPermissions.ACL = this.acl;
        }

        private void AccessControlListDialog_Load(object sender, EventArgs e)
        {
            if (userList != null)
            {
                foreach (var sid in userList)
                {
                    ListUsersAndGroups.Items.Add(sid);
                }

                ListUsersAndGroups.DisplayMember = "Name";

                if (ListUsersAndGroups.Items.Count > 0) ListUsersAndGroups.SelectedIndex = 0;
                else
                {
                    AclListPermissions.UpdateList();
                }
            }
        }

        private void ListUsersAndGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            AclListPermissions.SelectedUser = (SecurityIdentity)ListUsersAndGroups.SelectedItem;

            AclListPermissions.UpdateList();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (InputBox.Show("Add User or Group", "Enter the User or Group Name:", out var accountName) == DialogResult.OK)
            {
                try
                {
                    var sid = SecurityIdentity.SecurityIdentityFromName(accountName);

                    if (MessageBox.Show(string.Format("Add user or group: {0}?", sid.Name), "User or Group Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!userList.Contains(sid))
                        {
                            var ace = new AccessControlEntry(sid)
                            {
                                AceType = AceType.AccessAllowed
                            };
                            acl.Add(ace);
                            userList.Add(sid);
                            ListUsersAndGroups.Items.Add(sid);

                            ListUsersAndGroups.SelectedItem = sid;
                        }
                        else
                        {
                            MessageBox.Show("The selected user or group already exists.", "Duplicate User or Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User or group name was not found. " + ex.Message, "User or Group Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (ListUsersAndGroups.SelectedItem != null)
            {
                var sid = (SecurityIdentity)ListUsersAndGroups.SelectedItem;

                var toDelete = new List<AccessControlEntry>();

                foreach (var ace in acl)
                {
                    if (ace.AccountSID == sid)
                    {
                        toDelete.Add(ace);
                    }
                }

                foreach (var deleteAce in toDelete)
                {
                    acl.Remove(deleteAce);
                }

                userList.Remove(sid);

                ListUsersAndGroups.Items.Remove(sid);
            }
        }
    }
}