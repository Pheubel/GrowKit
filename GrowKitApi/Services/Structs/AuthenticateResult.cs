using GrowKitApi.Services.Enums;
using System;

namespace GrowKitApi.Services.Structs
{
    public struct AuthenticationResult
    {
        public readonly string TokenString;
        public readonly Results Result;

        public AuthenticationResult(string tokenString)
        {
            TokenString = tokenString;
            Result = Results.Succes;
        }

        public AuthenticationResult(Results errorReason)
        {
            TokenString = default;
            if (errorReason == Results.Succes)
                throw new ArgumentException("errorReason cannot be Succes");
            Result = errorReason;
        }
    }
}
