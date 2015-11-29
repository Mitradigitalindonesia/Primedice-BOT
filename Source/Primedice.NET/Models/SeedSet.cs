using Newtonsoft.Json;

namespace KriPod.Primedice
{
    /// <summary>Represents a set of seeds used by <see cref="User"/>s for their <see cref="Bet"/>s.</summary>
    public class SeedSet
    {
        [JsonProperty("nonce")]
        public ulong Nonce { get; internal set; }

        [JsonProperty("client")]
        public string ClientSeed { get; internal set; }

        [JsonProperty("server")]
        public string ServerSeedHashed { get; internal set; }

        [JsonProperty("previous_client")]
        public string PreviousClientSeed { get; internal set; }

        [JsonProperty("previous_server")]
        public string PreviousServerSeed { get; internal set; }

        [JsonProperty("previous_server_hashed")]
        public string PreviousServerSeedHashed { get; internal set; }

        [JsonProperty("next_seed")]
        public string NextServerSeedHashed { get; internal set; }

        internal string ServerSeed { get; set; }

        internal string NextServerSeed { get; set; }
    }
}
