using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowKitApi.Entities
{
    /// <summary> The database representation of the growkit stick.</summary>
    public class GrowKitStick
    {
        /// <summary> The id of the stick stored in the database.</summary>
        [Key]
        public long Id { get; set; }
        /// <summary> The Id of the owner of the stick.</summary>
        public long OwnerId { get; set; }
        /// <summary> The id of the stick managing the messaging of this stick.</summary>
        public long MasterStickId { get; set; }
        /// <summary> The id of current active preset on the stick.</summary>
        public int? ActivePresetId { get; set; }
        /// <summary> The current value for the light level.</summary>
        public int Light { get; set; }
        /// <summary> The current value for the moisture level.</summary>
        public int Moisture { get; set; }
        /// <summary> The current value for the temperature level.</summary>
        public int Temperature { get; set; }
        /// <summary> The amount of hours the plant has been in direct light.</summary>
        public int LightTime { get; set; }
        /// <summary> The ammount of seconds after the epoch of the growkit.</summary>
        public int TimestampUpdate { get; set; }

        /// <summary> The stick managing the messaging of this stick.</summary>
        [ForeignKey("MasterStickId")] public GrowKitStick MasterStick { get; set; }
        /// <summary>  The current active preset on the stick.</summary>
        [ForeignKey("ActivePresetId")]
        public PlantPreset ActivePreset { get; set; }
        /// <summary> The owner of the stick.</summary>
        [ForeignKey("OwnerId")] public ApplicationUser Owner { get; set; }
    }
}
