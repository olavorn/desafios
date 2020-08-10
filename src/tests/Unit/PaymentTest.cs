using api.Controllers;
using api.Extensions;
using api.Model;
using api.Models.EntityModel;
using api.Models.Enums;
using api.Models.IntegrationModel;
using api.Models.ResultModel;
using api.Models.ServiceModel;
using api.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    /// <summary>
    /// Classe de Testes
    /// </summary>
    /// <remarks>
    /// Testes funcionando, porém é necessário rodá-los 1 a 1
    /// </remarks>
    public class PaymentTest
    {
        #region Setup

        public IConfiguration Configuration { get; set; }
        public ILogger FakeLogger { get; set; }
        public apiContext DbContext { get; set; }
        public IAcquirerApi FakeAcquirer { get; set; }
        public IAccountApi FakeAccount { get; set; }

        public PaymentTest()
        {
            
        }

        public void SetupTest()
        {
            var options = new DbContextOptionsBuilder<apiContext>()
                .UseInMemoryDatabase(databaseName: "ApiDatabase")
                .Options;

            Configuration = BuildConfiguration();
            FakeLogger = BuildLogger();
            DbContext = new apiContext(Configuration, options);
            FakeAcquirer = BuildAcquirerApiService();
            FakeAccount = BuildFakeAccountService(DbContext);
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

        #endregion

        /// <summary>
        /// Realizar pagamento com cartão de crédito (transação);
        /// </summary>
        [Fact]
        public async Task CreditCardPaymentTest()
        {
            SetupTest();

            var customer = new CustomerModel()
            {
                Name = "Olavo Neto",
                Email = "olavo@exodus.eti.br",
            };

            DbContext.Customers.Add(customer.Map());
            DbContext.SaveChanges();

            var registeredCustomer = await DbContext.Customers.FirstOrDefaultAsync();
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

            PaymentController controller = new PaymentController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
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
            Assert.Equal(1, await DbContext.Customers.CountAsync());

            //payment
            Assert.Equal(1, await DbContext.Payments.CountAsync());
            Assert.Equal(5, (await DbContext.Payments.Include( p=>p.Instalments).FirstAsync()).Instalments.Count );
            Assert.Equal(3333, (await DbContext.Payments.FirstAsync()).CardLastDigits );
            Assert.Equal(20m, (await DbContext.Payments.Include(p => p.Instalments).FirstAsync()).Instalments[0].Ammount);
            Assert.Equal("Olavo Neto", (await DbContext.Payments.Include(p => p.Customer).FirstAsync() ).Customer.Name );

            //instalments
            Assert.Equal(5, await DbContext.Instalments.CountAsync());
            Assert.Equal(20m, (await DbContext.Instalments.FirstAsync()).Ammount);
            Assert.Equal(0.9m, (await DbContext.Instalments.FirstAsync()).FixedTax);
            Assert.Equal(0.0m, (await DbContext.Instalments.FirstAsync()).AdvanceTax);
            Assert.Equal(DateTime.Today.AddDays(30), (await DbContext.Instalments.FirstAsync()).TargetDate);
            Assert.Equal(DateTime.Today.AddDays(90), (await DbContext.Instalments.ToListAsync())[2].TargetDate);
            Assert.Equal(DateTime.Today.AddDays(120), (await DbContext.Instalments.ToListAsync())[3].TargetDate);
            Assert.Equal(DateTime.Today.AddDays(150), (await DbContext.Instalments.LastAsync()).TargetDate);

            Assert.Equal(0.0m, (await DbContext.Instalments.LastAsync()).FixedTax);
            Assert.Equal(0.0m, (await DbContext.Instalments.LastAsync()).AdvanceTax);
            Assert.Equal(19.1m, (await DbContext.Instalments.FirstAsync()).NetAmmount);
            Assert.Equal(customer.Id, (await DbContext.Instalments.Include(p => p.Customer).ToListAsync())[2].CustomerId);
            Assert.Equal(100m, (await DbContext.Instalments.Include(p => p.Customer).ToListAsync())[2].AllInstallments);
            Assert.Equal(4, (await DbContext.Instalments.Include(p => p.Customer).ToListAsync())[3].Number);
            Assert.NotNull((await DbContext.Instalments.LastAsync()).CreatedAt);
            Assert.Equal(100,(await DbContext.Instalments.LastAsync()).AllInstallments);
            Assert.Equal(5, (await DbContext.Instalments.LastAsync()).TotalOf);
        }

        

        /// <summary>
        /// Consultar transações disponíveis para antecipar 
        /// (Os valores já devem estar calculados, visando transparência 
        /// e possibilitando o planejamento financeiro do nosso cliente);
        /// </summary>
        [Fact]
        public async Task CheckAvailablePaymentsTest()
        {
            SetupTest();

            var customer = new CustomerModel()
            {
                Name = "Olavo Neto",
                Email = "olavo@exodus.eti.br",
            };

            DbContext.Customers.Add(customer.Map());
            DbContext.SaveChanges();

            var registeredCustomer = await DbContext.Customers.FirstOrDefaultAsync();
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

            var paymentModel2 = new PaymentModel()
            {
                Customer = customer,
                Amount = 150,
                InstalmentsCount = 5,
                CardNumber = "5555111122223333",
                CardExpirationDate = "08/2021",
                CardName = "OLAVO DE SOUZA ROCHA NETO",
                CardSecurityNumber = "558",
                AuthToken = waiToken
            };

            PaymentController controller = new PaymentController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            var cResponse1 = await controller.ProcessPayment(paymentModel);
            var cResponse2 = await controller.ProcessPayment(paymentModel2);

            var paymentListModel = new PaymentListModel()
            {
                AuthToken = waiToken
            };
            var cResponse3 = await controller.ListAvailableForAdvance(paymentListModel);
            Assert.Equal(2, cResponse3.Count);
        }

        /// <summary>
        /// Solicitar antecipação de transações informadas
        /// </summary>
        /// <remarks>
        /// Solicitações de antecipação são documentos emitidos pelo lojista/prestador através do nosso serviço. 
        /// A antecipação de uma transação tem um custo de 3.8% por parcela (ex: 1x -> 3,8%, 2x -> 3,8 * 2 = 7,6%), 
        /// sendo automaticamente debitado no seu repasse, já descontado o custo fixo. 
        /// Dessa forma, ao antecipar, o lojista recebe por todas as parcelas da transação no mesmo
        /// dia que a solicitação foi atendida. Para controle dessas solicitações, 
        /// são mantidas as seguintes informações:
        /// </remarks>
        [Fact]
        public async Task RequestForAdvancePaymentTest()
        {
            SetupTest();

            var customer = new CustomerModel()
            {
                Name = "Olavo Neto",
                Email = "olavo@exodus.eti.br",
            };

            DbContext.Customers.Add(customer.Map());
            DbContext.SaveChanges();

            var registeredCustomer = await DbContext.Customers.FirstOrDefaultAsync();
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

            var paymentModel2 = new PaymentModel()
            {
                Customer = customer,
                Amount = 150,
                InstalmentsCount = 5,
                CardNumber = "5555111122223333",
                CardExpirationDate = "08/2021",
                CardName = "OLAVO DE SOUZA ROCHA NETO",
                CardSecurityNumber = "558",
                AuthToken = waiToken
            };

            PaymentController controller = new PaymentController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            await controller.ProcessPayment(paymentModel);
            await controller.ProcessPayment(paymentModel2);

            var paymentListModel = new PaymentListModel()
            {
                AuthToken = waiToken
            };
            var firstPaymentForAdvance = 
                new AdvanceListModel()
                {
                    AuthToken = waiToken,
                    Payments = new[] { (await controller.ListAvailableForAdvance(paymentListModel)).Payments.First() }.Select(q => q.Id)
                };

            var adv = await controller.RequestForAdvance(firstPaymentForAdvance);
            Assert.Equal(150, adv.GrossAmount);
            var percentTax = 0.038m;
            var instalment = decimal.Divide(150, 5);
            Assert.Equal(150m - 0.9m - (1m* percentTax) * instalment - (2m * percentTax) * instalment - (3m * percentTax) * instalment - (4m * percentTax) * instalment - (5m * percentTax) * instalment, adv.NetAmount);
        }

        /// <summary>
        /// Consultar os detalhes da solicitação em andamento(devendo retornar, também, a lista de transações da antecipação);
        /// </summary>
        [Fact]
        public async Task GetAdvancePaymentRequestDetailsTest()
        {
            SetupTest();

            #region ContextSetup 

            var customer = new CustomerModel()
            {
                Name = "Olavo Neto",
                Email = "olavo@exodus.eti.br",
            };

            DbContext.Customers.Add(customer.Map());
            DbContext.SaveChanges();

            var registeredCustomer = await DbContext.Customers.FirstOrDefaultAsync();
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

            var paymentModel2 = new PaymentModel()
            {
                Customer = customer,
                Amount = 150,
                InstalmentsCount = 5,
                CardNumber = "5555111122223333",
                CardExpirationDate = "08/2021",
                CardName = "OLAVO DE SOUZA ROCHA NETO",
                CardSecurityNumber = "558",
                AuthToken = waiToken
            };

            PaymentController controller = new PaymentController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            await controller.ProcessPayment(paymentModel);
            await controller.ProcessPayment(paymentModel2);

            var paymentListModel = new PaymentListModel()
            {
                AuthToken = waiToken
            };
            var firstPaymentForAdvance =
                new AdvanceListModel()
                {
                    AuthToken = waiToken,
                    Payments = new[] { (await controller.ListAvailableForAdvance(paymentListModel)).Payments.First() }.Select(q => q.Id)
                };

            var adv = await controller.RequestForAdvance(firstPaymentForAdvance);

            #endregion

            var adModel = new AdvanceDetailsModel()
            {
                AuthToken = waiToken,
                Id = adv.Id
            };
            var radv = await controller.GetAdvanceDetails(adModel);
            Assert.Equal(radv.Id, adModel.Id);
            Assert.Single(radv.Payments);

            var percentTax = 0.038m;
            var instalment = decimal.Divide(150, 5);
            Assert.Equal(150m - 0.9m - (1m * percentTax) * instalment - (2m * percentTax) * instalment - (3m * percentTax) * instalment - (4m * percentTax) * instalment - (5m * percentTax) * instalment, adv.NetAmount);

        }

        /// <summary>
        /// Iniciar o atendimento da solicitação de antecipação;
        /// </summary>
        [Fact]
        public async Task BeginAdvancePaymentRequestEvaluationTest()
        {
            await GetAdvancePaymentRequestDetailsTest();

            var admin = new User()
            {
                Name = "Administrador",
                Email = "admin@pagcerto.com.br",
                IsActive = true,
            };

            DbContext.Users.Add(admin);
            DbContext.SaveChanges();

            var registeredAdmin = await DbContext.Users.FirstOrDefaultAsync();
            var waiAdminToken = registeredAdmin.MapToWhoAdminAmI().EncryptToken();

            AdminController controller = new AdminController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            var model = new AdvanceEvaluationModel()
            {
                AuthToken = waiAdminToken,
                Id = 1
            };

            var advance = await controller.BeginAdvanceEvaluation(model);

            Assert.NotNull(advance.EvaluationDateStart);
            Assert.Null(advance.EvaluationDateEnd);
            Assert.True(DateTime.Now.AddMinutes(-1) < advance.EvaluationDateStart);
            Assert.Equal(admin.Id, advance.EvaluationBy);
            Assert.Equal(1, advance.Id);
        }

        /// <summary>
        /// Não é possível iniciar um Adiantamento enquanto um pedido estiver em aberto
        /// </summary>
        [Fact]
        public async Task RequestAdvanceOnceTest()
        {
            //load the context
            await GetAdvancePaymentRequestDetailsTest();

            var registeredCustomer = await DbContext.Customers.FirstOrDefaultAsync();
            var waiToken = registeredCustomer.MapToWhoAmI().EncryptToken();

            PaymentController controller = new PaymentController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);

            var paymentListModel = new PaymentListModel()
            {
                AuthToken = waiToken
            };

            var seccondPaymentForAdvance = new AdvanceListModel()
            {
                AuthToken = waiToken,
                Payments = new[] { (await controller.ListAvailableForAdvance(paymentListModel)).Payments.ToList()[0] }.Select(q => q.Id)
            };

            var adv2 = await controller.RequestForAdvance(seccondPaymentForAdvance);
            Assert.IsType<AdvanceErrorJson>(adv2);
        }

        /// <summary>
        /// Aprovar uma solicitação de antecipação;
        /// </summary>
        [Fact]
        public async Task AprovePaymentTestAsync()
        {
            await BeginAdvancePaymentRequestEvaluationTest();
            var registeredAdmin = await DbContext.Users.FirstOrDefaultAsync();
            var waiAdminToken = registeredAdmin.MapToWhoAdminAmI().EncryptToken();

            AdminController controller = new AdminController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            var evaluationModel = new AdvanceEvaluationModel()
            {
                AuthToken = waiAdminToken,
                IsApproved = true ,
                Id = 1
            };
            var adv2 = await controller.EndAdvanceEvaluation(evaluationModel);
            Assert.IsType<AdvanceJson>(adv2);
        }

        /// <summary>
        /// reprovar uma solicitação de antecipação;
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RejectPaymentTestAsync()
        {
            await BeginAdvancePaymentRequestEvaluationTest();
            var registeredAdmin = await DbContext.Users.FirstOrDefaultAsync();
            var waiAdminToken = registeredAdmin.MapToWhoAdminAmI().EncryptToken();

            AdminController controller = new AdminController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            var evaluationModel = new AdvanceEvaluationModel()
            {
                AuthToken = waiAdminToken,
                IsApproved = false,
                Id = 1
            };
            var adv2 = await controller.EndAdvanceEvaluation(evaluationModel);
            Assert.IsType<AdvanceJson>(adv2);
        }

        /// <summary>
        /// Consultar histórico das solicitações realizadas em um determinado período(devendo retornar, também, a lista de transações da antecipação).
        /// </summary>
        [Fact]
        public async Task RequestPaymentHistoryTestAsync()
        {
            await AprovePaymentTestAsync();

            var registeredCustomer = await DbContext.Customers.FirstOrDefaultAsync();
            var waiToken = registeredCustomer.MapToWhoAmI().EncryptToken();

            PaymentController controller = new PaymentController(DbContext, FakeAcquirer, FakeAccount, FakeLogger);
            var phr = await controller.ListPaymentHistory(new PaymentListModel()
            {
                AuthToken = waiToken,
                StartDate = DateTime.Today.AddDays(-30),
                EndDate = DateTime.Today.AddDays(30)
            });

            Assert.Equal(DbContext.Payments.Count(), phr.Count);
        }


    }
}
