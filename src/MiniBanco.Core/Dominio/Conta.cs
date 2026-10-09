using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBanco.Core.Dominio;

/// <summary>
/// Conta bancária: guarda o saldo e o histórico de movimentações, e só altera ambos por suas operações.
/// </summary>
public class Conta
{
    private readonly List<Movimento> _movimentos;

    private Conta(int numero, string titular, string senhaHash, decimal saldo, IEnumerable<Movimento> movimentos)
    {
        Numero = numero;
        Titular = titular;
        SenhaHash = senhaHash;
        Saldo = saldo;
        _movimentos = movimentos.ToList();
    }

    public int Numero { get; }
    public string Titular { get; }
    public decimal Saldo { get; private set; }
    public IReadOnlyList<Movimento> Movimentos => _movimentos;

    /// <summary>Hash da senha. Interno ao Core: a interface nunca acessa o hash.</summary>
    internal string SenhaHash { get; }

    /// <summary>Abre uma conta nova, com saldo inicial lançado como primeira movimentação.</summary>
    public static Conta Abrir(int numero, string titular, string senha, decimal saldoInicial)
    {
        if (string.IsNullOrWhiteSpace(titular)) throw new ErroDeNegocio("Informe o titular da conta.");
        if (string.IsNullOrWhiteSpace(senha)) throw new ErroDeNegocio("Informe uma senha.");
        if (saldoInicial < 0) throw new ErroDeNegocio("Saldo inicial não pode ser negativo.");

        var conta = new Conta(numero, titular.Trim(), Seguranca.GerarHash(senha), 0, Enumerable.Empty<Movimento>());
        if (saldoInicial > 0) conta.Creditar(saldoInicial, TipoMovimento.Abertura);
        return conta;
    }

    /// <summary>Reconstrói uma conta já existente, por exemplo ao carregar do armazenamento.</summary>
    public static Conta Restaurar(int numero, string titular, string senhaHash, decimal saldo, IEnumerable<Movimento> movimentos)
        => new(numero, titular, senhaHash, saldo, movimentos);

    public bool VerificarSenha(string senha) => Seguranca.Verificar(senha, SenhaHash);

    public void Creditar(decimal valor, TipoMovimento tipo, Conta? contraparte = null)
    {
        ValidarValor(valor);
        Saldo += valor;
        Registrar(tipo, contraparte, valor);
    }

    public void Debitar(decimal valor, TipoMovimento tipo, Conta? contraparte = null)
    {
        ValidarValor(valor);
        if (valor > Saldo) throw new ErroDeNegocio("Saldo insuficiente.");
        Saldo -= valor;
        Registrar(tipo, contraparte, -valor);
    }

    private static void ValidarValor(decimal valor)
    {
        if (valor <= 0) throw new ErroDeNegocio("O valor deve ser maior que zero.");
    }

    private void Registrar(TipoMovimento tipo, Conta? contraparte, decimal valor)
    {
        _movimentos.Add(new Movimento(DateTime.Now, tipo, contraparte?.Numero, contraparte?.Titular, valor, Saldo));
    }
}
