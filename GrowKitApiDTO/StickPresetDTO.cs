namespace GrowKitApiDTO
{
    /// <summary> A message model for displaying the preset that is attached to a sensor stick.</summary>
    [System.Serializable]
    public class StickPresetDTO
    {
        /// <summary> The Id of the sensor stick used in the database.</summary>
        public long ID_Stick { get; set; }
        /// <summary> The Id of the preset used in the database.</summary>
        public int PresetId { get; set; }
    }
}
