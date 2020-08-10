using api.Models.EntityModel;
using api.Models.IntegrationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class AccountApiExtensions
    {
        public static WhoAmI TranslateToken(this string token)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<WhoAmI>(Encoding.UTF8.GetString(Convert.FromBase64String(token)));
        }

        public static WhoAdminAmI TranslateAdminToken(this string adminToken)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<WhoAdminAmI>(Encoding.UTF8.GetString(Convert.FromBase64String(adminToken)));
        }

        public static WhoAmI MapToWhoAmI(this Customer customer)
        {
            return new WhoAmI()
            {
                CustomerId = customer.Id,
                Email = customer.Email,
                Name = customer.Name,
                SessionId = new Random().Next(99999).ToString()
            };
        }

        public static WhoAdminAmI MapToWhoAdminAmI(this User user)
        {
            return new WhoAdminAmI()
            {
                AdminId = user.Id,
                Email = user.Email,
                Name = user.Name,
                SessionId = new Random().Next(99999).ToString()
            };
        }

        public static string EncryptToken(this WhoAmI user)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(user)), Base64FormattingOptions.None);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(dataAsString);
        }
        
    }
}
