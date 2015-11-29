using Newtonsoft.Json;
using System;

namespace KriPod.Primedice
{
    /// <summary>Represents a user.</summary>
    public class User
    {
        [JsonProperty("userid")]
        public ulong Id { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("registered")]
        public DateTime RegistrationTime { get; private set; }

        [JsonProperty("wagered")]
        public double WageredAmount { get; private set; }

        [JsonProperty("profit")]
        public double ProfitAmount { get; private set; }

        [JsonProperty("win_risk")]
        public double WinRisk { get; private set; }

        [JsonProperty("lose_risk")]
        public double LoseRisk { get; private set; }

        [JsonProperty("bets")]
        public ulong BetsCount { get; private set; }

        [JsonProperty("wins")]
        public ulong WinsCount { get; private set; }

        [JsonProperty("losses")]
        public ulong LossesCount { get; private set; }

        [JsonProperty("messages")]
        public ulong MessagesCount { get; private set; }
    }
}
