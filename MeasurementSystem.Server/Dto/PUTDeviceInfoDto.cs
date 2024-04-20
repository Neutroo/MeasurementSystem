using System.ComponentModel.DataAnnotations;

namespace MeasurementSystemWebAPI.Dto
{
    public class PUTDeviceInfoDto
    {
        [Required]
        public Dictionary<string, object>  Items { get; set; }
    }
}
