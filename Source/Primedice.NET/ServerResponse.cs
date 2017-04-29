using Newtonsoft.Json;

namespace KriPod.Primedice
{
    class ServerResponse<T>
    {
        [JsonProperty("meta")]
        public T MetaData { get; private set; }

        [JsonProperty("user")]
        public UserExtended UserExtended { get; private set; }

        [JsonProperty("bet")]
        public Bet Bet { get; private set; }

        [JsonProperty("seeds")]
        public SeedSet SeedSet { get; private set; }

        [JsonProperty("access_token")]
        public string AuthToken { get; private set; }
    }

    class ServerResponse : ServerResponse<object>
    {
        
    }
}
