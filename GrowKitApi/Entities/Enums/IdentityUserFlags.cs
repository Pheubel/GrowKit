using System;

namespace GrowKitApi.Entities.Enums
{
    /// <summary> The flags representing the state the authentication user is in.</summary>
    [Flags]
    public enum IdentityUserFlags : short
    {
        Uses2FA = 1 << 0,
        ConfirmedEmailAdress = 1 << 1,
        IsStaff = 1 << 8,
        IsJanitor = IsStaff | 1 << 9,
        IsModerator = IsStaff | 1 << 10,
        IsAdmin = IsStaff | 1 << 11
    }
}
