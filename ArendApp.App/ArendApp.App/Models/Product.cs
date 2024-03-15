using ArendApp.App.Services;
using System.ComponentModel;
using System.Linq;

namespace ArendApp.App.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Description("Название товара")]
        public string Name { get; set; }
        [Description("Описание товара")]
        public string Description { get; set; }
        [Description("Главное изображение")]
        public string MainImage { get; set; }
        [Description("Список второстепенных изображений")]
        public string SecondImages { get; set; }

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

        [Description("Залог")]
        public double Deposit { get; set; }
        public double OldPrice { get; set; }

        public string Capacity { get; set; }
        public string Brand { get; set; }
        public bool IsQuickCharge { get; set; }


        public void SetImgUrl()
        {
            if (MainImage.StartsWith("/"))
                MainImage = $"{ApiService.ServerUrl}{MainImage}";
            SecondImages = string.Join(" | ", SecondImages.Split('|').Select(t => t.StartsWith("/") ? $"{ApiService.ServerUrl}{t}" : t));
        }
    }
}
