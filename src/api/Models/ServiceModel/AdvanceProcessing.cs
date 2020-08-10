using api.Extensions;
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
    public class AdvanceProcessing
    {
        private apiContext _dbContext;
        private readonly IAccountApi _account;
        private readonly ILogger _log;
        private CardPaymentProcessing _cardProcessing;
        private const decimal _advanceTax = .038m;

        public AdvanceProcessing(apiContext dbContext, IAcquirerApi acquirer, IAccountApi account, ILogger log)
        {
            _dbContext = dbContext;
            _account = account;
            _log = log;
            _cardProcessing = new CardPaymentProcessing(dbContext, acquirer);
        }

        public bool Approved { get; private set; }
        public bool CardNotSupported { get; private set; }

        public async Task<Advance> Request(Advance advance, string token)
        {
            if (_dbContext.Advances.Any(q => q.IsApproved == null && q.EvaluationDateStart.HasValue))
                throw new Exception("Advance Already Queued. There can be only one Request queued for evaluation at a time.");

            try
            {
                var customer = await _account.WhoAmI(token);

                advance.CustomerId = customer.CustomerId;
                advance.RequestDate = DateTime.UtcNow;
                
                var pList = advance.Payments.Select(p => p.Id).ToList();
                var pays = _dbContext.Payments.IncludeInstalments().Where(q => q.CustomerId == advance.CustomerId && pList.Contains(q.Id));
                advance.Payments = pays.ToList();

                foreach (var pay in pays)
                {
                    advance.GrossAmount += pay.Instalments.Where(i => !i.PaidAt.HasValue).Sum(i => i.Ammount);
                    advance.FixedTaxes += pay.Instalments.Where(i => !i.PaidAt.HasValue).Sum(i => i.FixedTax);
                    
                    foreach (var instalment in pay.Instalments.Where(i => !i.PaidAt.HasValue)) {
                        decimal _advanceTax = GetAdvanceTaxRate(DateTime.Now, instalment.TargetDate);
                        advance.AdvanceTaxes += instalment.Ammount * _advanceTax;
                    }
                }

                advance.NetAmount = advance.GrossAmount - advance.FixedTaxes - advance.AdvanceTaxes;

                _dbContext.Advances.Add(advance);

                return advance;
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "There was an error while processing the Payment Request" );
                return null;
            }
        }

        public async Task<Advance> BeginEvaluation(Advance advance, string authAdminToken)
        {
            var admin = await _account.WhoAdminAmI(authAdminToken);

            if (admin == null)
                return null;

            var adv = _dbContext.Advances.Single(q => q.Id == advance.Id);
            adv.EvaluationDateStart = DateTime.Now;
            adv.EvaluationBy = admin.AdminId;

            return adv;
        }

        public Task<Advance> EndEvaluation(Advance advance, bool isApproved, string authToken)
        {
            throw new NotImplementedException();
        }

        private decimal GetAdvanceTaxRate(DateTime referenceDate, DateTime targetDate)
        {
            if (referenceDate > targetDate)
                return -1;

            short i = 1;
            while (referenceDate.AddDays(i * 30) <= targetDate) i++;

            return i * _advanceTax;
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
