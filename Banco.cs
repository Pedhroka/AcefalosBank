using System;
using System.Collections.Generic;

class Banco
{
    private readonly Dictionary<int, Conta> _contas = new Dictionary<int, Conta>();

    public void CriarConta(int numero, string titular, decimal saldoInicial)
    {
        if (_contas.ContainsKey(numero)) throw new InvalidOperationException("Número de conta já existe.");
        if (saldoInicial < 0) throw new ArgumentException("Saldo inicial não pode ser negativo.");
        _contas[numero] = new Conta(numero, titular, saldoInicial);
    }

    public Conta BuscarConta(int numero)
    {
        if (!_contas.TryGetValue(numero, out Conta conta)) throw new KeyNotFoundException("Conta não encontrada.");
        return conta;
    }

    public IEnumerable<Conta> ListarContas() => _contas.Values;
}