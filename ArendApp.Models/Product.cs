using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Description("Название товара")]
        public string Name { get; set; }
        [Required]
        [Description("Описание товара")]
        public string Description { get; set; }
        [Description("Главное изображение")]
        [Required]
        public string MainImage { get; set; }
        [Description("Список второстепенных изображений")]
        public string SecondImages { get; set; }

        [Required]
        [Description("Цена на один день")]
        public double OncePrice { get; set; }
        [Description("Цена на 3 дня")]
        public double ThreeDayPrice { get; set; }
        [Description("Цена на 7 дней")]
        public double SevenDayPrice { get; set; }
        [Description("Цена на 2 недели")]
        public double TwoWeekPrice { get; set; }
        [Description("Цена на месяц")]
        public double MonthPrice { get; set; }

        [Required]
        [Description("Залог")]
        public double Deposit { get; set; }
        public double OldPrice { get; set; }

        [Required]
        public string Capacity { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public bool IsQuickCharge { get; set; }

    }
}
