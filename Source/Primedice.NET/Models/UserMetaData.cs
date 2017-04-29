using Newtonsoft.Json;

namespace KriPod.Primedice
{
    class UserMetaData
    {
        [JsonProperty("blocked")]
        public bool IsBlocked { get; private set; }
    }
}
