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
        public double ProfitAmount { get; internal set; }

        [JsonProperty("win")]
        public bool IsWon { get; internal set; }

        [JsonProperty("jackpot")]
        public bool IsJackpot { get; private set; }

        [JsonProperty("nonce")]
        public ulong Nonce { get; internal set; }

        [JsonProperty("client")]
        public string ClientSeed { get; internal set; }

        [JsonProperty("server")]
        [JsonConverter(typeof(ServerSeedConverter))]
        public ServerSeed ServerSeedHashed { get; internal set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(BetTimeConverter))]
        public DateTime Time { get; internal set; }

        public bool IsSimulated { get; internal set; }

        /// <summary>Verifies the validity of the bet's outcome.</summary>
        /// <param name="serverSeed">The server seed which was used for rolling.</param>
        /// <returns>True whether the outcome of the bet was calculated fairly.</returns>
        public bool Verify(ServerSeed serverSeed)
        {
            return CalculateRoll(serverSeed).Equals(Roll);
        }

        internal float CalculateRoll(ServerSeed serverSeed)
        {
            var key = serverSeed.HexString;
            var text = ClientSeed + "-" + Nonce;

            // Generate HMAC-SHA256 hash using server seed as key and text as message
            var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha512);
            var hasher = algorithm.CreateHash(Encoding.UTF8.GetBytes(key));
            hasher.Append(Encoding.UTF8.GetBytes(text));
            var bytes = hasher.GetValueAndReset();

            // Keep grabbing hash bytes while the lucky number is greater than 10^6
            for (var i = 0; i + 2 < bytes.Length; i += 2) {
                // Start calculating the lucky number using the next 3 bytes
                var lucky = (bytes[i] << 16) + (bytes[i + 1] << 8) + bytes[i + 2];

                // Determine whether the iteration count is odd or even
                if (i % 4 == 0) {
                    // Even: Use the first 5 digits of the hex string from the 3 bytes
                    lucky = (lucky - lucky % 16) >> 4;

                } else {
                    // Odd: Use the last 5 digits of the hex string from the 3 bytes
                    lucky %= 2 << 24;
                }

                // Return a lucky number if possible
                if (lucky < 1000000) {
                    return (float)(lucky % 10000) / 100;
                }
            }

            // If the end of the hash is reached, just default to the highest number
            return 99.99F;
        }
    }
}
