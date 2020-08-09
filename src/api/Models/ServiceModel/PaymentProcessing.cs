using api.Model;
using api.Models.EntityModel;
using api.Models.IntegrationModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ServiceModel
{
    public class PaymentProcessing
    {
        private apiContext _dbContext;
        private readonly IAccountApi _account;
        private readonly ILogger _log;
        private CardPaymentProcessing _cardProcessing;

        public PaymentProcessing(apiContext dbContext, IAcquirerApi acquirer, IAccountApi account, ILogger log)
        {
            _dbContext = dbContext;
            _account = account;
            _log = log;
            _cardProcessing = new CardPaymentProcessing(dbContext, acquirer);
        }

        public bool Approved { get; private set; }
        public bool CardNotSupported { get; private set; }

        public async Task<bool> Process(Payment payment, string token)
        {
            try
            {
                var customer = await _account.WhoAmI(token);
                var result = await _cardProcessing.Process(payment).ConfigureAwait(false);

                payment.CreatedAt = DateTime.UtcNow;
                payment.CardLastDigits = GetLastFourCardDigits(payment.CardDigits);

                payment.Instalments = new List<Instalment>();
                var instalmentValues = ApplyInstalmentValues(payment.Amount, payment.InstalmentsCount);

                for (short i = 1; i < payment.InstalmentsCount + 1; i++)
                {
                    payment.Instalments.Add(new Instalment()
                    {
                        Ammount = instalmentValues[i - 1],
                        CreatedAt = payment.CreatedAt,
                        CustomerId = customer.CustomerId,
                        Number = i,
                        TotalOf = payment.InstalmentsCount,
                        TotalAmmount = payment.Amount
                    });
                }

                payment.Result = result ? Enums.PaymentResponse.Approved : Enums.PaymentResponse.Rejected;

                _dbContext.Payments.Add(payment);

                return true;
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "There was an error while processing the Payment Request" );
                return false;
            }
        }

        private decimal[] ApplyInstalmentValues(decimal amount, short instalmentsCount)
        {
            //não sei como a divisão pelo cartão de crédito é controlada em um caso real para este segmento. 
            //Desta forma para evitar perda de centavo nas dízimas, utilizei este médoto.
            var division = Decimal.Divide(amount, instalmentsCount);
            var result = new decimal[instalmentsCount];
            result[0] = 0m;
            for (var i = instalmentsCount - 1; i > 0; i--)
            {
                result[i] = division;
            }
            result[0] = amount - result.Sum();
            return result;
        }

        private short GetLastFourCardDigits(string cardDigits)
        {
            if (cardDigits.Length == 16)
                return short.Parse(cardDigits.Substring(12));
            else
                return -1;
        }
    }
}
