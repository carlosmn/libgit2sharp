using System;
using Renci.SshNet;
using System.IO;

namespace LibGit2Sharp
{
    /// <summary>
    /// A SSH transport using a managed implementation of SSH, which avoids the need for libssh2
    /// </summary>
    public class SshSubtransport : SmartSubtransport
    {
        protected override SmartSubtransportStream Action(String url, GitSmartSubtransportAction action)
        {
            throw new NotImplementedException();
        }
    }

    class SshSubstransportStream : SmartSubtransportStream
    {
        SshClient Client { get; set; }
        SshCommand Command { get; set; }

        public override int Write(Stream dataStream, long length)
        {
            throw new NotImplementedException();
        }

        public override int Read(Stream dataStream, long length, out long readTotal)
        {
            throw new NotImplementedException();
        }
    }
}

