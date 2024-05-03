using System.ComponentModel.DataAnnotations;

namespace MeasurementSystem.Server.Dto
{
    public class PUTDeviceInfoDto
    {
        [Required]
        public Dictionary<string, object>  Items { get; set; }
    }
}
