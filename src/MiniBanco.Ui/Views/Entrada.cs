using System.Globalization;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Converte o texto digitado pelo usuário. Entradas inválidas geram ErroDeNegocio com mensagem para exibir.
/// </summary>
internal static class Entrada
{
    public static int Numero(string texto)
    {
        if (!int.TryParse(texto, NumberStyles.Integer, CultureInfo.CurrentCulture, out int numero))
            throw new ErroDeNegocio("Informe um número de conta válido.");

        return numero;
    }

    public static decimal Valor(string texto)
    {
        if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
            throw new ErroDeNegocio("Informe um valor válido.");

        return valor;
    }
}
