using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Core.Persistencia;

/// <summary>
/// Grava e lê as contas em um arquivo JSON. A gravação é feita em arquivo temporário e depois substituída.
/// </summary>
public class RepositorioContas : IRepositorioContas
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _caminho;

    public RepositorioContas(string caminho)
    {
        _caminho = caminho;
    }

    public IReadOnlyList<Conta> Carregar()
    {
        if (!File.Exists(_caminho)) return Array.Empty<Conta>();

        try
        {
            List<ContaRegistro>? registros = JsonSerializer.Deserialize<List<ContaRegistro>>(File.ReadAllText(_caminho), Opcoes);
            return (registros ?? new List<ContaRegistro>()).Select(ParaConta).ToList();
        }
        catch (JsonException)
        {
            throw new ErroDePersistencia($"Arquivo de contas inválido: {_caminho}");
        }
    }

    public void Salvar(IEnumerable<Conta> contas)
    {
        string json = JsonSerializer.Serialize(contas.Select(ParaRegistro).ToList(), Opcoes);

        string? pasta = Path.GetDirectoryName(_caminho);
        if (!string.IsNullOrEmpty(pasta)) Directory.CreateDirectory(pasta);

        string temporario = _caminho + ".tmp";
        File.WriteAllText(temporario, json);
        File.Move(temporario, _caminho, overwrite: true);
    }

    private static Conta ParaConta(ContaRegistro registro) => Conta.Restaurar(
        registro.Numero,
        registro.Titular,
        registro.SenhaHash ?? string.Empty,
        registro.Saldo,
        registro.Movimentos ?? new List<Movimento>());

    private static ContaRegistro ParaRegistro(Conta conta) => new(
        conta.Numero,
        conta.Titular,
        conta.SenhaHash,
        conta.Saldo,
        conta.Movimentos.ToList());
}
