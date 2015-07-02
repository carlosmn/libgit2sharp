using System;
using System.Diagnostics;
using LibGit2Sharp.Core;

namespace LibGit2Sharp
{
    public class CertificateSshMD5 : Certificate
    {
        public readonly byte[] Hash;

        internal CertificateSshMD5(GitCertificateSsh cert)
        {
            Debug.Assert(cert.type == GitCertificateSshType.MD5);

            cert.HashMD5.CopyTo(Hash, 0);
        }
    }
}

