using api.Models.ResultModel;
using api.Models.ServiceModel;

namespace api.Controllers
{
    public class PaymentErrorJson : PaymentJson
    {
        private PaymentProcessing cardTransactionProcessing;

        public PaymentErrorJson(PaymentProcessing cardTransactionProcessing)
        {
            this.cardTransactionProcessing = cardTransactionProcessing;
        }
    }
}