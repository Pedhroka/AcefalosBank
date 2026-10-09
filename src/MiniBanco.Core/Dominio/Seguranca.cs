using System;
using System.Security.Cryptography;

namespace MiniBanco.Core.Dominio;

/// <summary>
/// Gera e verifica hashes de senha com PBKDF2 (SHA-256 e sal aleatório).
/// </summary>
internal static class Seguranca
{
    private const int Iteracoes = 100_000;
    private const int TamanhoSal = 16;
    private const int TamanhoHash = 32;

    public static string GerarHash(string senha)
    {
        byte[] sal = RandomNumberGenerator.GetBytes(TamanhoSal);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(senha, sal, Iteracoes, HashAlgorithmName.SHA256, TamanhoHash);
        return $"{Iteracoes}.{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verificar(string senha, string? armazenado)
    {
        if (string.IsNullOrEmpty(armazenado)) return false;

        string[] partes = armazenado.Split('.');
        if (partes.Length != 3 || !int.TryParse(partes[0], out int iteracoes)) return false;

        byte[] sal = Convert.FromBase64String(partes[1]);
        byte[] esperado = Convert.FromBase64String(partes[2]);
        byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(senha, sal, iteracoes, HashAlgorithmName.SHA256, esperado.Length);

        return CryptographicOperations.FixedTimeEquals(calculado, esperado);
    }
}
