//using GerenciadorAPI.Enums;
//using System.ComponentModel.DataAnnotations;

//namespace DevIO.Api.ViewModel
//{
//    public class TransacaoViewModel
//    {
//        [Key]
//        public Guid Id { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
//        public string Descricao { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
//        public string Tipo { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do campo {0} deve ser maior que {1}")]
//        public decimal Valor { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        public DateTime Data { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        public bool Recorrente { get; set; }

//        public int? QuantidadeParcelas { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        public StatusTransacao Status_Transacao { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        public FormaPagamento FormaPagamento { get; set; }

//        [Required(ErrorMessage = "O campo {0} é obrigatório")]
//        public CategoriaTransacao Categoria { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;
using GerenciadorAPI.Enums;
using System.Text.Json.Serialization; // Adicionando para JsonPropertyName

namespace DevIO.Api.ViewModel
{
    public class TransacaoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [JsonPropertyName("descricao")]  // Adicionando para garantir a correspondência de nome no JSON
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [JsonPropertyName("tipo")]  // Adicionando para garantir a correspondência de nome no JSON
        public string Tipo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do campo {0} deve ser maior que {1}")]
        [JsonPropertyName("valor")]  // Adicionando para garantir a correspondência de nome no JSON
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [JsonPropertyName("data")]  // Adicionando para garantir a correspondência de nome no JSON
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [JsonPropertyName("recorrente")]  // Adicionando para garantir a correspondência de nome no JSON
        public bool Recorrente { get; set; }

        [JsonPropertyName("quantidadeParcelas")]  // Adicionando para garantir a correspondência de nome no JSON
        public int? QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [JsonPropertyName("status_Transacao")]  // Adicionando para garantir a correspondência de nome no JSON
        public StatusTransacao Status_Transacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [JsonPropertyName("formaPagamento")]  // Adicionando para garantir a correspondência de nome no JSON
        public FormaPagamento FormaPagamento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [JsonPropertyName("categoria")]  // Adicionando para garantir a correspondência de nome no JSON
        public CategoriaTransacao Categoria { get; set; }
    }
}
