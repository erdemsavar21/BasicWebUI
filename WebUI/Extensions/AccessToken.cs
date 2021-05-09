using System;
namespace WebUI.Extensions
{
    public class AccessToken
    {
        public AccessToken()
        {
        }

        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
