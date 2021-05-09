using System;
using Microsoft.AspNetCore.Identity;

namespace Business.ServiceAdapters.AspIdentity.Model
{
    public class AppIdentityUser : IdentityUser
    {
        public AppIdentityUser()
        {

        }

        public string City { get; set; }
        public string Adresse { get; set; }
        public string Picture { get; set; }
        public DateTime? Birthday { get; set; }
        public int  Gender { get; set; }
    }
}
