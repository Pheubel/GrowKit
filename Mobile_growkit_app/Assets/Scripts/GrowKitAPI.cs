using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public static class GrowKitAPI
{
    private static readonly HttpClient _client = new HttpClient();

    public const string BaseUrl = "https://vps29.dss.cloud:5001/api";

    public static async Task<string> GetTest()
    {
        HttpResponseMessage response = null;

        string result = string.Empty;
        try
        {
            response = await _client.GetAsync($"{BaseUrl}/values");

            result = await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Debug.Log($"can't connect to the server: {e}");
        }
        finally
        {
            response?.Dispose();
        }

        return result;
    }

    public static async Task<GKSensorStick[]> GetSticks(Guid userId)
    {
        HttpResponseMessage response = null;
        GKSensorStick[] sticks = null;

        try
        {
            response = await _client.GetAsync($"{BaseUrl}/getsticks/{userId}");
            sticks = JsonUtility.FromJson<GKSensorStick[]>(await response.Content.ReadAsStringAsync());
        }
        catch(Exception e)
        {
#if UNITY_EDITOR
            Debug.Log($"can't connect to the server: {e}");
#endif
        }
        finally
        {
            response?.Dispose();
        }

        return sticks;
    }
}

[Serializable]
public class GKSensorStick
{
    /// <summary> The unique identity of the sensor stick.</summary>
    public long SensorId { get; set; }
    /// <summary> The linked hub responsible for this sensor stick.</summary>
    public ulong ConnectedHubId { get; set; }
    /// <summary> The name given to the plant.</summary>
    public string PlantName { get; set; }
    /// <summary> The latest water Level measured.</summary>
    public ushort WaterLevel { get; set; }
    /// <summary> The latest light Level measured.</summary>
    public ushort LightLevel { get; set; }
    /// <summary> The latest temperature Level measured.</summary>
    public ushort TemperatureLevel { get; set; }
}
