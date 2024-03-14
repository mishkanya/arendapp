

using System;

namespace ArendApp.App.Models
{
    public class User: ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Confirmed { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }

        public object Clone() => new User { Id = Id, Name = Name, Email = Email, Password = Password, Confirmed = Confirmed, IsAdmin = IsAdmin, Token = Token };
    }
}
