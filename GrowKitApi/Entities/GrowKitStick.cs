using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowKitApi.Entities
{
    public class GrowKitStick
    {
        [Key] public long IMSI { get; set; }
        public long OwnerId { get; set; }
        public long MasterStickId { get; set; }
        public int Light { get; set; }
        public int Moisture { get; set; }
        public int Temperature { get; set; }
        public int LightTime { get; set; }
        public int TimestampUpdate { get; set; }

        [ForeignKey("MasterStickId")] public GrowKitStick MasterStick { get; set; }
        [ForeignKey("OwnerId")] public ApplicationUser Owner { get; set; }
    }
}
