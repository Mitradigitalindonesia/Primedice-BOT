using PCLCrypto;
using System;
using System.Text;
using System.Collections.Generic;

namespace KriPod.Primedice
{
    /// <summary>Provides utility methods for Primedice.NET.</summary>
    public static class Utils
    {
        internal const string ApiUrlBase = "https://api.primedice.com/api/";

        internal const float HouseEdge = 0.01F;

        private const ulong SatoshiInBitcoin = 100000000;
        private const byte BitcoinPrecisionDigits = 8;

        private static readonly char[] DefaultAllowedRandomClientSeedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        /// <summary>Converts a specific amount of satoshis to bitcoins.</summary>
        /// <param name="satoshiAmount">Amount of satoshis to convert.</param>
        /// <returns>A number representing the currency amount in bitcoins.</returns>
        public static double SatoshiToBitcoin(double satoshiAmount)
        {
            return Math.Round(satoshiAmount / SatoshiInBitcoin, BitcoinPrecisionDigits, MidpointRounding.AwayFromZero);
        }

        /// <summary>Converts a specific amount of bitcoins to satoshis.</summary>
        /// <param name="bitcoinAmount">Amount of bitcoins to convert.</param>
        /// <returns>A number representing the currency amount in satoshis.</returns>
        public static double BitcoinToSatoshi(double bitcoinAmount)
        {
            return Math.Round(bitcoinAmount * SatoshiInBitcoin, BitcoinPrecisionDigits, MidpointRounding.AwayFromZero);
        }

        /// <summary>Generates a cryptographically random client seed.</summary>
        /// <param name="length">Amount of characters which the output should contain.</param>
        /// <param name="allowedChars">List of characters which are allowed in the output. Null means that the recommended set of characters (a-z, A-Z, 0-9) should be allowed.</param>
        /// <returns>A random string with the length specified in <paramref name="length"/>.</returns>
        public static string GenerateRandomClientSeed(ushort length = 30, IList<char> allowedChars = null)
        {
            // Set the value of allowedChars if necessary
            if (allowedChars == null) {
                allowedChars = DefaultAllowedRandomClientSeedChars;
            }

            // Initialize an array of random bytes
            var buffer = WinRTCrypto.CryptographicBuffer.GenerateRandom(length);

            // Convert the byte buffer to string
            var sb = new StringBuilder(length);
            for (var i = buffer.Length - 1; i >= 0; i--) {
                // Append a randomly selected char from the list of allowed chars
                sb.Append(allowedChars[buffer[i] % allowedChars.Count]);
            }

            return sb.ToString();
        }

        /// <summary>Generates a new random server seed.</summary>
        /// <returns>A <see cref="ServerSeed"/> instance.</returns>
        public static ServerSeed GenerateRandomServerSeed()
        {
            // Generate the seed from an array of random bytes
            return new ServerSeed(WinRTCrypto.CryptographicBuffer.GenerateRandom(32));
        }

        /// <summary>Converts an array of bytes to a lowercase hexadecimal string.</summary>
        /// <param name="bytes">Array of bytes to convert.</param>
        /// <returns>A lowercase hexadecimal string.</returns>
        public static string ByteArrayToHexString(byte[] bytes)
        {
            var length = bytes.Length;
            var chars = new char[length << 1];

            for (var i = 0; i < bytes.Length; i++) {
                var b = bytes[i] >> 4;
                chars[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));

                b = bytes[i] & 0xF;
                chars[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
            }

            return new string(chars);
        }

        /// <summary>Converts a hexadecimal string to a byte array.</summary>
        /// <param name="str">The hexadecimal string to convert.</param>
        /// <returns>An array of bytes.</returns>
        public static byte[] HexStringToByteArray(string str)
        {
            var length = str.Length;
            if (length % 2 == 1) {
                throw new PrimediceException("A byte array specified as a hexadecimal string cannot have an odd number of digits.");
            }

            var bytes = new byte[length >> 1];

            for (var i = 0; i < length >> 1; i++) {
                bytes[i] = (byte)((GetHexValue(str[i << 1]) << 4) + (GetHexValue(str[(i << 1) + 1])));
            }

            return bytes;
        }

        private static int GetHexValue(char hex)
        {
            var val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
