using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebUI.Extensions
{
    public static class SessionExtensionMethods
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            string objectstring = JsonConvert.SerializeObject(value);
            session.SetString(key, objectstring);
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            string objectString = session.GetString(key);

            if (String.IsNullOrEmpty(objectString))
            {
                return null;
            }

            T value = JsonConvert.DeserializeObject<T>(objectString);

            return value;
        }


    }
}
