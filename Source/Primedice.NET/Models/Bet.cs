using KriPod.Primedice.Converters;
using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Text;

namespace KriPod.Primedice
{
    /// <summary>Represents a bet made by a <see cref="User"/>.</summary>
    public class Bet
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("player_id")]
        public ulong OwnerUserId { get; private set; }

        [JsonProperty("player")]
        public string OwnerUsername { get; private set; }

        [JsonProperty("target")]
        public float Target { get; internal set; }

        [JsonProperty("multiplier")]
        public float Multiplier { get; internal set; }

        [JsonProperty("condition")]
        [JsonConverter(typeof(BetConditionConverter))]
        public BetCondition Condition { get; internal set; }

        [JsonProperty("roll")]
        public float Roll { get; internal set; }

        [JsonProperty("amount")]
        public double Amount { get; internal set; }

        [JsonProperty("profit")]
        public double ProfitAmount { get; private set; }

        [JsonProperty("win")]
        public bool IsWon { get; internal set; }

        [JsonProperty("jackpot")]
        public bool IsJackpot { get; private set; }

        [JsonProperty("nonce")]
        public ulong Nonce { get; internal set; }

        [JsonProperty("client")]
        public string ClientSeed { get; internal set; }

        [JsonProperty("server")]
        public string ServerSeedHashed { get; internal set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(BetTimeConverter))]
        public DateTime Time { get; internal set; }

        public bool IsSimulated { get; internal set; }

        /// <summary>Verifies the validity of the bet's outcome.</summary>
        /// <param name="serverSeed">The server seed which was used for rolling.</param>
        /// <returns>True whether the outcome of the bet was calculated fairly.</returns>
        public bool Verify(string serverSeed)
        {
            return CalculateRoll(serverSeed).Equals(Roll);
        }

        /// <summary>Verifies the validity of the bet's outcome.</summary>
        /// <param name="serverSeed">The server seed which was used for rolling.</param>
        /// <returns>True whether the outcome of the bet was calculated fairly.</returns>
        public bool Verify(byte[] serverSeed)
        {
            return CalculateRoll(Utils.ByteArrayToHexString(serverSeed)).Equals(Roll);
        }

        internal float CalculateRoll(string serverSeed)
        {
            var key = serverSeed;
            var text = ClientSeed + "-" + Nonce;

            // Generate HMAC-SHA256 hash using server seed as key and text as message
            var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha512);
            var hasher = algorithm.CreateHash(Encoding.UTF8.GetBytes(key));
            hasher.Append(Encoding.UTF8.GetBytes(text));
            var hash = Utils.ByteArrayToHexString(hasher.GetValueAndReset());

            double lucky;
            var i = 0;

            // Keep grabbing characters from the hash while lucky is greater than 10^6
            while (true) {
                // Examine an 5-char hex string as a double
                var hashStartIndex = i * 5;
                lucky = Convert.ToUInt32(hash.Substring(hashStartIndex, 5), 16);

                if (lucky < 1000000) {
                    break;
                }

                // If the end of the hash is reached, just default to the highest number
                if (hashStartIndex + 5 > 128) {
                    return 99.99F;
                }

                i++;
            }
            
            lucky %= 10000;
            lucky /= 100;

            return (float)lucky;
        }
    }
}
