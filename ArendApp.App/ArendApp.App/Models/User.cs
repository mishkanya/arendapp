﻿

namespace ArendApp.App.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Confirmed { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
    }
}
