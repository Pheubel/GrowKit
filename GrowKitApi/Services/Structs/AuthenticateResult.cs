using GrowKitApi.Services.Enums;
using System;

namespace GrowKitApi.Services.Structs
{
    /// <summary> A struct providing basic result information of user focused authentication methods.</summary>
    public struct AuthenticationResult
    {
        /// <summary> The token string used for authentication.</summary>
        public readonly string TokenString;
        /// <summary> The result code of the method.</summary>
        public readonly AuthenticationResults Result;

        /// <summary> The constructor for a succesful result for the authentication method.</summary>
        /// <param name="tokenString"> The token to be attached to the result body.</param>
        public AuthenticationResult(string tokenString)
        {
            TokenString = tokenString;
            Result = AuthenticationResults.Succes;
        }

        /// <summary> The constructor for afailed result of an authentication method.</summary>
        /// <param name="errorReason"> The reason why the method did not return succesful</param>
        public AuthenticationResult(AuthenticationResults errorReason)
        {
            TokenString = default;
            if (errorReason == AuthenticationResults.Succes)
                throw new ArgumentException("errorReason cannot be Succes");
            Result = errorReason;
        }
    }
}
