using GerenciadorAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Models
{
    public class Transacao : Entity
    {
        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public bool Recorrente { get; set; }

        public int? QuantidadeParcelas { get; set; }

        public StatusTransacao Status_Transacao { get; set; }

        public FormaPagamento FormaPagamento { get; set; }

        public CategoriaTransacao Categoria { get; set; }
    }
}
