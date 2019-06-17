using System;

namespace GrowKitApi.Entities.Enums
{
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
