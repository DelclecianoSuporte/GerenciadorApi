using Xunit;
using Moq;
using DevIO.Api.Controllers;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using DevIO.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using GerenciadorAPI.Enums;

namespace DevIO.Tests
{
    public class TransacaoControllerTests
    {
        private readonly TransacaoController _controller;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ITransacaoService> _mockTransacaoService;
        private readonly Mock<INotificador> _mockNotificador;

        public TransacaoControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockTransacaoService = new Mock<ITransacaoService>();
            _mockNotificador = new Mock<INotificador>();

            _controller = new TransacaoController(_mockNotificador.Object, _mockMapper.Object, _mockTransacaoService.Object);
        }

        [Fact]
        public async Task DeveRetornarListaDeTransacoes()
        {
            var transacoes = new List<Transacao>
            {
                new Transacao { },
                new Transacao { }
            };

            var transacaoViewModels = new List<TransacaoViewModel>
            {
                new TransacaoViewModel {  },
                new TransacaoViewModel {  }
            };

            _mockTransacaoService.Setup(service => service.ObterTodos()).ReturnsAsync(transacoes);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TransacaoViewModel>>(transacoes)).Returns(transacaoViewModels);

            var resultado = await _controller.MostrarTransacoes();

            var retornaActionResult = Assert.IsType<ActionResult<IEnumerable<TransacaoViewModel>>>(resultado);
            var okResult = Assert.IsType<OkObjectResult>(retornaActionResult.Result);

            var objetoConvertido = JObject.FromObject(okResult.Value);

            Assert.True((bool)objetoConvertido["success"]);
            var retornaValoresDoData = objetoConvertido["data"].ToObject<IEnumerable<TransacaoViewModel>>();
            Assert.NotEmpty(retornaValoresDoData); //Verifica se a lista de transacoes esta vazia
        }

        [Fact]
        public async Task DeveRetornarTransacaoPeloId()
        {
            var transacaoId = Guid.NewGuid();
            var transacao = new Transacao { Id = transacaoId };

            _mockTransacaoService.Setup(service => service.ObterPorId(transacaoId)).ReturnsAsync(transacao);
            var transacaoViewModel = new TransacaoViewModel { Id = transacaoId };
            _mockMapper.Setup(mapper => mapper.Map<TransacaoViewModel>(transacao)).Returns(transacaoViewModel);

            var resultado = await _controller.MostrarTransacaoPorId(transacaoId);

            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var objetoConvertido = JObject.FromObject(okResult.Value);

            Assert.True((bool)objetoConvertido["success"]);
            var modeloRetornado = objetoConvertido["data"].ToObject<TransacaoViewModel>();
            Assert.Equal(transacaoId, modeloRetornado.Id);
        }

        [Fact]
        public async Task DeveRetornarNotFoundQuandoTransacaoNaoExistir()
        {
            var transacaoId = Guid.NewGuid();

            _mockTransacaoService.Setup(service => service.ObterPorId(transacaoId)).ReturnsAsync((Transacao)null);

            var resultado = await _controller.MostrarTransacaoPorId(transacaoId);

            Assert.IsType<NotFoundObjectResult>(resultado.Result);
        }

        [Fact]
        public async Task DeveRetornarSucessoQuandoTransacoesRecorrentesForemAdicionadas()
        {
            var transacaoViewModel = new TransacaoViewModel
            {
                Tipo = "Receita",
                Valor = 100,
                Data = DateTime.Now,
                Descricao = "Pagamento de salário",
                Recorrente = true,
                QuantidadeParcelas = 3, // 3 parcelas
                Status_Transacao = StatusTransacao.Pago,
                FormaPagamento = FormaPagamento.Dinheiro,
                Categoria = CategoriaTransacao.Compras
            };

            _mockTransacaoService.Setup(service => service.AdicionarVarias(It.IsAny<List<Transacao>>()))
                .Returns(Task.CompletedTask);

            var resultado = await _controller.AdicionarTransacao(transacaoViewModel);

            var actionResult = Assert.IsType<CreatedAtActionResult>(resultado);
            var resultadoTransacao = Assert.IsType<List<Transacao>>(actionResult.Value);

            _mockTransacaoService.Verify(service => service.AdicionarVarias(It.IsAny<List<Transacao>>()), Times.Once);

            Assert.Equal(201, actionResult.StatusCode);
            Assert.Equal(3, resultadoTransacao.Count); // 3 transações baseado na quantidade de parcelas
        }

        [Fact]
        public async Task DeveRetornarSucessoQuandoTransacoesSemRecorrenciaForemAdicionadas()
        {
            var transacaoViewModel = new TransacaoViewModel
            {
                Tipo = "Credito",
                Valor = 100,
                Data = DateTime.Now,
                Descricao = "Pagamento de salário",
                Recorrente = false, // Não recorrente
                QuantidadeParcelas = null, // Não há parcelas
                Status_Transacao = StatusTransacao.Pago,
                FormaPagamento = FormaPagamento.Dinheiro,
                Categoria = CategoriaTransacao.Compras
            };

            // Configurar o mock para simular a adição de uma transação única
            _mockTransacaoService.Setup(service => service.Adicionar(It.IsAny<Transacao>()))
                .Returns(Task.CompletedTask);

            var resultado = await _controller.AdicionarTransacao(transacaoViewModel);

            var actionResult = Assert.IsType<CreatedAtActionResult>(resultado);
            var resultadoTransacao = Assert.IsType<Transacao>(actionResult.Value);  // Retorna o resultado de apenas uma transacao

            _mockTransacaoService.Verify(service => service.Adicionar(It.IsAny<Transacao>()), Times.Once);

            Assert.Equal(201, actionResult.StatusCode);

            Assert.NotNull(resultadoTransacao);
            Assert.Equal(transacaoViewModel.Valor, resultadoTransacao.Valor);
        }

        [Fact]
        public async Task DeveRetornarTransacaoAtualizadaComSucesso()
        {
            var id = Guid.NewGuid();

            var transacaoViewModel = new TransacaoViewModel { Id = id };

            var transacao = new Transacao { Id = id };

            _mockMapper.Setup(m => m.Map<Transacao>(It.IsAny<TransacaoViewModel>()))
                .Returns(transacao);

            _mockTransacaoService.Setup(s => s.Atualizar(It.IsAny<Transacao>()))
                .Returns(Task.CompletedTask);

            var resultado = await _controller.Atualizar(id, transacaoViewModel);

            var actionResult = Assert.IsType<ActionResult<TransacaoViewModel>>(resultado);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var objetoConvertido = JObject.FromObject(okResult.Value);

            Assert.True((bool)objetoConvertido["success"]);

            var transacaoRetornada = objetoConvertido["data"].ToObject<TransacaoViewModel>();

            Assert.Equal(transacaoViewModel.Id, transacaoRetornada.Id);
        }

        [Fact]
        public async Task DeveRetornarSucessoQuandoForRemovidaUmaTransacaoSemRecorrencia()
        {
            var id = Guid.NewGuid();
            var transacao = new Transacao { Id = id };

            var transacaoViewModel = new TransacaoViewModel { Id = id };

            _mockTransacaoService.Setup(s => s.ObterPorId(id))
                .ReturnsAsync(transacao);

            _mockTransacaoService.Setup(s => s.Remover(id))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map<TransacaoViewModel>(It.IsAny<Transacao>()))
                .Returns(transacaoViewModel);

            var resultado = await _controller.Excluir(id);

            var actionResult = Assert.IsType<ActionResult<TransacaoViewModel>>(resultado);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var objetoConvertido = JObject.FromObject(okResult.Value);

            Assert.True((bool)objetoConvertido["success"]);

            var transacaoRemovida = objetoConvertido["data"].ToObject<TransacaoViewModel>();

            Assert.Equal(transacaoViewModel.Id, transacaoRemovida.Id);
        }

        [Fact]
        public async Task DeveRetornarSucessoQuandoForRemovidoTransacoesRecorrentes()
        {
            var id = Guid.NewGuid();
            var transacao = new Transacao
            {
                Id = id,
                Recorrente = true
            };

            var transacoesRecorrentes = new List<Transacao>();

            for (int i = 0; i < 2; i++)
            {
                transacoesRecorrentes.Add(new Transacao { Id = Guid.NewGuid() });
            }

            _mockTransacaoService.Setup(s => s.ObterPorId(id))
                .ReturnsAsync(transacao);

            _mockTransacaoService.Setup(s => s.ObterTransacoesRecorrentes(id))
                .ReturnsAsync(transacoesRecorrentes);

            _mockTransacaoService.Setup(s => s.Remover(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var resultado = await _controller.ExcluirRecorrentes(id);

            Assert.IsType<NoContentResult>(resultado);

            _mockTransacaoService.Verify(s => s.Remover(It.IsAny<Guid>()), Times.Exactly(transacoesRecorrentes.Count + 1));
        }

    }
}
