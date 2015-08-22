using System;
using System.Collections.Generic;
using Renci.SshNet;
using System.IO;

namespace LibGit2Sharp
{
    class AuthenticationState
    {
        public List<Type> AuthenticationTypes { get; set; }
    }

    class AcquirerPasswordAuthenticationMethod : AuthenticationMethod
    {
        public override string Name {
            get {
                return "password";
            }
        }

        readonly SshSubtransport subtransport;

        AcquirerPasswordAuthenticationMethod(SshSubtransport subtransport, string username)
            : base(username)
        {
            this.subtransport = subtransport;
        }

        public override AuthenticationResult Authenticate(Session session)
        {
            // Ask the server for what authentication methods it actually allows
            // TODO: this bit should go into a common function for all
            if (subtransport.AuthState.AuthenticationTypes == null)
            {
                using (var method = new NoneAuthenticationMethod(Username))
                {
                    method.Authenticate(session);
                    foreach (var allowed in method.AllowedAuthentications)
                    {
                        switch (allowed)
                        {
                            case "password":
                                subtransport.AuthState.AuthenticationTypes.Add(typeof(UsernamePasswordCredentials));
                                break;
                        }
                    }
                }
            }

            Credentials cred;
            int res = subtransport.AcquireCredentials(out cred, Username, subtransport.AuthState.AuthenticationTypes.ToArray());
            if (res != 0)
            {
                throw new NotImplementedException("dunno what to do for errors");
            }

            UsernamePasswordCredentials creds = (UsernamePasswordCredentials)cred;

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

            throw new NotImplementedException();
        }
    }

    class SshSubtransportStream : SmartSubtransportStream
    {

        SshClient client;
        SshCommand command;
        string path;

        SshSubtransportStream(SshSubtransport parent, string repoPath)
            : base(parent)
        {
            client = new SshClient(parent.ConnectionInfo);
            path  = repoPath;
        }

        public override int Write(Stream dataStream, long length)
        {
            throw new NotImplementedException();
        }

        public override int Read(Stream dataStream, long length, out long bytesRead)
        {
            throw new NotImplementedException();
        }
    }
}

