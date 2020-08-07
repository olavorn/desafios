using api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ServiceModel
{
    public class CardPaymentProcessing
    {
        private ApiDbContext _dbContext;
        private IAcquirerApi _acquirerApi;

        public CardPaymentProcessing(ApiDbContext dbContext, IAcquirerApi acquirerApi)
        {
            _dbContext = dbContext;
            _acquirerApi = acquirerApi;
        }

        public Payment Payment { get; private set; }
        public bool Reproved { get; private set; }
        public bool CardNotSupported { get; private set; }

        public async Task<bool> Process(Payment payment)
        {
            Payment = payment;

            await _acquirerApi.Process(Payment);

            CardNotSupported = Payment.CardTransaction == null;
            if (CardNotSupported) return false;

            Reproved = Payment.CardTransaction.ReprovedAt != null;
            if (Reproved) return false;

            _dbContext.Payments.Add(Payment);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
