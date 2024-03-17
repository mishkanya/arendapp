using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class UserProductsBase
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Description("Id пользователя")]
        [Display(Name = "Id пользователя")]
        public int UsedId { get; set; }
        [Required]
        [Description("Id товара")]
        [Display(Name = "Id товара")]
        public int ProductId { get; set; }
    }
}
