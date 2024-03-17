using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class UserInventory : UserProductsBase
    {
        [Description("Взят в аренду")]
        [Display(Name = "Взят в аренду")]
        [Required]
        public DateTime StartPeriod { get; set; }
        [Required]
        [Description("Окончание аренды")]
        [Display(Name = "Окончание аренды")]
        public DateTime EndPeriod { get; set; }
    }
}
