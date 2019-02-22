using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A model for a growthkit hub.</summary>
    public class GKHub
    {
        /// <summary>The unique identifier of the the SIM card.</summary>
        /// <remarks>An IMSI does not expire, unlike an MSISDN.</remarks>
        [Key] public ulong IMSI { get; set; }
        /// <summary>The sensors linked to this hub unit.</summary>
        public virtual ICollection<GKSensorStick> Sensors { get; set; }

    }
}
