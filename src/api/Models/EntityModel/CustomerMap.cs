using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.ToTable("Cliente");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).HasColumnType("uniqueidentifier").ValueGeneratedOnAdd();
            entity.Property(p => p.Name).HasColumnName("Nome").HasColumnType("varchar(255)").IsRequired();
            entity.Property(p => p.Email).HasColumnName("Email").HasColumnType("varchar(255)").IsRequired();
            entity.Property(p => p.IsActive).HasColumnName("IsActive").HasColumnType("bit").HasDefaultValue(true);

        }
    }
}
