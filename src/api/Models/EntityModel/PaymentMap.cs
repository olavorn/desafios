using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> entity)
        {
            entity.ToTable("Pagamento");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            entity.Property(p => p.CustomerId).HasColumnName("IdCliente").HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(p => p.CreatedAt).HasColumnName("DataPagamento").IsRequired();
            entity.Property(p => p.PaidAt).HasColumnName("DataRepasse");
            entity.Property(p => p.OperatorResponse).HasColumnName("ConfirmacaoAdquirente");
            entity.Property(p => p.Amount).HasColumnName("Valor").HasColumnType("decimal(8,2)");
            entity.Property(p => p.CardLastDigits).HasColumnName("DigitosCartao").HasColumnType("smallint");
            entity.Property(p => p.CardExpirationDate).HasColumnName("ExpiracaoCartao").HasColumnType("varchar(7)");
            entity.Property(p => p.CardName).HasColumnName("NomeCartao").HasColumnType("varchar(255)");

            entity.HasMany(p => p.Instalments)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Advance)
                .WithMany(a => a.Payments)
                .HasForeignKey(p => p.AdvanceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ForSqlServerHasIndex(q => q.CustomerId);
            entity.ForSqlServerHasIndex(q => q.AdvanceId);
            entity.ForSqlServerHasIndex(q => q.CreatedAt);
            entity.ForSqlServerHasIndex(q => q.PaidAt);
        }
    }
}
