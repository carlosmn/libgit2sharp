using System;
using System.Collections.Generic;
using Renci.SshNet;
using System.IO;

namespace LibGit2Sharp.Ssh
{
    class AuthenticationState
    {
        public List<Type> AuthenticationTypes { get; set; }
        public Credentials Credentials;
    }

    class AcquirerPasswordAuthenticationMethod : AuthenticationMethodBase
    {
        public override string Name {
            get {
                return "password";
            }
        }

        AcquirerPasswordAuthenticationMethod(SshSubtransport subtransport, string username)
            : base(subtransport, username)
        {
        }

        public override AuthenticationResult Authenticate(Session session)
        {
            QueryAuthentication(session);

            // If we don't have credentials which match what we support, let the next one try
            UsernamePasswordCredentials creds = subtransport.AuthState.Credentials as UsernamePasswordCredentials;
            if (creds == null)
            {
                return AuthenticationResult.Failure;
            }

            using (var method = new PasswordAuthenticationMethod(creds.Username, creds.Password))
            {
                return method.Authenticate(session);
            }
        }
    }


    /// <summary>
    /// A SSH transport using a managed implementation of SSH, which avoids the need for libssh2
    /// </summary>
    public class SshSubtransport : SmartSubtransport
    {
        /// <summary>
        /// Shared state among the authentication methods
        /// </summary>
        internal AuthenticationState AuthState { get; private set; }

        /// <summary>
        /// Information about the connection we want to establish to the server
        /// </summary>
        /// <value>The connection info.</value>
        internal ConnectionInfo ConnectionInfo { get; private set; }

        public SshSubtransport()
        {
            AuthState = new AuthenticationState();
        }

        ConnectionInfo extractConnectionInfo(string url)
        {
            Uri uri;

            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                if (uri.Scheme != "ssh")
                {
                    throw new InvalidOperationException("The SSH subtransport can only handle SSH");
                }

                if (uri.UserInfo == null)
                {
                    throw new NotImplementedException("Cannot deal with not having a user rn");
                }

                // FIXME: let's actually support an auth method at some point
                return new ConnectionInfo(uri.Host, uri.Port == -1 ? 22 : uri.Port, uri.UserInfo, new NoneAuthenticationMethod(uri.UserInfo));
            }

            throw new Exception("oops");
        }

        protected override SmartSubtransportStream Action(String url, GitSmartSubtransportAction action)
        {
            var uri = new Uri(url);
            ConnectionInfo = extractConnectionInfo(url);

            throw new NotImplementedException();
        }
    }

    class SshSubtransportStream : SmartSubtransportStream
    {

        SshClient client;
        SshCommand command;
        string path;
        string commandName;

        SshSubtransportStream(SshSubtransport parent, string repoPath, string command)
            : base(parent)
        {
            client = new SshClient(parent.ConnectionInfo);
            path  = repoPath;
            commandName = command;
        }

        void SendCommand()
        {
            if (!client.IsConnected)
            {
                client.Connect();
            }

        }

        public override int Write(Stream dataStream, long length)
        {
            throw new NotImplementedException("write");
        }

        public override int Read(Stream dataStream, long length, out long bytesRead)
        {
            if (command == null)
            {
                SendCommand();
            }

            throw new NotImplementedException("read");
        }
    }
}

