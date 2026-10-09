using System;
using System.Collections.Generic;
using System.Linq;

public class Banco
{
    private readonly RepositorioContas _repositorio;
    private readonly Dictionary<int, Conta> _contas;

    public Banco(RepositorioContas repositorio)
    {
        _repositorio = repositorio;
        _contas = repositorio.Carregar().ToDictionary(c => c.Numero);
    }

    public void CriarConta(int numero, string titular, decimal saldoInicial)
    {
        if (_contas.ContainsKey(numero)) throw new InvalidOperationException("Número de conta já existe.");
        if (saldoInicial < 0) throw new ArgumentException("Saldo inicial não pode ser negativo.");
        _contas[numero] = new Conta(numero, titular, saldoInicial);
        Salvar();
    }

    public Conta BuscarConta(int numero)
    {
        if (!_contas.TryGetValue(numero, out Conta? conta)) throw new KeyNotFoundException("Conta não encontrada.");
        return conta;
    }

    public void Depositar(int numero, decimal valor)
    {
        BuscarConta(numero).Depositar(valor);
        Salvar();
    }

    public void Sacar(int numero, decimal valor)
    {
        BuscarConta(numero).Sacar(valor);
        Salvar();
    }

    public IEnumerable<Conta> ListarContas() => _contas.Values;

    private void Salvar() => _repositorio.Salvar(_contas.Values);
}
