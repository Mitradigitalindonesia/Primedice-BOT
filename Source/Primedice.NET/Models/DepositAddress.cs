using Newtonsoft.Json;

namespace KriPod.Primedice
{
    class DepositAddress
    {
        [JsonProperty("address")]
        public string Address { get; private set; }
    }
}
