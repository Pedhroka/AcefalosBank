using MiniBanco.Core.Dominio;
using MiniBanco.Core.Persistencia;

namespace MiniBanco.Core.Testes;

/// <summary>
/// Repositório falso para testar o Banco sem tocar em disco.
/// </summary>
internal sealed class RepositorioEmMemoria : IRepositorioContas
{
    private readonly List<Conta> _contas;

    public RepositorioEmMemoria(IEnumerable<Conta>? contas = null)
    {
        _contas = contas?.ToList() ?? new List<Conta>();
    }

    public int Salvamentos { get; private set; }

    public IReadOnlyList<Conta> Carregar() => _contas.ToList();

    public void Salvar(IEnumerable<Conta> contas)
    {
        Salvamentos++;
        _contas.Clear();
        _contas.AddRange(contas);
    }
}
