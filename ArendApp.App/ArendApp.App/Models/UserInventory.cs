

using System;

namespace ArendApp.App.Models
{
    public class UserInventory : UserProductsBase
    {
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
    }
}
