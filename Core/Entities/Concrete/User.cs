using System;
using Core.Entities;

namespace Core.Entities.Concrete
{
    public class User : IEntity
    {
        public User()
        {
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Adresse { get; set; }
        public string Picture { get; set; }
        public DateTime? Birthday { get; set; }
        public int Gender { get; set; }
    }
}
