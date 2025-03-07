using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface ITransacaoService : IDisposable
    {
        Task<IEnumerable<Transacao>> ObterTodos();
        Task<Transacao> ObterPorId(Guid id);
        Task Adicionar(Transacao transacao);
        Task AdicionarVarias(IEnumerable<Transacao> transacoes);
        Task Atualizar(Transacao transacao);
        Task Remover(Guid id);
        Task<IEnumerable<Transacao>> ObterTransacoesRecorrentes(Guid transacaoOrigemId);  // Novo método
    }
}
