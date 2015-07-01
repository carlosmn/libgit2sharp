using System;

namespace LibGit2Sharp
{
    /// <summary>
    /// Git certificate types to present to the user
    /// </summary>
    internal enum GitCertificateType
    {
        /// <summary>
        /// No type, there is no information to pass
        /// </summary>
        None = 0,
        /// <summary>
        /// The certificate is a x509 certificate
        /// </summary>
        X509 = 1,
        /// <summary>
        /// The "certificate" is in fact a hostkey identification for ssh.
        /// </summary>
        Hostkey = 2,
    }
}

