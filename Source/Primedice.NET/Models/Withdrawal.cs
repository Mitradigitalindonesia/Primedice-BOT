using Newtonsoft.Json;
using System;

namespace KriPod.Primedice
{
    /// <summary>Represents a withdrawal from a wallet.</summary>
    public class Withdrawal
    {
        [JsonProperty("amount")]
        public double Amount { get; private set; }

        [JsonProperty("sent")]
        public double Sent { get; private set; }

        [JsonProperty("txid")]
        public string TxId { get; private set; }

        [JsonProperty("address")]
        public string Address { get; private set; }

        [JsonProperty("confirmed")]
        public bool IsConfirmed { get; private set; }

        [JsonProperty("timestamp")]
        public DateTime Time { get; private set; }
    }
}
