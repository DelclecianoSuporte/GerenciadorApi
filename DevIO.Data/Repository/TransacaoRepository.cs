using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class TransacaoRepository : Repository<Transacao>, ITransacaoRepository
    {
        public TransacaoRepository(Contexto db) : base(db) { }

        public async Task<IEnumerable<Transacao>> ObterTransacoesRecorrentes(Guid transacaoOrigemId)
        {
            var transacaoOrigem = await Db.Transacao.FindAsync(transacaoOrigemId);
            if (transacaoOrigem == null)
            {
                return Enumerable.Empty<Transacao>();
            }

            var descricaoSemParcela = transacaoOrigem.Descricao.Split(" - ")[0];

            var transacoes = await Db.Transacao
                .Where(t => t.Descricao.StartsWith(descricaoSemParcela) && t.Id != transacaoOrigemId)
                .ToListAsync();

            return transacoes;
        }
    }
}
