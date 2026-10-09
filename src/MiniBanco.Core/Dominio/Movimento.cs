using System;

namespace MiniBanco.Core.Dominio;

public sealed record Movimento(
    DateTime Data,
    TipoMovimento Tipo,
    int? NumeroContraparte,
    string? NomeContraparte,
    decimal Valor,
    decimal SaldoApos);
