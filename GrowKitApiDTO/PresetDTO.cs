namespace GrowKitApiDTO
{
    /// <summary> The model for sending preset information.</summary>
    [System.Serializable]
    public class PresetDTO
    {
        /// <summary> The low and high point of the ideal light level range.</summary>
        /// <remarks> The low point is store at [0], the high point is stored at [1]</remarks>
        public int[] Light { get; set; }
        /// <summary> The low and high point of the ideal temperature level range.</summary>
        /// <remarks> The low point is store at [0], the high point is stored at [1]</remarks>
        public int[] Temperature { get; set; }
        /// <summary> The low and high point of the ideal sunshine time range.</summary>
        /// <remarks> The low point is store at [0], the high point is stored at [1]</remarks>
        public int[] Sunshine { get; set; }
        /// <summary> The low and high point of the ideal moistur levele range.</summary>
        /// <remarks> The low point is store at [0], the high point is stored at [1]</remarks>
        public int[] Moisture { get; set; }
    }
}
