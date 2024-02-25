using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class UserProductsBase
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UsedId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
