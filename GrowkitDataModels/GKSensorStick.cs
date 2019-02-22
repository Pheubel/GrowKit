using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A model for the sensor stick.</summary>
    public class GKSensorStick
    {
        /// <summary> The unique identity of the sensor stick.</summary>
        [Key] public ulong SensorId { get; set; }
        /// <summary> The linked hub responsible for this sensor stick.</summary>
        public virtual GKHub ConnectedHub { get; set; }
        /// <summary> The name given to the plant.</summary>
        public string PlantName { get; set; }
        /// <summary> The latest water Level measured.</summary>
        public short WaterLevel { get; set; }
        /// <summary> The latest light Level measured.</summary>
        public short LightLevel { get; set; }
        /// <summary> The latest temperature Level measured.</summary>
        public short TemperatureLevel { get; set; }
        /// <summary> The latest Ph Level measured.</summary>
        public short PhLevel { get; set; }
    }
}
