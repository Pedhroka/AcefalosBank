using System.Collections.Generic;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Core.Persistencia;

public interface IRepositorioContas
{
    IReadOnlyList<Conta> Carregar();

    void Salvar(IEnumerable<Conta> contas);
}
