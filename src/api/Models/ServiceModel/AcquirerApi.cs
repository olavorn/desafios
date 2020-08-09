using api.Model;
using api.Models.EntityModel;
using System.Threading.Tasks;

namespace api.Models.ServiceModel
{
    public class AcquirerApi : IAcquirerApi
    {
        public async Task Process(ICardTransaction payment)
        {
            if (payment.CardDigits?.Length != 16)
                throw new System.Exception("Acquirer Error: Card Invalid");

            if (payment.CardSecurityNumber?.Length != 3)
                throw new System.Exception("Acquirer Error: Security Number Incorrect");

            if (payment.CardExpirationDate?.Length != 7)
                throw new System.Exception("Acquirer Error: Card Expiration Date Invalid. Should be MM/YYYY");

            payment.Result = Enums.PaymentResponse.Approved;

            await Task.CompletedTask;
        }
    }
}