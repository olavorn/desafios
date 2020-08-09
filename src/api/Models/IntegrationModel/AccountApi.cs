using api.Extensions;
using api.Models.EntityModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace api.Models.IntegrationModel
{
    public class AccountApi : IAccountApi
    {
        private AccountApiOptions _options;
        private readonly apiContext _dbContext;
        private HttpClient _httpClient;

        public AccountApi(IOptions<AccountApiOptions> options)
        {
            _options = options.Value;

            if (!_options.UseInternal)
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                _dbContext = options.Value.DbSource;
            }
        }

        public async Task<WhoAmI> WhoAmI(string token)
        {
            if (!_options.UseInternal)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);

                var response = await _httpClient.PostAsJsonAsync(_options.WhoAmIUrl, token);

                return response.IsSuccessStatusCode ? await response.Content.ReadAsJsonAsync<WhoAmI>() : null;
            }
            else
            {
                var wai = token.TranslateToken();
                return _dbContext.Customers.FirstOrDefault(q => q.Id == wai.CustomerId).MapToWhoAmI();
            }
        }
        
    }

    
}
