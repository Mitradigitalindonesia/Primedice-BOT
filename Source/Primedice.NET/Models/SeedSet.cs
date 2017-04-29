using KriPod.Primedice.Converters;
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

        [JsonProperty("previous_client")]
        public string PreviousClientSeed { get; internal set; }

        [JsonProperty("server")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed ServerSeedHashed { get; internal set; }

        [JsonProperty("previous_server")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed PreviousServerSeed { get; internal set; }

        [JsonProperty("previous_server_hashed")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed PreviousServerSeedHashed { get; internal set; }

        [JsonProperty("next_seed")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed NextServerSeedHashed { get; internal set; }

        internal ServerSeed ServerSeed { get; set; }

        internal ServerSeed NextServerSeed { get; set; }
    }
}
