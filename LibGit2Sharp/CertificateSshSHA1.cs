using System;
using System.Diagnostics;
using LibGit2Sharp.Core;

namespace LibGit2Sharp
{
    public class CertificateSshSHA1 : Certificate
    {
        public readonly byte[] Hash;

        internal CertificateSshSHA1(GitCertificateSsh cert)
        {
            Debug.Assert(cert.type == GitCertificateSshType.SHA1);

            cert.HashSHA1.CopyTo(Hash, 0);
        }
    }
}
