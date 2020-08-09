using api.Models.EntityModel;
using api.Models.IntegrationModel;
using api.Models.ServiceModel;
using System;

namespace tests
{
    public class FakeAccountApiFactory
    {
        private readonly apiContext _context;

        public FakeAccountApiFactory(apiContext context)
        {
            _context = context;
        }

        internal IAccountApi CreateAccountApi()
        {
            return new AccountApi(new AccountApiOptions()
            {
                UseInternal = true,
                DbSource = _context
            });
        }
    }
}