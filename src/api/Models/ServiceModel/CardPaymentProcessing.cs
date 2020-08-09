using api.Model;
using api.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ServiceModel
{
    public class CardPaymentProcessing
    {
        private apiContext _dbContext;
        private IAcquirerApi _acquirerApi;

        public CardPaymentProcessing(apiContext dbContext, IAcquirerApi acquirerApi)
        {
            _dbContext = dbContext;
            _acquirerApi = acquirerApi;
        }

        public bool Approved { get; private set; }
        public bool CardNotSupported { get; private set; }

        public async Task<bool> Process(ICardTransaction payment)
        {
            await _acquirerApi.Process(payment).ConfigureAwait(false);

            return payment.Result == Enums.PaymentResponse.Approved;
        }
    }
}
