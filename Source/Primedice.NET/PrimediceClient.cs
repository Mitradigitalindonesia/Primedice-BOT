using KriPod.Primedice.Components;

namespace KriPod.Primedice
{
    /// <summary>Represents a client which implements the functionality of Primedice.</summary>
    public class PrimediceClient
    {
        /// <summary>Contains methods for user management.</summary>
        public UserManager Users { get; }

        /// <summary>Contains methods for bet management.</summary>
        public BetManager Bets { get; }

        /// <summary>Contains methods for wallet management.</summary>
        public WalletManager Wallet { get; }

        private RestWebClient WebClient { get; }

        /// <summary>Creates a new Primedice client instance.</summary>
        /// <param name="authToken">Access token used for creating an authenticated instance.</param>
        public PrimediceClient(string authToken = null)
        {
            WebClient = new RestWebClient(authToken);

            Users = new UserManager(WebClient);
            Bets = new BetManager(WebClient);
            Wallet = new WalletManager(WebClient);
        }
    }
}
