using DevIO.Business.Models;
using GerenciadorAPI.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Mappings
{
    public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Descricao)
                .IsRequired()
                 .HasColumnType("varchar(200)");

            builder.Property(t => t.Tipo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(t => t.Valor)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Data)
               .IsRequired()
               .HasColumnType("datetime");

            builder.Property(t => t.Recorrente)
              .IsRequired();

            builder.Property(t => t.QuantidadeParcelas);

            builder.Property(t => t.Status_Transacao)
            .IsRequired()
            .HasConversion(v => v.ToString(), v => (StatusTransacao)Enum.Parse(typeof(StatusTransacao), v))
            .HasColumnType("varchar(100)");

            builder.Property(t => t.FormaPagamento)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (FormaPagamento)Enum.Parse(typeof(FormaPagamento), v))
                .HasColumnType("varchar(100)");

            builder.Property(t => t.Categoria)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (CategoriaTransacao)Enum.Parse(typeof(CategoriaTransacao), v))
                .HasColumnType("varchar(100)");

            builder.ToTable("Transacoes");
        }
    }
}
