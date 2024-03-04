using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArendApp.App.Models
{
    public class SendedCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime Limit { get; set; } = DateTime.Now.AddHours(1);
    }
}
