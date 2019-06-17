using OtpNet;

namespace GrowKitApi.SettingModels
{
    public class TotpSettings
    {
        public OtpHashMode HashMode { get; set; }
        public int ToleratedStepDifference { get; set; }
    }
}
