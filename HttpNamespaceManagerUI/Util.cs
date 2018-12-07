using System;
using System.Runtime.InteropServices;

namespace HttpNamespaceManager.UI
{
    internal static class Util
    {
        [DllImport("shell32.dll")]
        internal static extern bool IsUserAnAdmin();

        internal static string GetErrorMessage(uint errorCode)
        {
            uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            var dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

            var source = new IntPtr();

            var msgBuffer = "";

            var retVal = FormatMessage(dwFlags, source, errorCode, 0, ref msgBuffer, 512, null);

            return msgBuffer.ToString();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [MarshalAs(UnmanagedType.LPTStr)] ref string lpBuffer, int nSize, IntPtr[] Arguments);


        [DllImport("kernel32.dll")]
        internal static extern uint GetLastError();
    }


    internal enum TOKEN_INFORMATION_CLASS
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId,
        TokenGroupsAndPrivileges,
        TokenSessionReference,
        TokenSandBoxInert,
        TokenAuditPolicy,
        TokenOrigin
    }

    internal enum ShowCommand
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }
}
