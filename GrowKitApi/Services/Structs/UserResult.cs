using GrowKitApi.Services.Enums;

namespace GrowKitApi.Services.Structs
{
    /// <summary> A struct providing basic result information of user focused authentication methods.</summary>
    public readonly struct UserResult
    {
        /// <summary> The result code of the method.</summary>
        public readonly AuthenticationResults Result;
        /// <summary> THe id of the user in question.</summary>
        public readonly long User;
        /// <summary> Determines if the user has two factor authentication enabled.</summary>
        public readonly bool Has2FaEnabled;

        /// <summary> The constructor for a failed result.</summary>
        /// <param name="errorReason"> THe reason why the method failed.</param>
        public UserResult(AuthenticationResults errorReason)
        {
            User = default;
            Has2FaEnabled = default;
            Result = errorReason;
        }

        /// <summary> THe constructor for a succesful result of an authentication method</summary>
        /// <param name="user"> The id of the user.</param>
        /// <param name="has2FaEnabled"> Signal that the current user has 2fa enabled, defaults to false.</param>
        public UserResult(long user, bool has2FaEnabled = false)
        {
            User = user;
            Has2FaEnabled = has2FaEnabled;
            Result = AuthenticationResults.Succes;
        }
    }
}
