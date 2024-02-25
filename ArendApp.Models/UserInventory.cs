using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class UserInventory : UserProductsBase
    {
        [Required]
        public DateTime StartPeriod { get; set; }
        [Required]
        public DateTime EndPeriod { get; set; }
    }
}
