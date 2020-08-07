using api.Models.Enums;
using System;


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
    public class Payment
    {
        /// <summary>
        /// Código identificador numérico;
        /// </summary>
        public int Id { get; set; }

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
        public DateTime? PaidAt { get; set; }

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
        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// Valor do repasse (valor da transação subtraído a taxa fixa);
        /// </summary>
        public decimal? TransferDue { get; set; }

        /// <summary>
        /// Número de parcelas
        /// </summary>
        public int InstallmentsCount { get; set; }

        /// <summary>
        /// Quatro últimos dígitos do cartão (na requisição, o número do cartão deve conter 16 caracteres numéricos, sem espaços)
        /// </summary>
        public short CardLastDigits { get; set; }
    }
   
}
