using KriPod.Primedice.Converters;
using Newtonsoft.Json;

namespace KriPod.Primedice
{
    /// <summary>Represents a <see cref="User"/> with additional properties.</summary>
    public class UserExtended : User
    {
        [JsonProperty("password")]
        public bool HasPassword { get; private set; }

        [JsonProperty("email_enabled")]
        public bool HasEmail { get; private set; }

        [JsonProperty("address_enabled")]
        public bool HasBitcoinAddress { get; private set; }

        [JsonProperty("otp_enabled")]
        public bool IsOtpAuthEnabled { get; private set; }
        
        [JsonProperty("balance")]
        public double Balance { get; private set; }

        [JsonProperty("affiliate_total")]
        public double AffiliateBalance { get; private set; }

        [JsonProperty("address")]
        public string BitcoinAddress { get; private set; }

        [JsonProperty("referred")]
        public ulong ReferredCount { get; private set; }

        [JsonProperty("nonce")]
        public ulong Nonce { get; private set; }

        [JsonProperty("client")]
        public string ClientSeed { get; private set; }

        [JsonProperty("previous_client")]
        public string PreviousClientSeed { get; private set; }

        [JsonProperty("server")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed ServerSeedHashed { get; private set; }

        [JsonProperty("previous_server")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed PreviousServerSeed { get; private set; }

        [JsonProperty("previous_server_hashed")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed PreviousServerSeedHashed { get; private set; }

        [JsonProperty("next_seed")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed NextServerSeed { get; private set; }

        [JsonProperty("otp_token")]
        public string OtpAuthToken { get; private set; }

        [JsonProperty("otp_qr")]
        public string OtpAuthQrCodeUrl { get; private set; }

        public string AuthToken { get; internal set; }
    }
}
