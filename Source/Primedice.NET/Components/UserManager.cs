using System.Collections.Generic;
using System.Threading.Tasks;

namespace KriPod.Primedice.Components
{
    /// <summary>Represents a <see cref="PrimediceClient"/>'s container of user management methods.</summary>
    public class UserManager
    {
        private RestWebClient WebClient { get; }

        internal UserManager(RestWebClient webClient)
        {
            WebClient = webClient;
        }

        /// <summary>Gets a specific <see cref="User"/> by username.</summary>
        /// <param name="username">Username of the sought <see cref="User"/>.</param>
        /// <returns>An awaitable <see cref="User"/> object, or null if no match was found.</returns>
        public async Task<User> Get(string username)
        {
            // Try querying self if no username was specified
            var isQueryingSelf = string.IsNullOrEmpty(username);

            // Abort the operation of querying self if the web client isn't authorized
            if (isQueryingSelf && !WebClient.IsAuthorized) {
                return null;
            }

            // Return the appropriate response object
            var response = await WebClient.Get<ServerResponse>("users/" + (isQueryingSelf ? "1" : username));
            return response.UserExtended;
        }

        /// <summary>Gets the <see cref="User"/> which belongs to the current <see cref="PrimediceClient"/> instance.</summary>
        /// <returns>An awaitable <see cref="User"/> object, or null if the current <see cref="PrimediceClient"/> is not authenticated.</returns>
        public async Task<UserExtended> GetSelf()
        {
            return await Get(null) as UserExtended;
        }

        /// <summary>Creates a new <see cref="User"/>.</summary>
        /// <param name="username">Username of the <see cref="User"/> to be created.</param>
        /// <param name="affiliateUsername">Username of the <see cref="User"/> who referred the <see cref="User"/> to be created.</param>
        /// <returns>An awaitable <see cref="User"/> object if the registration was successful.</returns>
        public async Task<UserExtended> Create(string username, string affiliateUsername = null)
        {
            var response = await WebClient.Post<ServerResponse<UserMetaData>>("register", new Dictionary<string, string> {
                ["username"] = username,
                ["affiliate"] = affiliateUsername
            });

            // Throw an exception whether the user is blocked
            if (response.MetaData.IsBlocked) {
                throw new PrimediceException("The user is blocked.");
            }

            // Obtain the user's auth token, and then return the entire user object
            response.UserExtended.AuthToken = response.AuthToken;
            return response.UserExtended;
        }
    }
}
