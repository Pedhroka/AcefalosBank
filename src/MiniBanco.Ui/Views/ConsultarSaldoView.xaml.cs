using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Busca uma conta pelo número e exibe titular e saldo.
/// </summary>
public partial class ConsultarSaldoView : UserControl
{
    private readonly Banco _banco;

    public ConsultarSaldoView(Banco banco)
    {
        InitializeComponent();
        _banco = banco;
    }

    private void Buscar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(TxtNumero.Text, out int numero))
                throw new ArgumentException("Informe um número de conta válido.");

            Conta conta = _banco.BuscarConta(numero);

            TxtTitular.Text = conta.Titular;
            TxtBadgeConta.Text = $"Conta {conta.Numero}";
            TxtSaldo.Text = conta.Saldo.ToString("C");
            PainelResultado.Visibility = Visibility.Visible;
            Feedback.Ocultar(TxtMensagem);
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            PainelResultado.Visibility = Visibility.Collapsed;
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }
}
