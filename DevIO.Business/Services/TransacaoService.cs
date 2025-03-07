using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        public async Task Adicionar(Transacao transacao)
        {
            await _transacaoRepository.Adicionar(transacao);
        }

        public async Task AdicionarVarias(IEnumerable<Transacao> transacoes)
        {
            foreach (var transacao in transacoes)
            {
                await _transacaoRepository.Adicionar(transacao);
            }
        }

        public async Task Atualizar(Transacao transacao)
        {
            await _transacaoRepository.Atualizar(transacao);
        }

        public void Dispose()
        {
          _transacaoRepository?.Dispose();
        }

        public async Task<Transacao> ObterPorId(Guid id)
        {
           return await _transacaoRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Transacao>> ObterTodos()
        {
          return await _transacaoRepository.ObterTodos();
        }

        public async Task<IEnumerable<Transacao>> ObterTransacoesRecorrentes(Guid transacaoOrigemId)
        {
            return await _transacaoRepository.ObterTransacoesRecorrentes(transacaoOrigemId);
        }

        public async Task Remover(Guid id)
        {
            await _transacaoRepository.Remover(id);
        }
    }
}
