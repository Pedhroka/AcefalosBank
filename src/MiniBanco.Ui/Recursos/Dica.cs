using System.Windows;

namespace MiniBanco.Ui.Recursos;

/// <summary>
/// Propriedade anexada que define o texto de exemplo exibido em um campo vazio.
/// </summary>
public static class Dica
{
    public static readonly DependencyProperty TextoProperty =
        DependencyProperty.RegisterAttached("Texto", typeof(string), typeof(Dica), new PropertyMetadata(string.Empty));

    public static string GetTexto(DependencyObject elemento) => (string)elemento.GetValue(TextoProperty);

    public static void SetTexto(DependencyObject elemento, string valor) => elemento.SetValue(TextoProperty, valor);
}
