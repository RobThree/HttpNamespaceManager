using System;
using System.Runtime.InteropServices;

namespace HttpNamespaceManager.Lib.AccessControl
{
    internal static class Util
    {
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

        /*
         * DWORD GetLastError(void);
         */
        [DllImport("kernel32.dll")]
        internal static extern uint GetLastError();

        /*
         * HLOCAL LocalAlloc(
         *     UINT uFlags,
         *     SIZE_T uBytes
         * );
         */
        [DllImport("Kernel32.dll")]
        internal static extern IntPtr LocalAlloc(LocalAllocFlags uFlags, uint uBytes);

        /*
         * HLOCAL LocalFree(
         *     HLOCAL hMem
         * );
         */
        [DllImport("Kernel32.dll")]
        internal static extern IntPtr LocalFree(IntPtr hMem);
    }

    [Flags]
    internal enum LocalAllocFlags
    {
        Fixed = 0x00,
        Moveable = 0x20,
        ZeroInit = 0x40
    }
}
