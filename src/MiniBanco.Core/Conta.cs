using System;
using System.Collections.Generic;

public class Conta
{
    public int Numero { get; }
    public string Titular { get; }
    public decimal Saldo { get; private set; }

    public Conta(int numero, string titular, decimal saldoInicial)
    {
        Numero = numero;
        Titular = titular;
        Saldo = saldoInicial;
    }

    public void Depositar(decimal valor)
    {
        if (valor <= 0) throw new ArgumentException("Valor de depósito deve ser positivo.");
        Saldo += valor;
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0) throw new ArgumentException("Valor de saque deve ser positivo.");
        if (valor > Saldo) throw new InvalidOperationException("Saldo insuficiente.");
        Saldo -= valor;
    }
}