namespace GrowKitApiDTO
{
    /// <summary> The model for the message update recieved from the sensor stick.</summary>
    /// <remarks> This class gets re-used for sending data from the database to an API-user when requesting values.</remarks>
    [System.Serializable]
    public class StickUpdateDTO
    {
        /// <summary> The current value for the light level.</summary>
        public int Light { get; set; }
        /// <summary> The current value for the moisture level.</summary>
        public int Moisture { get; set; }
        /// <summary> The current value for the temperature level.</summary>
        public int Temperature { get; set; }
        /// <summary> The amount of hours the plant has been in direct light.</summary>
        public int LightTime { get; set; }
        /// <summary> The ammount of seconds after the epoch of the growkit.</summary>
        public int Timestamp { get; set; }
    }
}
