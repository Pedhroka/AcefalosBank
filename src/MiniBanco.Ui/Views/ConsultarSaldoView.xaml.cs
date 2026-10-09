using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Exibe o saldo da conta logada, oculto por padrão e revelado pelo ícone de olho.
/// </summary>
public partial class ConsultarSaldoView : UserControl
{
    private const string IconeMostrar = "";
    private const string IconeOcultar = "";
    private const string SaldoOculto = "R$ ••••••";

    private readonly Conta _conta;
    private bool _visivel;

    public ConsultarSaldoView(Conta conta)
    {
        InitializeComponent();
        _conta = conta;

        TxtTitular.Text = conta.Titular;
        TxtConta.Text = $"Conta {conta.Numero}";
        AtualizarSaldo();
    }

    private void Olho_Click(object sender, RoutedEventArgs e)
    {
        _visivel = !_visivel;
        AtualizarSaldo();
    }

    private void AtualizarSaldo()
    {
        TxtSaldo.Text = _visivel ? _conta.Saldo.ToString("C") : SaldoOculto;
        TxtIconeOlho.Text = _visivel ? IconeOcultar : IconeMostrar;
        BtnOlho.ToolTip = _visivel ? "Ocultar saldo" : "Mostrar saldo";
    }
}
