namespace MiniBanco.Ui.Views;

/// <summary>
/// Linha do extrato já formatada para exibição.
/// </summary>
public sealed record LinhaExtrato(string Data, string Descricao, string Contraparte, string Valor, string SaldoApos, bool Positivo)
{
    public static LinhaExtrato De(Movimento movimento) => new(
        movimento.Data.ToString("dd/MM/yyyy HH:mm"),
        movimento.Descricao,
        movimento.NomeContraparte is null ? "—" : $"{movimento.NomeContraparte} ({movimento.NumeroContraparte})",
        (movimento.Valor > 0 ? "+" : string.Empty) + movimento.Valor.ToString("C"),
        movimento.SaldoApos.ToString("C"),
        movimento.Valor > 0);
}
