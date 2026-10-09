using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Auxiliares compartilhados pelas telas: mensagens de retorno e leitura de valores.
/// </summary>
internal static class Feedback
{
    public static void Mostrar(TextBlock destino, string texto, bool sucesso)
    {
        destino.Text = texto;
        destino.Style = (Style)Application.Current.FindResource(sucesso ? "TextoMensagemSucesso" : "TextoMensagemErro");
        destino.Visibility = Visibility.Visible;
    }

    public static void Ocultar(TextBlock destino)
    {
        destino.Visibility = Visibility.Collapsed;
    }

    public static bool TryLerValor(string texto, out decimal valor)
    {
        return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out valor);
    }
}
