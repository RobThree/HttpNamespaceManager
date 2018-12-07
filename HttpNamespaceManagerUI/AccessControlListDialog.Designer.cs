namespace HttpNamespaceManager.UI
{
    partial class AccessControlListDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessControlListDialog));
            this.labelObjectNameLabel = new System.Windows.Forms.Label();
            this.labelObjectName = new System.Windows.Forms.Label();
            this.labelUsersAndGroups = new System.Windows.Forms.Label();
            this.ListUsersAndGroups = new System.Windows.Forms.ListBox();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.AclListPermissions = new HttpNamespaceManager.UI.AccessControlRightsListBox();
            this.SuspendLayout();
            // 
            // labelObjectNameLabel
            // 
            this.labelObjectNameLabel.AutoSize = true;
            this.labelObjectNameLabel.Location = new System.Drawing.Point(12, 9);
            this.labelObjectNameLabel.Name = "labelObjectNameLabel";
            this.labelObjectNameLabel.Size = new System.Drawing.Size(70, 13);
            this.labelObjectNameLabel.TabIndex = 0;
            this.labelObjectNameLabel.Text = "Object name:";
            // 
            // labelObjectName
            // 
            this.labelObjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelObjectName.AutoEllipsis = true;
            this.labelObjectName.Location = new System.Drawing.Point(88, 9);
            this.labelObjectName.Name = "labelObjectName";
            this.labelObjectName.Size = new System.Drawing.Size(286, 23);
            this.labelObjectName.TabIndex = 1;
            this.labelObjectName.Text = "name";
            // 
            // labelUsersAndGroups
            // 
            this.labelUsersAndGroups.AutoSize = true;
            this.labelUsersAndGroups.Location = new System.Drawing.Point(12, 37);
            this.labelUsersAndGroups.Name = "labelUsersAndGroups";
            this.labelUsersAndGroups.Size = new System.Drawing.Size(109, 13);
            this.labelUsersAndGroups.TabIndex = 2;
            this.labelUsersAndGroups.Text = "Group or User Names";
            // 
            // ListUsersAndGroups
            // 
            this.ListUsersAndGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListUsersAndGroups.FormattingEnabled = true;
            this.ListUsersAndGroups.IntegralHeight = false;
            this.ListUsersAndGroups.Location = new System.Drawing.Point(12, 53);
            this.ListUsersAndGroups.Name = "ListUsersAndGroups";
            this.ListUsersAndGroups.Size = new System.Drawing.Size(362, 267);
            this.ListUsersAndGroups.TabIndex = 3;
            this.ListUsersAndGroups.SelectedIndexChanged += new System.EventHandler(this.ListUsersAndGroups_SelectedIndexChanged);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRemove.Location = new System.Drawing.Point(299, 326);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(75, 23);
            this.ButtonRemove.TabIndex = 5;
            this.ButtonRemove.Text = "Remove";
            this.ButtonRemove.UseVisualStyleBackColor = true;
            this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAdd.Location = new System.Drawing.Point(218, 326);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(75, 23);
            this.ButtonAdd.TabIndex = 4;
            this.ButtonAdd.Text = "Add...";
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(299, 513);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 8;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(218, 513);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 7;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // AclListPermissions
            // 
            this.AclListPermissions.ACL = null;
            this.AclListPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AclListPermissions.BackColor = System.Drawing.SystemColors.Control;
            this.AclListPermissions.Location = new System.Drawing.Point(12, 364);
            this.AclListPermissions.Name = "AclListPermissions";
            this.AclListPermissions.SelectedUser = null;
            this.AclListPermissions.Size = new System.Drawing.Size(362, 143);
            this.AclListPermissions.TabIndex = 6;
            // 
            // AccessControlListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 548);
            this.Controls.Add(this.AclListPermissions);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.ButtonRemove);
            this.Controls.Add(this.ListUsersAndGroups);
            this.Controls.Add(this.labelUsersAndGroups);
            this.Controls.Add(this.labelObjectName);
            this.Controls.Add(this.labelObjectNameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 413);
            this.Name = "AccessControlListDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Permissions";
            this.Load += new System.EventHandler(this.AccessControlListDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelObjectNameLabel;
        private System.Windows.Forms.Label labelObjectName;
        private System.Windows.Forms.Label labelUsersAndGroups;
        private System.Windows.Forms.ListBox ListUsersAndGroups;
        private System.Windows.Forms.Button ButtonRemove;
        private System.Windows.Forms.Button ButtonAdd;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private AccessControlRightsListBox AclListPermissions;
    }
}