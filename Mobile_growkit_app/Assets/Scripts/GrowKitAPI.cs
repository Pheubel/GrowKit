public static class GrowKitAPI
{
    public const string BaseUrl = "http://vps29.dss.cloud:5000/api";
    public static string StickEndpoint(long stickID) => $"{BaseUrl}/sensorstick/{stickID}";
}