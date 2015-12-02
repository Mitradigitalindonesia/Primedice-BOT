using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace KriPod.Primedice.Tests
{
    [TestClass]
    public class BetTests
    {
        [TestMethod]
        public async Task CreateAndVerifySimulated()
        {
            // Initialize a new, unauthorized client instance for bet simulation
            var client = new PrimediceClient();

            // Change the client seed and the next server seed
            await client.Bets.ChangeClientSeed("5S7u5PSIEQsimc6XmhK7cMx7U0wWpT");
            client.Bets.ChangeSimulatedServerSeed(new ServerSeed("be1961f028ff456db6b2ccdebef5265bc16b21bbc5fbba993abd2ff6767a39ad"));

            // Apply the change of the next server seed immediately
            client.Bets.ChangeSimulatedServerSeed();

            // Create a new bet, and then verify its most important details
            var betAmount = 42;
            var bet = await client.Bets.Create(betAmount, BetCondition.LowerThan, 11);
            Assert.AreEqual(45.03, bet.Roll, 0.001);
            Assert.AreEqual(true, bet.IsSimulated);
            Assert.AreEqual(false, bet.IsWon);
            Assert.AreEqual(betAmount, bet.Amount);
        }
    }
}
