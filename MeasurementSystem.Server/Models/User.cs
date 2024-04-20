using System.ComponentModel.DataAnnotations;

namespace MeasurementSystemWebAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}
