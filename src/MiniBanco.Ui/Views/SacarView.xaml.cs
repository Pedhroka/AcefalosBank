using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Retira um valor do saldo de uma conta existente.
/// </summary>
public partial class SacarView : UserControl
{
    private readonly Banco _banco;

    public SacarView(Banco banco)
    {
        InitializeComponent();
        _banco = banco;
    }

    private void Sacar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(TxtNumero.Text, out int numero))
                throw new ArgumentException("Informe um número de conta válido.");
            if (!Feedback.TryLerValor(TxtValor.Text, out decimal valor))
                throw new ArgumentException("Informe um valor válido.");

            Conta conta = _banco.BuscarConta(numero);
            conta.Sacar(valor);

            Feedback.Mostrar(TxtMensagem, $"Saque realizado. Novo saldo: {conta.Saldo:C}", true);
            TxtValor.Clear();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
        {
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }
}
