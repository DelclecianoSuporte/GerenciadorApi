using AutoMapper;
using DevIO.Api.ViewModel;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/transacao")]
    public class TransacaoController : MainController
    {
        private readonly IMapper _mapper;
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(INotificador notificador, IMapper mapper, ITransacaoService transacaoService) : base(notificador)
        {
            _mapper = mapper;
            _transacaoService = transacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoViewModel>>> MostrarTransacoes()
        {
            var transacoes = (_mapper.Map<IEnumerable<TransacaoViewModel>>(await _transacaoService.ObterTodos()));
            return CustomResponse(transacoes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransacaoViewModel>> MostrarTransacaoPorId(Guid id)
        {
            var transacao = await _transacaoService.ObterPorId(id);
            if (transacao == null)
            {
                return NotFound(new { message = $"O ID {id} passado não foi encontrado, ele pode ter sido apagado." }); ;
            }

            return CustomResponse(transacao);
        }

        //[HttpPost]
        //public async Task<IActionResult> AdicionarTransacao([FromBody] TransacaoViewModel transacaoViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var transacoes = new List<Transacao>();

        //    if (transacaoViewModel.Recorrente && transacaoViewModel.QuantidadeParcelas.HasValue)
        //    {
        //        for (int i = 0; i < transacaoViewModel.QuantidadeParcelas.Value; i++)
        //        {
        //            transacoes.Add(new Transacao
        //            {
        //                Id = Guid.NewGuid(),
        //                Tipo = transacaoViewModel.Tipo,
        //                Valor = transacaoViewModel.Valor,
        //                Data = transacaoViewModel.Data.AddMonths(i),
        //                Descricao = $"{transacaoViewModel.Descricao} - Parcela {i + 1}",
        //                Recorrente = transacaoViewModel.Recorrente,
        //                QuantidadeParcelas = transacaoViewModel.QuantidadeParcelas,
        //                Status_Transacao = transacaoViewModel.Status_Transacao,
        //                FormaPagamento = transacaoViewModel.FormaPagamento,
        //                Categoria = transacaoViewModel.Categoria
        //            });
        //        }

        //        await _transacaoService.AdicionarVarias(transacoes);
        //    }
        //    else
        //    {
        //        var transacao = new Transacao
        //        {
        //            Id = Guid.NewGuid(),
        //            Tipo = transacaoViewModel.Tipo,
        //            Valor = transacaoViewModel.Valor,
        //            Data = transacaoViewModel.Data,
        //            Descricao = transacaoViewModel.Descricao,
        //            Recorrente = transacaoViewModel.Recorrente,
        //            QuantidadeParcelas = transacaoViewModel.QuantidadeParcelas,
        //            Status_Transacao = transacaoViewModel.Status_Transacao,
        //            FormaPagamento = transacaoViewModel.FormaPagamento,
        //            Categoria = transacaoViewModel.Categoria
        //        };

        //        await _transacaoService.Adicionar(transacao);
        //    }

        //    return CreatedAtAction(nameof(MostrarTransacaoPorId), new { id = transacoes.First().Id }, transacoes);
        //}

        [HttpPost]
        public async Task<IActionResult> AdicionarTransacao([FromBody] TransacaoViewModel transacaoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transacoes = new List<Transacao>();

            if (transacaoViewModel.Recorrente && transacaoViewModel.QuantidadeParcelas.HasValue)
            {
                for (int i = 0; i < transacaoViewModel.QuantidadeParcelas.Value; i++)
                {
                    transacoes.Add(new Transacao
                    {
                        Id = Guid.NewGuid(),
                        Tipo = transacaoViewModel.Tipo,
                        Valor = transacaoViewModel.Valor,
                        Data = transacaoViewModel.Data.AddMonths(i),
                        Descricao = $"{transacaoViewModel.Descricao} - Parcela {i + 1}",
                        Recorrente = transacaoViewModel.Recorrente,
                        QuantidadeParcelas = transacaoViewModel.QuantidadeParcelas,
                        Status_Transacao = transacaoViewModel.Status_Transacao,
                        FormaPagamento = transacaoViewModel.FormaPagamento,
                        Categoria = transacaoViewModel.Categoria
                    });
                }

                await _transacaoService.AdicionarVarias(transacoes);
            }
            else
            {
                var transacao = new Transacao
                {
                    Id = Guid.NewGuid(),
                    Tipo = transacaoViewModel.Tipo,
                    Valor = transacaoViewModel.Valor,
                    Data = transacaoViewModel.Data,
                    Descricao = transacaoViewModel.Descricao,
                    Recorrente = transacaoViewModel.Recorrente,
                    QuantidadeParcelas = transacaoViewModel.QuantidadeParcelas,
                    Status_Transacao = transacaoViewModel.Status_Transacao,
                    FormaPagamento = transacaoViewModel.FormaPagamento,
                    Categoria = transacaoViewModel.Categoria
                };

                await _transacaoService.Adicionar(transacao);
                return CreatedAtAction(nameof(MostrarTransacaoPorId), new { id = transacao.Id }, transacao);
            }

            return CreatedAtAction(nameof(MostrarTransacaoPorId), new { id = transacoes.First().Id }, transacoes);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TransacaoViewModel>> Atualizar(Guid id, [FromBody] TransacaoViewModel transacaoViewModel)
        {
            if (id != transacaoViewModel.Id)
            {
                return NotFound("O id informado não é o mesmo que foi passado na query");
            }

            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _transacaoService.Atualizar(_mapper.Map<Transacao>(transacaoViewModel));

            return CustomResponse(transacaoViewModel);
        }



        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TransacaoViewModel>> Excluir(Guid id)
        {
            var funcionarioViewModel = await _transacaoService.ObterPorId(id);

            if (funcionarioViewModel == null)
            {
                return NotFound();
            }

            await _transacaoService.Remover(id);

            return CustomResponse(funcionarioViewModel);
        }

        [HttpDelete("recorrentes/{id:guid}")]
        public async Task<ActionResult> ExcluirRecorrentes(Guid id)
        {
            var transacaoViewModel = await _transacaoService.ObterPorId(id);

            if (transacaoViewModel == null)
            {
                return NotFound();
            }

            if (transacaoViewModel.Recorrente)
            {
                var transacoesRecorrentes = await _transacaoService.ObterTransacoesRecorrentes(id);
                foreach (var transacao in transacoesRecorrentes)
                {
                    await _transacaoService.Remover(transacao.Id);
                }
                await _transacaoService.Remover(id);

                return NoContent();
            }
            else
            {
                await _transacaoService.Remover(id);
                return NoContent(); 
            }
        }


    }
}
