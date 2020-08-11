using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class AdvanceMap : IEntityTypeConfiguration<Advance>
    {
        public void Configure(EntityTypeBuilder<Advance> entity)
        {
            entity.ToTable("Adiantamento");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnType("bigint").ValueGeneratedOnAdd();

            entity.Property(p => p.RequestDate).HasColumnName("DataRequisicao").HasColumnType("datetime");
            entity.Property(p => p.EvaluationDateEnd).HasColumnName("DataPagamentoFim").IsRequired();
            entity.Property(p => p.EvaluationDateStart).HasColumnName("DataPagamentoInicio").IsRequired();
            entity.Property(p => p.EvaluationBy).HasColumnName("AvaliadoPor");
            entity.Property(p => p.AdvanceDue).HasColumnName("TotalRepasse").HasColumnType("decimal(8,2)").IsRequired();
            entity.Property(p => p.IsApproved).HasColumnName("DataRepasse");
            entity.Property(p => p.AdvanceTaxes).HasColumnName("TaxaAntecipacao").HasColumnType("decimal(8,2)");
            entity.Property(p => p.FixedTaxes).HasColumnName("TaxaFixa").HasColumnType("decimal(8,2)");
            entity.Property(p => p.NetAmount).HasColumnName("Liquido").HasColumnType("decimal(8,2)");
            entity.Property(p => p.GrossAmount).HasColumnName("Valor").HasColumnType("decimal(8,2)");

            entity.HasMany(a => a.Payments)
                .WithOne()
                .HasForeignKey( p => p.AdvanceId);
            
            entity.ForSqlServerHasIndex(q => q.CustomerId);
            entity.ForSqlServerHasIndex(q => q.RequestDate);
            entity.ForSqlServerHasIndex(q => q.EvaluationDateEnd);
        }
    }
}
