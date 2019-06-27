using System;

namespace GrowKitApi.Entities.Enums
{
    /// <summary> The flags representing the state the appication user is in.</summary>
    [Flags]
    public enum UserFlags
    {
        HasUsernameSet = 1 << 2,
        IsVerifiedCreator = 1 << 3,
        HasCommissionsSetUp = 1 << 4,
        HasTiersSetUp = 1 << 5,
        HasAvatarSet = 1 << 6
    }
}
