using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.DTOs
{
    public class RestaurantDTO
    {
        public int ResId { get; set; }

        [Required(ErrorMessage = "Please enter restaurant name")]
        public string ResName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter restaurant email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please enter restaurant address")]
        public string Address { get; set; } = null!;
    }
}
