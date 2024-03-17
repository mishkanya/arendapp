using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Description("Имя пользоваьеля")]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }
        [Required]
        [Description("Email пользователя")]
        [Display(Name = "Email пользователя")]
        public string Email { get; set; }
        [Required]
        [Description("Пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Description("Email подтвержден")]
        [Display(Name = "Email подтвержден")]
        public bool Confirmed { get; set; }
        [Description("пользователь - админ")]
        [Display(Name = "пользователь - админ")]
        public bool IsAdmin { get; set; }
        [Description("Токен авторизации")]
        [Display(Name = "Токен авторизации")]
        public string Token { get; set; } = Guid.NewGuid().ToString();
    }
}
