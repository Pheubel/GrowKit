using OtpNet;

namespace GrowKitApi.SettingModels
{
    /// <summary> The settings used for the otp library.</summary>
    public class TotpSettings
    {
        /// <summary> The hashmode which will be used by the application.</summary>
        public OtpHashMode HashMode { get; set; }
        /// <summary> The tolerated difference in steps forwards and backwards for validation.</summary>
        public int ToleratedStepDifference { get; set; }
    }
}
