using api.Models.EntityModel;
using api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.Model
{
    /// <summary>
    /// Transações são operações financeiras originadas de vendas com cartão de crédito. 
    /// Para cada transação, é cobrado uma taxa fixa de R$ 0,90 (independente do número de parcelas)
    /// e para controle dessa movimentação financeira são mantidas as seguintes informações:
    /// </summary>
    /// <example>
    /// Toda transação aprovada gera parcelas com vencimento a cada 30 dias, 
    /// ex: Venda de R$100,00 em 2x, gera duas parcelas de R$50,00, 
    /// sendo a primeira com vencimento para 30 dias e a segunda para 60 dias. 
    /// O pagamento da taxa fixa é feito apenas na primeira parcela, 
    /// então para esse exemplo o lojista receberia na primeira parcela R$49,10 e na segunda R$50,00.
    /// </example>
    public class Payment : ICardTransaction
    {
        /// <summary>
        /// Código identificador numérico;
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Código identificador numérico;
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// O trâmite de uma solicitação de antecipação progride através de etapas;
        /// </summary>
        /// <remarks>
        /// - Aguardando análise: A solicitação ainda está na fila aguardando análise da equipe financeira;
        /// - Em análise: A equipe financeira está atualmente analisando as transações solicitadas;
        /// - Finalizada: Quando a análise da solicitação é encerrada podendo assumir um dos seguintes resultados: aprovada ou reprovada.
        /// </remarks>
        public PaymentStatus Status { get; set; }

        /// <summary>
        /// Quando a análise da solicitação é encerrada podendo assumir um dos seguintes resultados: aprovada ou reprovada.
        /// </summary>
        public PaymentResponse Result { get; set; }

        /// <summary>
        /// Data em que a transação foi efetuada;
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Confirmação da adquirente (aprovada ou recusada);
        /// </summary>
        public OperatorResponse OperatorResponse { get; set; }

        /// <summary>
        /// Valor da transação;
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Data do repasse (caso já tenha ocorrido);
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// Valor do repasse (valor da transação subtraído a taxa fixa);
        /// </summary>
        public decimal? TransferDue { get => Instalments?.Where(q => q.PaidAt != null).Sum(q => q.Ammount); }

        /// <summary>
        /// Número de parcelas
        /// </summary>
        public short InstalmentsCount { get; set; }

        /// <summary>
        /// Nome no Cartão
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Card Expiration Date
        /// </summary>
        public string CardExpirationDate { get; set; }

        /// <summary>
        /// Card Security Number
        /// </summary>
        public string CardSecurityNumber { get; set; }

        /// <summary>
        /// Dígitos do cartão (na requisição, o número do cartão deve conter 16 caracteres numéricos, sem espaços)
        /// </summary>
        public string CardDigits { get; set; }

        /// <summary>
        /// Quatro últimos dígitos do cartão (na requisição, o número do cartão deve conter 16 caracteres numéricos, sem espaços)
        /// </summary>
        public short CardLastDigits { get; set; }

        /// <summary>
        /// Referência de Antecipação
        /// </summary>
        public Advance Advance { get; set; }

        /// <summary>
        /// Chave Referência de Antecipação
        /// </summary>
        public long AdvanceId { get; set; }

        /// <summary>
        /// Parcelas
        /// </summary>
        public List<Instalment> Instalments { get; set; }

        /// <summary>
        /// Cliente
        /// </summary>
        public Customer Customer { get; set; }
    }
   
}
