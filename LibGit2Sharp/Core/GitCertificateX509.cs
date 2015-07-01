using System;
using System.Runtime.InteropServices;

namespace LibGit2Sharp
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct GitCertificateX509
    {
        /// <summary>
        /// Type of the certificate, in this case, GitCertificateType.X509
        /// </summary>
        public GitCertificateType type;
        /// <summary>
        /// Pointer to the X509 certificate data
        /// </summary>
        public IntPtr data;
        /// <summary>
        ///  The size of the certificate data
        /// </summary>
        public UIntPtr len;
    }
}

