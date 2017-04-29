using PCLCrypto;
using System.Text;

namespace KriPod.Primedice
{
    /// <summary>Represents a server seed used for <see cref="Bet"/>s.</summary>
    public class ServerSeed
    {
        private string _hexString;
        private byte[] _bytes;

        public string HexString {
            get {
                // Lazy property initialization
                if (_hexString == null && _bytes != null) {
                    _hexString = Utils.ByteArrayToHexString(_bytes);
                }

                return _hexString;
            }

            private set { _hexString = value; }
        }

        public byte[] Bytes {
            get {
                // Lazy property initialization
                if (_bytes == null && _hexString != null) {
                    _bytes = Utils.HexStringToByteArray(_hexString);
                }

                return _bytes;
            }

            private set { _bytes = value; }
        }

        /// <summary>Creates a new server seed instance.</summary>
        /// <param name="hexString">Hexadecimal string to parse the server seed from.</param>
        public ServerSeed(string hexString)
        {
            HexString = hexString;
            Bytes = Utils.HexStringToByteArray(hexString);
        }

        /// <summary>Creates a new server seed instance.</summary>
        /// <param name="bytes">Byte array to parse the server seed from.</param>
        public ServerSeed(byte[] bytes)
        {
            HexString = Utils.ByteArrayToHexString(bytes);
            Bytes = bytes;
        }

        internal ServerSeed(string hexString, byte[] bytes)
        {
            HexString = hexString;
            Bytes = bytes;
        }

        /// <summary>Gets a hashed instance of the current server seed.</summary>
        /// <returns>A <see cref="ServerSeed"/> with the hashed parameters of this instance.</returns>
        public ServerSeed GetHashed()
        {
            // Hash the server seed with the SHA256 algorithm
            var algorithm = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            return new ServerSeed(algorithm.HashData(Encoding.UTF8.GetBytes(HexString)));
        }
    }
}
