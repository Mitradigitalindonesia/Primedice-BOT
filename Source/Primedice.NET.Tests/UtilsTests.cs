using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KriPod.Primedice.Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void SatoshiToBitcoin()
        {
            Assert.AreEqual(1, Utils.SatoshiToBitcoin(Config.SatoshiInBitcoin));
        }

        [TestMethod]
        public void BitcoinToSatoshi()
        {
            Assert.AreEqual(Config.SatoshiInBitcoin, Utils.BitcoinToSatoshi(1));
        }

        [TestMethod]
        public void GenerateRandomClientSeed()
        {
            // Specify the requirements of the seed to be generated
            const ushort seedLength = 50;
            var allowedChars = new List<char> { 'a', 'b', 'c', '1', '2', '3' };
            
            // Generate a client seed with the parameters specified
            var clientSeed = Utils.GenerateRandomClientSeed(seedLength, allowedChars);

            // Check whether the length of the generated seed is correct
            Assert.AreEqual(seedLength, clientSeed.Length);

            // Check whether the generated seed only consists of the allowed characters
            for (var i = 0; i < clientSeed.Length; i++) {
                Assert.IsTrue(allowedChars.Contains(clientSeed[i]));
            }
        }

        [TestMethod]
        public void GenerateRandomServerSeed()
        {
            Assert.AreEqual(Config.ServerSeedBytes.Length, Utils.GenerateRandomServerSeed().Bytes.Length);
        }

        [TestMethod]
        public void GenerateServerSeedHash()
        {
            CollectionAssert.AreEqual(Config.ServerSeedHashedBytes, new ServerSeed(Config.ServerSeed).GetHashed().Bytes);
            CollectionAssert.AreEqual(Config.ServerSeedHashedBytes, new ServerSeed(Config.ServerSeedBytes).GetHashed().Bytes);
        }

        [TestMethod]
        public void ByteArrayToHexString()
        {
            Assert.AreEqual(Config.ServerSeed, Utils.ByteArrayToHexString(Config.ServerSeedBytes));
        }

        [TestMethod]
        public void HexStringToByteArray()
        {
            CollectionAssert.AreEqual(Config.ServerSeedBytes, Utils.HexStringToByteArray(Config.ServerSeed));
        }
    }
}
