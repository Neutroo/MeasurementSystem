using MeasurementSystem.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace MeasurementSystem.Server.Dto
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
