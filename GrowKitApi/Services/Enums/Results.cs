namespace GrowKitApi.Services.Enums
{
    /// <summary> The result codes for authentication related methods</summary>
    public enum AuthenticationResults : byte
    {
        UnknownError,
        Succes,
        InvalidCredentials,
        EmailTaken,
        MissingPassword,
        InvalidToken,
        TokenHasBeenUsed
    }
}
