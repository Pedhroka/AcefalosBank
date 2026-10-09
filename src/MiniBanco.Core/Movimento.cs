using System;

public sealed record Movimento(
    DateTime Data,
    string Descricao,
    int? NumeroContraparte,
    string? NomeContraparte,
    decimal Valor,
    decimal SaldoApos);
