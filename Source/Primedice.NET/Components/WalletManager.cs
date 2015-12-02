using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace KriPod.Primedice.Components
{
    /// <summary>Represents a <see cref="PrimediceClient"/>'s container of wallet management methods.</summary>
    public class WalletManager
    {
        private RestWebClient WebClient { get; }

        internal WalletManager(RestWebClient webClient)
        {
            WebClient = webClient;
        }

        /// <summary>Withdraws a specific amount of satoshis to a given address.</summary>
        /// <param name="amount">Amount of satoshi to withdraw.</param>
        /// <param name="address">A Bitcoin address where the withdrawn funds should be sent.</param>
        /// <returns>An awaitable <see cref="Withdrawal"/> object if the withdrawal was successful.</returns>
        public async Task<Withdrawal> Withdraw(double amount, string address)
        {
            return await WebClient.Post<Withdrawal>("withdraw", new Dictionary<string, string> {
                ["amount"] = amount.ToString(CultureInfo.InvariantCulture),
                ["address"] = address
            });
        }
    }
}
