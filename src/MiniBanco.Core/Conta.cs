using System;
using System.Collections.Generic;
using System.Linq;

public class Conta
{
    private readonly List<Movimento> _movimentos;

    public Conta(int numero, string titular, string senhaHash, decimal saldo = 0, IEnumerable<Movimento>? movimentos = null)
    {
        Numero = numero;
        Titular = titular;
        SenhaHash = senhaHash;
        Saldo = saldo;
        _movimentos = movimentos?.ToList() ?? new List<Movimento>();
    }

    public int Numero { get; }
    public string Titular { get; }
    public string SenhaHash { get; }
    public decimal Saldo { get; private set; }
    public IReadOnlyList<Movimento> Movimentos => _movimentos;

    public void Creditar(decimal valor, string descricao, Conta? contraparte = null)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser positivo.");
        Saldo += valor;
        Registrar(descricao, contraparte, valor);
    }

    public void Debitar(decimal valor, string descricao, Conta? contraparte = null)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser positivo.");
        if (valor > Saldo) throw new InvalidOperationException("Saldo insuficiente.");
        Saldo -= valor;
        Registrar(descricao, contraparte, -valor);
    }

    private void Registrar(string descricao, Conta? contraparte, decimal valor)
    {
        _movimentos.Add(new Movimento(
            DateTime.Now,
            descricao,
            contraparte?.Numero,
            contraparte?.Titular,
            valor,
            Saldo));
    }
}
