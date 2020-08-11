using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.EntityModel
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Usuario");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).HasColumnType("uniqueidentifier").ValueGeneratedOnAdd();
            entity.Property(p => p.Name).HasColumnName("Nome").HasColumnType("varchar(255)").IsRequired();
            entity.Property(p => p.Email).HasColumnName("Email").HasColumnType("varchar(255)").IsRequired();
            entity.Property(p => p.IsActive).HasColumnName("Ativa").HasColumnType("bit").HasDefaultValue(true);
        }
    }
}
