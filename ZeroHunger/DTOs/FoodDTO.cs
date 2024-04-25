using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.DTOs
{
    public class FoodDTO
    {
        public int FoodId { get; set; }

        [Required(ErrorMessage = "Please enter food name")]
        public string FoodName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter quantity.")]
        [Range(0.1, 100, ErrorMessage = "Quantity can't be 0 or less.")]
        public float Quantity { get; set; }

        public int ReqId { get; set; }
    }
}
