using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Retira um valor do saldo da conta logada.
/// </summary>
public partial class SacarView : UserControl
{
    private readonly Banco _banco;
    private readonly Conta _conta;

    public SacarView(Banco banco, Conta conta)
    {
        InitializeComponent();
        _banco = banco;
        _conta = conta;

        TxtOrigem.Text = $"Sua conta: Conta {conta.Numero} · {conta.Titular}";
    }

    private void Sacar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!Feedback.TryLerValor(TxtValor.Text, out decimal valor))
                throw new ArgumentException("Informe um valor válido.");

            _banco.Sacar(_conta.Numero, valor);

            Feedback.Mostrar(TxtMensagem, $"Saque de {valor:C} realizado. Novo saldo: {_conta.Saldo:C}", true);
            TxtValor.Clear();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }
}
