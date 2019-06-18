using System.ComponentModel.DataAnnotations;

namespace GrowKitApi.Entities
{
    public class PlantPreset
    {
        [Key]
        public int PresetId { get; set; }
    }
}
