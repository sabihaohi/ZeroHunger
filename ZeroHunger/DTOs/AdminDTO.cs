namespace ZeroHunger.DTOs
{
    public class AdminDTO
    {
        public int AdminId { get; set; }

        public string AdminName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
