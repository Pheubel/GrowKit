﻿using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A preset of settings with optimal levels for a plant.</summary>
    public class PlantPreset
    {
        /// <summary> The unique identity of this preset.</summary>
        [Key] public int PresetId { get; set; }
        /// <summary> The name for this preset.</summary>
        [Required] public string Name { get; set; }
        /// <summary> The estimated ammount of days until the user can expect their plant to have matured.</summary>
        public int EstimatedGrowthTime { get; set; }
        /// <summary> The ideal water level for this plant to grow in.</summary>
        public ushort OptimalWaterLevel { get; set; }
        /// <summary> The ideal light level for this plant to grow in.</summary>
        public ushort OptimalLightLevel { get; set; }
        /// <summary> The ideal temperature level for this plant to grow in.</summary>
        public ushort OptimalTemperatureLevel { get; set; }
    }
}
