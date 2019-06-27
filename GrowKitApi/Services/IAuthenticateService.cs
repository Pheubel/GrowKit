using GrowKitApi.Services.Structs;
using GrowKitApiDTO;
using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    /// <summary> A authentication store that handles common actions related to authentication of users.</summary>
    public interface IAuthenticateService
    {
        /// <summary> Authenticates the user with the given credentials.</summary>
        /// <param name="credentials"> The user credentials.</param>
        /// <returns> Authentication result.</returns>
        Task<AuthenticationResult> AuthenticateAsync(AuthenticationDTO credentials);

        /// <summary> Validates the 2fa code given by the user.</summary>
        /// <param name="code"> The given c.ode.</param>
        /// <param name="userId"> The user to validate agains.</param>
        /// <returns> Authentication result</returns>
        Task<AuthenticationResult> ValidateAuthenticationCodeAsync(string code, long userId);
    }
}
