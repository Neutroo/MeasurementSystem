using MeasurementSystemWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MeasurementSystemWebAPI.Dto
{
    public class POSTUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public User ToDomain()
            => new()
            {
                Username = Username,
                Password = Password
            };
    }
}
