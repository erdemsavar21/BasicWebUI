﻿using System;
namespace Core.Utilities.Security.Jwt
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