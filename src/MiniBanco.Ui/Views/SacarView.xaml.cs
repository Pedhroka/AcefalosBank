using System.Windows;
using System.Windows.Controls;
using MiniBanco.Core.Dominio;

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
        Mensagens.Executar(TxtMensagem, () =>
        {
            _banco.Sacar(_conta.Numero, Entrada.Valor(TxtValor.Text));

            TxtValor.Clear();
            return $"Saque realizado. Novo saldo: {_conta.Saldo:C}";
        });
    }
}
