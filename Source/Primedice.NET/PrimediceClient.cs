using KriPod.Primedice.Components;
using System;

namespace KriPod.Primedice
{
    /// <summary>Represents a client which implements the functionality of Primedice.</summary>
    public class PrimediceClient : IDisposable
    {
        /// <summary>Contains methods for user management.</summary>
        public UserManager Users { get; }

        /// <summary>Contains methods for bet management.</summary>
        public BetManager Bets { get; }

        /// <summary>Contains methods for wallet management.</summary>
        public WalletManager Wallet { get; }

        private RestWebClient WebClient { get; }

        private bool IsDisposed { get; set; }

        /// <summary>Creates a new Primedice client instance.</summary>
        /// <param name="authToken">Access token used for creating an authenticated instance.</param>
        public PrimediceClient(string authToken = null)
        {
            WebClient = new RestWebClient(authToken);

            Users = new UserManager(WebClient);
            Bets = new BetManager(WebClient);
            Wallet = new WalletManager(WebClient);
        }

        /// <summary>Disposes the resources used by the client.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing) {
                // Free managed resources
                WebClient.Dispose();
            }

            IsDisposed = true;
        }
    }
}
