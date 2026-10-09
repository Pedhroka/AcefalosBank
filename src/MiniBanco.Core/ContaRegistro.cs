using System.Collections.Generic;

public sealed record ContaRegistro(
    int Numero,
    string Titular,
    string SenhaHash,
    decimal Saldo,
    List<Movimento>? Movimentos);
