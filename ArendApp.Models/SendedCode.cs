using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArendApp.Models
{
    public class SendedCode
    {
        public int Id { get; set; }
        [Description("Id пользователя")]
        [Display(Name = "Id пользователя")]
        public int UserId { get; set; }
        [Description("Код")]
        [Display(Name = "Код")]
        public string Code { get; set; }
        [Description("Время действия")]
        [Display(Name = "Время действия")]
        public DateTime Limit { get; set; } = DateTime.Now.AddHours(1);
    }
}
