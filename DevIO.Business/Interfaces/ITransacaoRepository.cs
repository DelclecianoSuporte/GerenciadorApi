﻿using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface ITransacaoRepository : IRepository<Transacao>
    {
        Task<IEnumerable<Transacao>> ObterTransacoesRecorrentes(Guid transacaoOrigemId);
    }
}
