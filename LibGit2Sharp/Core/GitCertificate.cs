using System;
using System.Runtime.InteropServices;

namespace LibGit2Sharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GitCertificate
    {
        public GitCertificateType type;
    }
}

