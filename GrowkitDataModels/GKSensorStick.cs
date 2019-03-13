﻿using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A model for the sensor stick.</summary>
    public class GKSensorStick
    {
        /// <summary> The unique identity of the sensor stick.</summary>
        [Key] public long SensorId { get; set; }
        /// <summary> The linked hub responsible for this sensor stick.</summary>
        public virtual GKHub ConnectedHub { get; set; }
        /// <summary> The name given to the plant.</summary>
        public string PlantName { get; set; }
        /// <summary> The latest water Level measured.</summary>
        public ushort WaterLevel { get; set; }
        /// <summary> The latest light Level measured.</summary>
        public ushort LightLevel { get; set; }
        /// <summary> The latest temperature Level measured.</summary>
        public ushort TemperatureLevel { get; set; }
    }
}