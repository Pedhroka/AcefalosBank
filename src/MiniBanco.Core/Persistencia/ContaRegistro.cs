using System.Collections.Generic;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Core.Persistencia;

/// <summary>
/// Formato em que a conta é gravada no arquivo JSON.
/// </summary>
public sealed record ContaRegistro(
    int Numero,
    string Titular,
    string SenhaHash,
    decimal Saldo,
    List<Movimento>? Movimentos);
