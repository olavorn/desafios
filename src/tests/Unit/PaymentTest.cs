using api.Controllers;
using api.Extensions;
using api.Model;
using api.Models.EntityModel;
using api.Models.Enums;
using api.Models.IntegrationModel;
using api.Models.ServiceModel;
using api.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class PaymentTest
    {
        /// <summary>
        /// Realizar pagamento com cart�o de cr�dito (transa��o);
        /// </summary>
        [Fact]
        public async Task CreditCardPaymentTest()
        {
            var options = new DbContextOptionsBuilder<apiContext>()
            .UseInMemoryDatabase(databaseName: "ApiDatabase")
            .Options;
            

            var configuration = BuildConfiguration();
            var fakeLogger = BuildLogger();
            var fakeAcquirer = BuildAcquirerApiService();
            

            var dbContext = new apiContext(configuration, options);
            var fakeAccount = BuildFakeAccountService(dbContext);

            var customer = new CustomerModel()
            {
                Name = "Olavo Neto",
                Email = "olavo@exodus.eti.br",
            };

            dbContext.Customers.Add(customer.Map());
            dbContext.SaveChanges();
            var registeredCustomer = await dbContext.Customers.FirstOrDefaultAsync();
            var waiToken = registeredCustomer.MapToWhoAmI().EncryptToken();
            customer.Id = registeredCustomer.Id;

            var paymentModel = new PaymentModel()
            {
                Customer = customer,
                Amount = 100,
                InstalmentsCount = 5,
                CardNumber = "5555111122223333",
                CardExpirationDate = "08/2021",
                CardName = "OLAVO DE SOUZA ROCHA NETO",
                CardSecurityNumber = "558",
                AuthToken = waiToken
            };
            var before = DateTime.Now;

            PaymentController controller = new PaymentController(dbContext, fakeAcquirer, fakeAccount, fakeLogger);
            var cResponse = await controller.ProcessPayment(paymentModel);

            Assert.IsNotType<PaymentErrorJson>(cResponse);

            Assert.NotNull(cResponse);
            Assert.Equal(100M, cResponse.Amount);
            Assert.Equal(5, cResponse.InstalmentsCount);
            Assert.Null(cResponse.CanceledAt);
            Assert.Equal(OperatorResponse.Acepted, cResponse.OperatorResponse);
            Assert.Null(cResponse.TransferDate);

            //pelo contexto

            //Customers
            Assert.Equal(1, await dbContext.Customers.CountAsync());

            //payment
            Assert.Equal(1, await dbContext.Payments.CountAsync());
            Assert.Equal(5, (await dbContext.Payments.Include( p=>p.Instalments).FirstAsync()).Instalments.Count );
            Assert.Equal(3333, (await dbContext.Payments.FirstAsync()).CardLastDigits );
            Assert.Equal(20m, (await dbContext.Payments.Include(p => p.Instalments).FirstAsync()).Instalments[0].Ammount);
            Assert.Equal("Olavo Neto", (await dbContext.Payments.Include(p => p.Customer).FirstAsync() ).Customer.Name );

            //instalments
            Assert.Equal(5, await dbContext.Instalments.CountAsync());
            Assert.Equal(20m, (await dbContext.Instalments.FirstAsync()).Ammount);
            Assert.Equal(customer.Id, (await dbContext.Instalments.Include(p => p.Customer).ToListAsync())[2].CustomerId);
            Assert.Equal(100m, (await dbContext.Instalments.Include(p => p.Customer).ToListAsync())[2].TotalAmmount);
            Assert.Equal(4, (await dbContext.Instalments.Include(p => p.Customer).ToListAsync())[3].Number);
            Assert.NotNull((await dbContext.Instalments.LastAsync()).CreatedAt);
            Assert.Equal(100,(await dbContext.Instalments.LastAsync()).TotalAmmount);
            Assert.Equal(5, (await dbContext.Instalments.LastAsync()).TotalOf);
        }

        private IAccountApi BuildFakeAccountService(apiContext dbContext)
        {
            var acquirer = new FakeAccountApiFactory(dbContext).CreateAccountApi();
            return acquirer;
        }

        private ILogger BuildLogger()
        {
            var logger = new LoggerFactory().CreateLogger("api");
            return logger;
        }

        private IAcquirerApi BuildAcquirerApiService()
        {
            var acquirer = new FakeAcquirerFactory().CreateAcquirer();
            return acquirer;
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
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
        /// <remarks>
        /// Solicita��es de antecipa��o s�o documentos emitidos pelo lojista/prestador atrav�s do nosso servi�o. 
        /// A antecipa��o de uma transa��o tem um custo de 3.8% por parcela (ex: 1x -> 3,8%, 2x -> 3,8 * 2 = 7,6%), 
        /// sendo automaticamente debitado no seu repasse, j� descontado o custo fixo. 
        /// Dessa forma, ao antecipar, o lojista recebe por todas as parcelas da transa��o no mesmo
        /// dia que a solicita��o foi atendida. Para controle dessas solicita��es, 
        /// s�o mantidas as seguintes informa��es:
        /// </remarks>
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
