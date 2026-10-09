using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class RepositorioContas
{
    private static readonly JsonSerializerOptions Opcoes = new JsonSerializerOptions { WriteIndented = true };

    private readonly string _caminho;

    public RepositorioContas(string caminho)
    {
        _caminho = caminho;
    }

    public IEnumerable<Conta> Carregar()
    {
        if (!File.Exists(_caminho)) return new List<Conta>();

        try
        {
            List<ContaRegistro>? registros = JsonSerializer.Deserialize<List<ContaRegistro>>(File.ReadAllText(_caminho));
            return (registros ?? new List<ContaRegistro>())
                .Select(r => new Conta(r.Numero, r.Titular, r.SenhaHash ?? string.Empty, r.Saldo, r.Movimentos))
                .ToList();
        }
        catch (JsonException)
        {
            throw new InvalidOperationException($"Arquivo de contas inválido: {_caminho}");
        }
    }

    public void Salvar(IEnumerable<Conta> contas)
    {
        var registros = contas
            .Select(c => new ContaRegistro(c.Numero, c.Titular, c.SenhaHash, c.Saldo, c.Movimentos.ToList()))
            .ToList();
        string json = JsonSerializer.Serialize(registros, Opcoes);

        string? pasta = Path.GetDirectoryName(_caminho);
        if (!string.IsNullOrEmpty(pasta)) Directory.CreateDirectory(pasta);

        string temporario = _caminho + ".tmp";
        File.WriteAllText(temporario, json);
        File.Move(temporario, _caminho, overwrite: true);
    }
}
