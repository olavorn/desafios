using System;
using Xunit;

namespace tests
{
    public class PaymentTest
    {
        /// <summary>
        /// Realizar pagamento com cart�o de cr�dito (transa��o);
        /// </summary>
        [Fact]
        public void CreditCardPaymentTest()
        {

        }

        /// <summary>
        /// Consultar transa��es dispon�veis para antecipar 
        /// (Os valores j� devem estar calculados, visando transpar�ncia 
        /// e possibilitando o planejamento financeiro do nosso cliente);
        /// </summary>
        [Fact]
        public void CheckAvailablePaymentsTest()
        {

        }

        /// <summary>
        /// Solicitar antecipa��o de transa��es informadas
        /// </summary>
        [Fact]
        public void RequestAdvancePaymentTest()
        {

        }

        /// <summary>
        /// Consultar os detalhes da solicita��o em andamento(devendo retornar, tamb�m, a lista de transa��es da antecipa��o);
        /// </summary>
        [Fact]
        public void GetAdvancePaymentRequestDetailsTest()
        {

        }

        /// <summary>
        /// Iniciar o atendimento da solicita��o de antecipa��o;
        /// </summary>
        [Fact]
        public void BeginAdvancePaymentRequestEvaluationTest()
        {

        }

        /// <summary>
        /// Aprovar ou reprovar uma solicita��o de antecipa��o;
        /// </summary>
        public void AproveRejectPaymentTest()
        {

        }

        /// <summary>
        /// Consultar hist�rico das solicita��es realizadas em um determinado per�odo(devendo retornar, tamb�m, a lista de transa��es da antecipa��o).
        /// </summary>
        public void RequestPaymentHistoryTest()
        {

        }


    }
}
