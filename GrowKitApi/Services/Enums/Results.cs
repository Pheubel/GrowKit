namespace GrowKitApi.Services.Enums
{
    public enum Results : byte
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
