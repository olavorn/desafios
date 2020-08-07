using PagCerto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagCerto.Model
{
    /// <summary>
    /// Transações são operações financeiras originadas de vendas com cartão de crédito. 
    /// Para cada transação, é cobrado uma taxa fixa de R$ 0,90 (independente do número de parcelas)
    /// e para controle dessa movimentação financeira são mantidas as seguintes informações:
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Código identificador numérico;
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// O trâmite de uma solicitação de antecipação progride através das seguintes etapas:
        /// Aguardando análise: A solicitação ainda está na fila aguardando análise da equipe financeira;
        /// Em análise: A equipe financeira está atualmente analisando as transações solicitadas;
        /// Finalizada: Quando a análise da solicitação é encerrada podendo assumir um dos seguintes resultados: aprovada ou reprovada.
        /// </summary>
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// Quando a análise da solicitação é encerrada podendo assumir um dos seguintes resultados: aprovada ou reprovada.
        /// </summary>
        public TransactionResult Result { get; set; }

        /// <summary>
        /// Data em que a transação foi efetuada;
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Data do repasse (caso já tenha ocorrido);
        /// </summary>
        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// Confirmação da adquirente (aprovada ou recusada);
        /// </summary>
        public OperatorResponse OperatorResponse { get; set; }

        /// <summary>
        /// Valor da transação;
        /// </summary>
        public decimal TransactionValue { get; set; }

        /// <summary>
        /// Valor do repasse (valor da transação subtraído a taxa fixa);
        /// </summary>
        public decimal TransferDue { get; set; }

        /// <summary>
        /// Número de parcelas
        /// </summary>
        public int InstallmentsCount { get; set; }

        /// <summary>
        /// Quatro últimos dígitos do cartão (na requisição, o número do cartão deve conter 16 caracteres numéricos, sem espaços)
        /// </summary>
        public int LastFourCard { get; set; }
    }
}
