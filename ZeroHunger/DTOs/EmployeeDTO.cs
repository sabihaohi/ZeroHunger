using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.DTOs
{
    public class EmployeeDTO
    {
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Please enter employee name")]
        public string EmpName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter employee email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; } = null!;

        [Range(0.1, 100000, ErrorMessage = "Salary can't be 0 or less.")]
        public float Salary { get; set; }

        [Required(ErrorMessage = "Please enter employee address")]
        public string Address { get; set; } = null!;
    }
}
