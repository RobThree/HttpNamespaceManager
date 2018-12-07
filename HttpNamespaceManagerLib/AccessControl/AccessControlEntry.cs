using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpNamespaceManager.Lib.AccessControl
{
    public class AccessControlEntry : ICollection<AceRights>
    {
        private AceType aceType = AceType.AccessAllowed;
        private AceFlags flags = AceFlags.None;
        private AceRights rights = AceRights.None;
        private Guid objectGuid = Guid.Empty;
        private Guid inheritObjectGuid = Guid.Empty;
        private SecurityIdentity accountSID;

        /// <summary>
        /// Gets or Sets the Access Control Entry Type
        /// </summary>
        public AceType AceType
        {
            get { return aceType; }
            set { aceType = value; }
        }

        /// <summary>
        /// Gets or Sets the Access Control Entry Flags
        /// </summary>
        public AceFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        /// <summary>
        /// Gets or Sets the Access Control Entry Rights
        /// </summary>
        /// <remarks>This is a binary flag value, and can be more easily 
        /// accessed via the Access Control Entry collection methods.</remarks>
        public AceRights Rights
        {
            get { return rights; }
            set { rights = value; }
        }

        /// <summary>
        /// Gets or Sets the Object Guid
        /// </summary>
        public Guid ObjectGuid
        {
            get { return objectGuid; }
            set { objectGuid = value; }
        }

        /// <summary>
        /// Gets or Sets the Inherit Object Guid
        /// </summary>
        public Guid InheritObjectGuid
        {
            get { return inheritObjectGuid; }
            set { inheritObjectGuid = value; }
        }

        /// <summary>
        /// Gets or Sets the Account SID
        /// </summary>
        public SecurityIdentity AccountSID
        {
            get { return accountSID; }
            set { accountSID = value; }
        }

        private AccessControlEntry()
        {
            // Do Nothing
        }

        public AccessControlEntry(SecurityIdentity account)
        {
            accountSID = account;
        }

        public AccessControlEntry(AccessControlEntry original)
        {
            accountSID = original.accountSID;
            aceType = original.aceType;
            flags = original.flags;
            inheritObjectGuid = original.inheritObjectGuid;
            objectGuid = original.objectGuid;
            rights = original.rights;
        }

        /// <summary>
        /// Renders the Access Control Entry as an SDDL ACE string
        /// </summary>
        /// <returns>An SDDL ACE string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0};", aceTypeStrings[(int)aceType]);

            for (var flag = 0x01; flag <= (int)AceFlags.AuditFailure; flag = flag << 1)
            {
                if ((flag & (int)flags) == flag) sb.Append(aceFlagStrings[(int)Math.Log(flag, 2.0d)]);
            }

            sb.Append(';');

            foreach (var right in this)
            {
                sb.Append(rightsStrings[(int)Math.Log((int)right, 2.0d)]);
            }

            sb.Append(';');

            sb.AppendFormat("{0};", objectGuid != Guid.Empty ? objectGuid.ToString() : "");

            sb.AppendFormat("{0};", inheritObjectGuid != Guid.Empty ? inheritObjectGuid.ToString() : "");

            if (accountSID != null) sb.Append(accountSID.ToString());

            return sb.ToString();
        }

        private static readonly string [] aceTypeStrings = new string [] { "A", "D", "OA", "OD", "AU", "AL", "OU", "OL" };
        private static readonly string [] aceFlagStrings = new string [] { "CI", "OI", "NP", "IO", "ID", "SA", "FA" };
        private static readonly string [] rightsStrings = new string [] {
            "GA",
            "GR",
            "GW",
            "GX",
            "RC",
            "SD",
            "WD",
            "WO",
            "RP",
            "WP",
            "CC",
            "DC",
            "LC",
            "SW",
            "LO",
            "DT",
            "CR",
            "FA",
            "FR",
            "FW",
            "FX",
            "KA",
            "KR",
            "KW",
            "KX"
        };

        private const string aceExpr = @"^(?'ace_type'[A-Z]+)?;(?'ace_flags'([A-Z]{2})+)?;(?'rights'([A-Z]{2})+|0x[0-9A-Fa-f]+)?;(?'object_guid'[0-9A-Fa-f\-]+)?;(?'inherit_object_guid'[0-9A-Fa-f\-]+)?;(?'account_sid'[A-Z]+?|S(-[0-9]+)+)?$";

        public static AccessControlEntry AccessControlEntryFromString(string aceString)
        {
            var aceRegex = new Regex(aceExpr, RegexOptions.IgnoreCase);

            var aceMatch = aceRegex.Match(aceString);

            if (!aceMatch.Success) throw new FormatException("Invalid ACE String Format");

            var ace = new AccessControlEntry();

            if (aceMatch.Groups["ace_type"] != null && aceMatch.Groups["ace_type"].Success && !string.IsNullOrEmpty(aceMatch.Groups["ace_type"].Value))
            {
                var aceTypeValue = Array.IndexOf<string>(aceTypeStrings, aceMatch.Groups["ace_type"].Value.ToUpper());

                if (aceTypeValue == -1) throw new FormatException("Invalid ACE String Format");

                ace.aceType = (AceType)aceTypeValue;
            }
            else throw new FormatException("Invalid ACE String Format");

            if (aceMatch.Groups["ace_flags"] != null && aceMatch.Groups["ace_flags"].Success && !string.IsNullOrEmpty(aceMatch.Groups["ace_flags"].Value))
            {
                var aceFlagsValue = aceMatch.Groups["ace_flags"].Value.ToUpper();
                for (var i = 0; i < aceFlagsValue.Length - 1; i += 2)
                {
                    var flagValue = Array.IndexOf<string>(aceFlagStrings, aceFlagsValue.Substring(i, 2));

                    if (flagValue == -1) throw new FormatException("Invalid ACE String Format");

                    ace.flags = ace.flags | ((AceFlags)(int)Math.Pow(2.0d, flagValue));
                }
            }

            if (aceMatch.Groups["rights"] != null && aceMatch.Groups["rights"].Success && !string.IsNullOrEmpty(aceMatch.Groups["rights"].Value))
            {
                var rightsValue = aceMatch.Groups["rights"].Value.ToUpper();
                for (var i = 0; i < rightsValue.Length - 1; i += 2)
                {
                    var rightValue = Array.IndexOf<string>(rightsStrings, rightsValue.Substring(i, 2));

                    if (rightValue == -1) throw new FormatException("Invalid ACE String Format");

                    ace.Add((AceRights)(int)Math.Pow(2.0d, rightValue));
                }
            }

            if (aceMatch.Groups["object_guid"] != null && aceMatch.Groups["object_guid"].Success && !string.IsNullOrEmpty(aceMatch.Groups["object_guid"].Value))
            {
                ace.objectGuid = new Guid(aceMatch.Groups["object_guid"].Value);
            }

            if (aceMatch.Groups["inherit_object_guid"] != null && aceMatch.Groups["inherit_object_guid"].Success && !string.IsNullOrEmpty(aceMatch.Groups["inherit_object_guid"].Value))
            {
                ace.inheritObjectGuid = new Guid(aceMatch.Groups["inherit_object_guid"].Value);
            }

            if (aceMatch.Groups["account_sid"] != null && aceMatch.Groups["account_sid"].Success && !string.IsNullOrEmpty(aceMatch.Groups["account_sid"].Value))
            {
                ace.accountSID = SecurityIdentity.SecurityIdentityFromString(aceMatch.Groups["account_sid"].Value.ToUpper());
            }
            else throw new FormatException("Invalid ACE String Format");

            return ace;
        }

        #region Rights Collection Members

        public void Add(AceRights item)
        {
            rights = rights | item;
        }

        public void Clear()
        {
            rights = AceRights.None;
        }

        public bool Contains(AceRights item)
        {
            return item == (item & rights);
        }

        public void CopyTo(AceRights[] array, int arrayIndex)
        {
            foreach (var right in this)
            {
                array[arrayIndex++] = right;
            }
        }

        public int Count
        {
            get
            {
                var count = 0;
                for (var col = (int)rights; col != 0; col = col >> 1) count += ((col & 1) == 1) ? 1 : 0;
                return count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(AceRights item)
        {
            if (Contains(item))
            {
                rights = rights & ~item;
                return true;
            }
            else return false;
        }

        public IEnumerator<AceRights> GetEnumerator()
        {
            var current = (int)AceRights.GenericAll;
            for (var col = (int)rights; col != 0; col = col >> 1, current = current << 1)
            {
                while (col != 0 && (col & 1) != 1)
                {
                    col = col >> 1;
                    current = current << 1;
                }

                if ((col & 1) == 1)
                {
                    yield return (AceRights)current;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)GetEnumerator();
        }

        #endregion
    }

    public enum AceType
    {
        AccessAllowed = 0,
        AccessDenied,
        ObjectAccessAllowed,
        ObjectAccessDenied,
        Audit,
        Alarm,
        ObjectAudit,
        ObjectAlarm
    }

    [Flags]
    public enum AceFlags
    {
        None = 0x0000,
        ContainerInherit = 0x0001,
        ObjectInherit = 0x0002,
        NoPropogate = 0x0004,
        InheritOnly = 0x0008,
        Inherited = 0x0010,
        AuditSuccess = 0x0020,
        AuditFailure = 0x0040
    }

    [Flags]
    public enum AceRights
    {
        None = 0x00000000,
        GenericAll = 0x00000001,
        GenericRead = 0x00000002,
        GenericWrite = 0x00000004,
        GenericExecute = 0x00000008,
        StandardReadControl = 0x00000010,
        StandardDelete = 0x00000020,
        StandardWriteDAC = 0x00000040,
        StandardWriteOwner = 0x00000080,
        DirectoryReadProperty = 0x00000100,
        DirectoryWriteProperty = 0x00000200,
        DirectoryCreateChild = 0x00000400,
        DirectoryDeleteChild = 0x00000800,
        DirectoryListChildren = 0x00001000,
        DirectorySelfWrite = 0x00002000,
        DirectoryListObject = 0x00004000,
        DirectoryDeleteTree = 0x00008000,
        DirectoryControlAccess = 0x00010000,
        FileAll = 0x00020000,
        FileRead = 0x00040000,
        FileWrite = 0x00080000,
        FileExecute = 0x00100000,
        KeyAll = 0x00200000,
        KeyRead = 0x00400000,
        KeyWrite = 0x00800000,
        KeyExecute = 0x01000000
    }
}
