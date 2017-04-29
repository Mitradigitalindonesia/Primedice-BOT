using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace KriPod.Primedice.Components
{
    /// <summary>Represents a <see cref="PrimediceClient"/>'s container of bet management methods.</summary>
    public class BetManager
    {
        private RestWebClient WebClient { get; }

        private SeedSet SimulatedSeedSet { get; set; }

        internal BetManager(RestWebClient webClient)
        {
            WebClient = webClient;

            if (!webClient.IsAuthorized) {
                // Initialize a new simulated seed set
                SimulatedSeedSet = new SeedSet();
                ChangeClientSeed().Wait();

                for (var i = 2; i > 0; i--) {
                    ChangeSimulatedServerSeed();
                }
            }
        }

        /// <summary>Gets a specific <see cref="Bet"/> by ID.</summary>
        /// <param name="id">ID of the sought <see cref="Bet"/>.</param>
        /// <returns>An awaitable <see cref="Bet"/> object, or null if no match was found.</returns>
        public async Task<Bet> Get(ulong id)
        {
            var response = await WebClient.Get<ServerResponse>("bets/" + id);
            return response.Bet;
        }

        /*public object GetLast30OfSite()
        {
            return null;
        }*/

        /// <summary>Creates a new <see cref="Bet"/> on the server, or a simulated one if the current <see cref="PrimediceClient"/> is not authenticated.</summary>
        /// <param name="amount">Amount of satoshi to bet.</param>
        /// <param name="condition">Condition of the bet, relative to <paramref name="target"/>.</param>
        /// <param name="target">Target of the dice roll.</param>
        /// <returns>An awaitable <see cref="Bet"/> object if the bet was placed successfully.</returns>
        public async Task<Bet> Create(double amount, BetCondition condition, float target)
        {
            // Round the target automatically
            target = (float)Math.Round(target, 2, MidpointRounding.AwayFromZero);

            if (WebClient.IsAuthorized) {
                // Create a new bet on the server
                var response = await WebClient.Post<ServerResponse>("bet", new Dictionary<string, string> {
                    ["amount"] = amount.ToString(CultureInfo.InvariantCulture),
                    ["target"] = target.ToString(CultureInfo.InvariantCulture),
                    ["condition"] = condition.ToJsonString()
                });
                return response.Bet;

            } else {
                // Create a new simulated bet
                var bet = new Bet {
                    IsSimulated = true,
                    ServerSeedHashed = SimulatedSeedSet.ServerSeedHashed,
                    ClientSeed = SimulatedSeedSet.ClientSeed,
                    Nonce = SimulatedSeedSet.Nonce,
                    Time = DateTime.UtcNow,
                    Amount = amount,
                    Condition = condition,
                    Target = target
                };

                // Calculate the roll of the bet
                var roll = bet.CalculateRoll(SimulatedSeedSet.ServerSeed);
                bet.Roll = roll;

                // Determine whether the bet was won or lost and calculate the multiplier
                bool isWon;
                float multiplier;
                if (condition == BetCondition.LowerThan) {
                    isWon = roll < target;
                    multiplier = 100 / target * (1 - Utils.HouseEdge);
                } else {
                    isWon = roll > target;
                    multiplier = 100 / (100 - target) * (1 - Utils.HouseEdge);
                }
                multiplier = (float)Math.Floor(multiplier * 100000) / 100000;

                // Set the properties of the bet
                bet.IsWon = isWon;
                bet.Multiplier = multiplier;
                if (isWon) {
                    bet.ProfitAmount = amount * (multiplier - 1);
                } else {
                    bet.ProfitAmount = -amount;
                }

                // Generate a new server seed and reset nonce to 0 if necessary
                if (SimulatedSeedSet.Nonce == ulong.MaxValue) {
                    SimulatedSeedSet.Nonce = 0;

                    var nextServerSeed = Utils.GenerateRandomServerSeed();
                    SimulatedSeedSet.PreviousServerSeed = SimulatedSeedSet.ServerSeed;
                    SimulatedSeedSet.PreviousServerSeedHashed = SimulatedSeedSet.ServerSeedHashed;
                    SimulatedSeedSet.ServerSeed = SimulatedSeedSet.NextServerSeed;
                    SimulatedSeedSet.ServerSeedHashed = SimulatedSeedSet.NextServerSeedHashed;
                    SimulatedSeedSet.NextServerSeed = nextServerSeed;
                    SimulatedSeedSet.NextServerSeedHashed = nextServerSeed.GetHashed();

                } else {
                    // Increment the simulated nonce by 1
                    SimulatedSeedSet.Nonce += 1;
                }

                // Return the simulated bet object
                return bet;
            }
        }

        /// <summary>Changes the client seed which affects the outcome of bets.</summary>
        /// <param name="seed">The new seed to be used.</param>
        /// <returns>An awaitable <see cref="SeedSet"/> object if the client seed was changed successfully.</returns>
        public async Task<SeedSet> ChangeClientSeed(string seed = null)
        {
            // Generate a new client seed automatically if no seed was specified
            if (string.IsNullOrEmpty(seed)) {
                seed = Utils.GenerateRandomClientSeed();
            }

            if (WebClient.IsAuthorized) {
                // Change the client seed on the server
                var response = await WebClient.Post<ServerResponse>("seed", new Dictionary<string, string> {
                    ["seed"] = seed
                });
                return response.SeedSet;

            } else {
                // Change the client seed locally
                SimulatedSeedSet.ClientSeed = seed;
                return SimulatedSeedSet;
            }
        }

        /// <summary>
        /// Changes the simulated server seed which affects the outcome of bets.</summary>
        /// <param name="nextSeed">The seed to be used after the next server seed change.</param>
        /// <returns>A <see cref="SeedSet"/> object if the simulated server seed was changed successfully.</returns>
        public SeedSet ChangeSimulatedServerSeed(ServerSeed nextSeed = null)
        {
            // Throw an exception if not in simulation mode
            if (WebClient.IsAuthorized) {
                throw new PrimediceException("The simulated server seed can only be altered in simulation mode.");
            }

            // Generate a new server seed automatically if no next seed was specified
            if (nextSeed == null) {
                nextSeed = Utils.GenerateRandomServerSeed();
            }

            var nextSeedHashed = nextSeed.GetHashed();

            // Change the values of the simulated SeedSet
            SimulatedSeedSet.PreviousServerSeed = SimulatedSeedSet.ServerSeed;
            SimulatedSeedSet.PreviousServerSeedHashed = SimulatedSeedSet.ServerSeedHashed;
            SimulatedSeedSet.ServerSeed = SimulatedSeedSet.NextServerSeed;
            SimulatedSeedSet.ServerSeedHashed = SimulatedSeedSet.NextServerSeedHashed;
            SimulatedSeedSet.NextServerSeed = nextSeed;
            SimulatedSeedSet.NextServerSeedHashed = nextSeedHashed;

            return SimulatedSeedSet;
        }
    }
}
