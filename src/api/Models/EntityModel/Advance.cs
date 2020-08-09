using api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class Advance
    {
        public long Id { get; set; }

        /// <summary>
        /// Usuário que gera a Antecipação (intencionalmente não normalizado por questões de performance. Discutir posteriormente)
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Lista de transações solicitadas na antecipação.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Data da solicitação;
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Data da análise(quando iniciou e quando terminou);
        /// </summary>
        public DateTime? EvaluationDateStart { get; set; }

        /// <summary>
        /// Data da análise(quando iniciou e quando terminou);
        /// </summary>
        public DateTime? EvaluationDateEnd { get; set; }

        /// <summary>
        /// Resultado da análise(aprovado ou reprovado);
        /// </summary>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Valor total das transações solicitadas para antecipação(já descontado a taxa fixa);
        /// </summary>
        public decimal GrossAmount { get; set; }


        /// <summary>
        /// Valor líquido das transações solicitadas para antecipação(já descontado a taxa fixa);
        /// </summary>
        public decimal NetAmount { get; set; }

        /// <summary>
        /// Valor total do repasse(descontado a taxa fixa e a taxa da antecipação)
        /// </summary>
        public decimal AdvanceDue { get; set; }

        /// <summary>
        /// Lista de transações solicitadas na antecipação.
        /// </summary>
        public List<Payment> Payments { get; set; }
        public decimal FixedTaxes { get; set; }
        public decimal AdvanceTaxes { get; set; }
    }
}
