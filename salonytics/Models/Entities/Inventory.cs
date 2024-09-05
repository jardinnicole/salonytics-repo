using System.ComponentModel.DataAnnotations;

namespace salonytics.Models.Entities
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
        public string PhotoPath { get; set; }
    }
}
