using System;
using Xunit;

namespace tests
{
    public class PaymentTest
    {
        /// <summary>
        /// Realizar pagamento com cartão de crédito (transação);
        /// </summary>
        [Fact]
        public void CreditCardPaymentTest()
        {

        }

        /// <summary>
        /// Consultar transações disponíveis para antecipar 
        /// (Os valores já devem estar calculados, visando transparência 
        /// e possibilitando o planejamento financeiro do nosso cliente);
        /// </summary>
        [Fact]
        public void CheckAvailablePaymentsTest()
        {

        }

        /// <summary>
        /// Solicitar antecipação de transações informadas
        /// </summary>
        [Fact]
        public void RequestAdvancePaymentTest()
        {

        }

        /// <summary>
        /// Consultar os detalhes da solicitação em andamento(devendo retornar, também, a lista de transações da antecipação);
        /// </summary>
        [Fact]
        public void GetAdvancePaymentRequestDetailsTest()
        {

        }

        /// <summary>
        /// Iniciar o atendimento da solicitação de antecipação;
        /// </summary>
        [Fact]
        public void BeginAdvancePaymentRequestEvaluationTest()
        {

        }

        /// <summary>
        /// Aprovar ou reprovar uma solicitação de antecipação;
        /// </summary>
        public void AproveRejectPaymentTest()
        {

        }

        /// <summary>
        /// Consultar histórico das solicitações realizadas em um determinado período(devendo retornar, também, a lista de transações da antecipação).
        /// </summary>
        public void RequestPaymentHistoryTest()
        {

        }


    }
}
