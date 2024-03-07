using System.ComponentModel.DataAnnotations;

namespace ArendApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Confirmed { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; } = Guid.NewGuid().ToString();
    }
}
