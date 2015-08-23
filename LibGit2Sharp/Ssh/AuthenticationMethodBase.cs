using System;
using System.Collections.Generic;
using Renci.SshNet;

namespace LibGit2Sharp.Ssh
{
    /// <summary>
    /// The common base for our authentication methods which we use to convert the push aspect
    /// of SSH.NET's own authentication methods to libgit2's, which prompts for the credentials
    /// once the server asks for them, rather than having to set them up-front.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class AuthenticationMethodBase : AuthenticationMethod
    {
        /// <summary>
        /// The name of the authentication method this corresponds to. Typically 'password' or 'publickey'.
        /// </summary>
        public override abstract string Name { get; }

        /// <summary>
        /// The subtransport which is using this instance. It contains the shared authentication state.
        /// </summary>
        protected readonly SshSubtransport subtransport;

        internal AuthenticationMethodBase(SshSubtransport subtransport, string username)
            : base(username)
        {
            this.subtransport = subtransport;
        }

        void QueryServer(Session session)
        {
            // Already done, the caller can read from the state already
            if (subtransport.AuthState.AuthenticationTypes != null)
            {
                return;
            }

            subtransport.AuthState.AuthenticationTypes = new List<Type>();

            // Send an authentication method of 'none' so the server refuses and responds with the ones
            // which it actually supports.
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

        /// <summary>
        /// Ask the server for the authentication methods it supports, and the caller for which one it wants to use.
        /// These will be stored in the shared state in the subtransport. The query will be skipped if we have already
        /// performed the query.
        /// </summary>
        /// <param name="session">The session to query against</param>
        protected void QueryAuthentication(Session session)
        {
            QueryServer(session);

            int res = subtransport.AcquireCredentials(out subtransport.AuthState.Credentials, Username, subtransport.AuthState.AuthenticationTypes.ToArray());
            if (res != 0)
            {
                throw new NotImplementedException("dunno what to do for errors");
            }

            // TODO: we should check to make sure the callback returned a type of authentication we support
        }
    }
}

