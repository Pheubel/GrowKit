using GrowKitApi.Services.Enums;

namespace GrowKitApi.Services.Structs
{
    public readonly struct UserResult
    {
        public readonly Results Result;
        public readonly long User;
        public readonly bool Has2FaEnabled;


        public UserResult(Results errorReason)
        {
            User = default;
            Has2FaEnabled = default;
            Result = errorReason;
        }

        public UserResult(long user, bool has2FaEnabled = false)
        {
            User = user;
            Has2FaEnabled = has2FaEnabled;
            Result = Results.Succes;
        }
    }
}
