using System.Collections.Generic;
using System.Linq;
using MiniBanco.Core.Persistencia;

namespace MiniBanco.Core.Dominio;

/// <summary>
/// Ponto de entrada das operações bancárias. Cada operação que altera contas é salva ao final.
/// </summary>
public class Banco
{
    private readonly IRepositorioContas _repositorio;
    private readonly Dictionary<int, Conta> _contas;

    public Banco(IRepositorioContas repositorio)
    {
        _repositorio = repositorio;
        _contas = repositorio.Carregar().ToDictionary(c => c.Numero);
    }

    public Conta CriarConta(int numero, string titular, string senha, decimal saldoInicial)
    {
        if (_contas.ContainsKey(numero)) throw new ErroDeNegocio("Número de conta já existe.");

        Conta conta = Conta.Abrir(numero, titular, senha, saldoInicial);
        _contas[numero] = conta;
        Salvar();
        return conta;
    }

    public Conta Autenticar(int numero, string senha)
    {
        if (!_contas.TryGetValue(numero, out Conta? conta) || !conta.VerificarSenha(senha))
            throw new ErroDeNegocio("Número da conta ou senha inválidos.");

        return conta;
    }

    public Conta BuscarConta(int numero)
    {
        if (!_contas.TryGetValue(numero, out Conta? conta)) throw new ErroDeNegocio("Conta não encontrada.");
        return conta;
    }

    public void Sacar(int numero, decimal valor)
    {
        BuscarConta(numero).Debitar(valor, TipoMovimento.Saque);
        Salvar();
    }

    /// <summary>Transfere da conta de origem para a de destino e devolve a conta de destino.</summary>
    public Conta Transferir(int numeroOrigem, int numeroDestino, decimal valor)
    {
        if (numeroOrigem == numeroDestino) throw new ErroDeNegocio("Não é possível depositar na própria conta.");

        Conta origem = BuscarConta(numeroOrigem);
        Conta destino = BuscarConta(numeroDestino);

        origem.Debitar(valor, TipoMovimento.DepositoEnviado, destino);
        destino.Creditar(valor, TipoMovimento.DepositoRecebido, origem);
        Salvar();
        return destino;
    }

    private void Salvar() => _repositorio.Salvar(_contas.Values);
}
