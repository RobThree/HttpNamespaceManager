using HttpNamespaceManager.Lib.AccessControl;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HttpNamespaceManager.Lib
{
    public class HttpApi : IDisposable
    {
        public HttpApi()
        {
            var version = new HTTPAPI_VERSION
            {
                HttpApiMajorVersion = 1,
                HttpApiMinorVersion = 0
            };

            HttpInitialize(version, HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
        }

        ~HttpApi()
        {
            Dispose(false);
        }

        protected void Dispose(bool p)
        {
            HttpTerminate(HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public Dictionary<string, SecurityDescriptor> QueryHttpNamespaceAcls()
        {
            var nsTable = new Dictionary<string, SecurityDescriptor>();

            var query = new HTTP_SERVICE_CONFIG_URLACL_QUERY
            {
                QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryNext
            };

            var pQuery = Marshal.AllocHGlobal(Marshal.SizeOf(query));

            try
            {
                var retval = NO_ERROR;
                for (query.dwToken = 0; true; query.dwToken++)
                {
                    Marshal.StructureToPtr(query, pQuery, false);

                    try
                    {
                        uint returnSize = 0;

                        // Get Size
                        retval = HttpQueryServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, pQuery, (uint) Marshal.SizeOf(query), IntPtr.Zero, 0, ref returnSize, IntPtr.Zero);

                        if (retval == ERROR_NO_MORE_ITEMS)
                        {
                            break;
                        }
                        if (retval != ERROR_INSUFFICIENT_BUFFER)
                        {
                            throw new Exception("HttpQueryServiceConfiguration returned unexpected error code.");
                        }

                        var pConfig = Marshal.AllocHGlobal((IntPtr)returnSize);

                        try
                        {
                            retval = HttpQueryServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, pQuery, (uint)Marshal.SizeOf(query), pConfig, returnSize, ref returnSize, IntPtr.Zero);

                            if (retval == NO_ERROR)
                            {
                                var config = (HTTP_SERVICE_CONFIG_URLACL_SET)Marshal.PtrToStructure(pConfig, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));

                                nsTable.Add(config.KeyDesc.pUrlPrefix, SecurityDescriptor.SecurityDescriptorFromString(config.ParamDesc.pStringSecurityDescriptor));
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(pConfig);
                        }
                    }
                    finally
                    {
                        Marshal.DestroyStructure(pQuery, typeof(HTTP_SERVICE_CONFIG_URLACL_QUERY));
                    }
                }

                if (retval != ERROR_NO_MORE_ITEMS)
                {
                    throw new Exception("HttpQueryServiceConfiguration returned unexpected error code.");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pQuery);
            }

            return nsTable;
        }

        public void SetHttpNamespaceAcl(string urlPrefix, SecurityDescriptor acl)
        {
            var urlAclConfig = new HTTP_SERVICE_CONFIG_URLACL_SET();
            urlAclConfig.KeyDesc.pUrlPrefix = urlPrefix;
            urlAclConfig.ParamDesc.pStringSecurityDescriptor = acl.ToString();

            var pUrlAclConfig = Marshal.AllocHGlobal(Marshal.SizeOf(urlAclConfig));

            Marshal.StructureToPtr(urlAclConfig, pUrlAclConfig, false);

            try
            {
                var retval = HttpSetServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, pUrlAclConfig, (uint)Marshal.SizeOf(urlAclConfig), IntPtr.Zero);

                if (retval != 0)
                {
                    throw new ExternalException("Error Setting Configuration: " + Util.GetErrorMessage(retval));
                }
            }
            finally
            {
                if (pUrlAclConfig != IntPtr.Zero)
                {
                    Marshal.DestroyStructure(pUrlAclConfig, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
                    Marshal.FreeHGlobal(pUrlAclConfig); ;
                }
            }
        }

        public void RemoveHttpHamespaceAcl(string urlPrefix)
        {
            var urlAclConfig = new HTTP_SERVICE_CONFIG_URLACL_SET();
            urlAclConfig.KeyDesc.pUrlPrefix = urlPrefix;

            var pUrlAclConfig = Marshal.AllocHGlobal(Marshal.SizeOf(urlAclConfig));

            Marshal.StructureToPtr(urlAclConfig, pUrlAclConfig, false);

            try
            {
                var retval = HttpDeleteServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, pUrlAclConfig, (uint)Marshal.SizeOf(urlAclConfig), IntPtr.Zero);

                if (retval != 0)
                {
                    throw new ExternalException("Error Setting Configuration: " + Util.GetErrorMessage(retval));
                }
            }
            finally
            {
                if (pUrlAclConfig != IntPtr.Zero)
                {
                    Marshal.DestroyStructure(pUrlAclConfig, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
                    Marshal.FreeHGlobal(pUrlAclConfig); ;
                }
            }
        }

        internal const uint ERROR_NO_MORE_ITEMS = 259;
        internal const uint ERROR_INSUFFICIENT_BUFFER = 122;
        internal const uint NO_ERROR = 0;
        internal const uint HTTP_INITIALIZE_CONFIG = 2;

        [DllImport("Httpapi.dll")]
        internal static extern uint HttpInitialize(HTTPAPI_VERSION Version, uint Flags, IntPtr pReserved);

        [DllImport("Httpapi.dll")]
        internal static extern uint HttpTerminate(uint Flags, IntPtr pReserved);

        [DllImport("Httpapi.dll")]
        internal static extern uint HttpSetServiceConfiguration(IntPtr ServiceHandle, HTTP_SERVICE_CONFIG_ID ConfigId, IntPtr pConfigInformation, uint ConfigInformationLength, IntPtr pOverlapped);

        [DllImport("Httpapi.dll")]
        internal static extern uint HttpQueryServiceConfiguration(IntPtr ServiceHandle, HTTP_SERVICE_CONFIG_ID ConfigId, IntPtr pInputConfigInfo, uint InputConfigLength, IntPtr pOutputConfigInfo, uint OutputConfigInfoLength, ref uint pReturnLength, IntPtr pOverlapped);

        [DllImport("Httpapi.dll")]
        internal static extern uint HttpDeleteServiceConfiguration(IntPtr ServiceHandle, HTTP_SERVICE_CONFIG_ID ConfigId, IntPtr pConfigInformation, uint ConfigInformationLength, IntPtr pOverlapped);
    }

    internal struct HTTPAPI_VERSION
    {
        public ushort HttpApiMajorVersion;
        public ushort HttpApiMinorVersion;
    }

    internal enum HTTP_SERVICE_CONFIG_ID
    {
        HttpServiceConfigIPListenList,
        HttpServiceConfigSSLCertInfo,
        HttpServiceConfigUrlAclInfo,
        HttpServiceConfigTimeout,
        HttpServiceConfigMax
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HTTP_SERVICE_CONFIG_URLACL_QUERY
    {
        public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;
        public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;
        public uint dwToken;
    }

    internal enum HTTP_SERVICE_CONFIG_QUERY_TYPE
    {
        HttpServiceConfigQueryExact,
        HttpServiceConfigQueryNext,
        HttpServiceConfigQueryMax
    }

    internal struct HTTP_SERVICE_CONFIG_URLACL_KEY
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pUrlPrefix;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HTTP_SERVICE_CONFIG_URLACL_SET
    {
        public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;
        public HTTP_SERVICE_CONFIG_URLACL_PARAM ParamDesc;
    }

    internal struct HTTP_SERVICE_CONFIG_URLACL_PARAM
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pStringSecurityDescriptor;
    }
}
