using api.Models.EntityModel;
using System;
using System.Collections.Generic;

namespace api.Model
{
    public class Instalment
    {
        /// <summary>
        /// Id do Cliente
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Id da Transação
        /// </summary>
        public long PaymentId { get; set; }

        /// <summary>
        /// Id da Parcela
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Total da Parcela
        /// </summary>
        public decimal Ammount { get; set; }

        /// <summary>
        /// Total do Pagamento
        /// </summary>
        public decimal AllInstalments { get; set; }

        /// <summary>
        /// Data do Pagamento
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// Número da Parcela
        /// </summary>
        public short Number { get; set; }

        /// <summary>
        /// Total de Parcelas
        /// </summary>
        public short TotalOf { get; set; }

        /// <summary>
        /// Data de Criação da Parcela
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Referência do Cliente
        /// </summary>
        public Customer Customer { get; set; }

        public Payment Payment { get; set; }

        /// <summary>
        /// Data Alvo para Pagamento
        /// </summary>
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Taxa Fixa
        /// </summary>
        public decimal FixedTax { get; set; }

        /// <summary>
        /// Taxa de Antecipação Aplicada
        /// </summary>
        public decimal AdvanceTax { get; set; }

        /// <summary>
        /// Líquido a Receber
        /// </summary>
        public decimal NetAmmount { get => Ammount - FixedTax - AdvanceTax; }
    }
}