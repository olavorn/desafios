using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public static class PaymentMap
    {
        public static void Map(this EntityTypeBuilder<Payment> entity)
        {
            entity.ToTable(nameof(Payment));

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.PaidAt).HasColumnName("DataPagamento").IsRequired();
            entity.Property(p => p.TransferDate).HasColumnName("DataRepasse");
            entity.Property(p => p.OperatorResponse).HasColumnName("ConfirmacaoAdquirente");
            entity.Property(p => p.Amount).HasColumnName("Valor").HasColumnType("decimal(8,2)");
            entity.Property(p => p.CardLastDigits).HasColumnName("DigitosCartao").HasColumnType("smallint");
        }
    }
}
