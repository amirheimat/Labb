using System.ComponentModel;

namespace Supermarket.API.Domain.Models
{
    public enum EBand : byte
    {
        [Description("Wifi")]
        Wifi = 1,

        [Description("4G")]
        FourG = 2,

        [Description("Wifi & 4G")]
        WifiFourG = 3,
    }
}
