using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class InstalmentMap : IEntityTypeConfiguration<Instalment>
    {
        public void Configure(EntityTypeBuilder<Instalment> entity)
        {
            entity.ToTable("Parcela");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            entity.Property(p => p.CustomerId).HasColumnName("IdCliente").HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(p => p.PaymentId).HasColumnName("IdPagamento").HasColumnType("bigint").IsRequired();
            entity.Property(p => p.Ammount).HasColumnName("TotalParcela");
            entity.Property(p => p.CreatedAt).HasColumnName("CriadoEm");
            entity.Property(p => p.Number).HasColumnName("NumParcela").HasColumnType("smallint");
            entity.Property(p => p.TotalOf).HasColumnName("TotalParcelas").HasColumnType("smallint");
            entity.Property(p => p.PaidAt).HasColumnName("DatPagamento").HasColumnType("datetime");

            entity.HasOne(i => i.Customer)
                .WithMany(b => b.Instalments)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Payment)
                .WithMany(b => b.Instalments)
                .HasForeignKey(i => i.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
