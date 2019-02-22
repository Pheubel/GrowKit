using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A preset of settings with optimal levels for a plant.</summary>
    public class PlantPreset
    {
        /// <summary> The unique identity of this preset.</summary>
        [Key] public uint PresetId { get; set; }
        /// <summary> The name for this preset.</summary>
        public string Name { get; set; }
        /// <summary> The estimated ammount of days until the user can expect their plant to have matured.</summary>
        public int EstimatedGrowthTime { get; set; }
        /// <summary> The ideal water level for this plant to grow in.</summary>
        public short OptimalWaterLevel { get; set; }
        /// <summary> The ideal light level for this plant to grow in.</summary>
        public short OptimalLightLevel { get; set; }
        /// <summary> The ideal temperature level for this plant to grow in.</summary>
        public short OptimalTemperatureLevel { get; set; }
        /// <summary> The ideal Ph level for this plant to grow in.</summary>
        public short OptimalPhLevel { get; set; }
    }
}
