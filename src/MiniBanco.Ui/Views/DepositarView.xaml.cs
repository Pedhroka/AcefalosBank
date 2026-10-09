using System.Windows;
using System.Windows.Controls;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Transfere um valor da conta logada para outra conta cadastrada.
/// </summary>
public partial class DepositarView : UserControl
{
    private readonly Banco _banco;
    private readonly Conta _conta;

    public DepositarView(Banco banco, Conta conta)
    {
        InitializeComponent();
        _banco = banco;
        _conta = conta;

        TxtOrigem.Text = $"Sua conta: Conta {conta.Numero} · {conta.Titular}";
    }

    private void Depositar_Click(object sender, RoutedEventArgs e)
    {
        Mensagens.Executar(TxtMensagem, () =>
        {
            Conta destino = _banco.Transferir(_conta.Numero, Entrada.Numero(TxtDestino.Text), Entrada.Valor(TxtValor.Text));

            TxtDestino.Clear();
            TxtValor.Clear();
            return $"Depósito enviado para {destino.Titular}. Novo saldo: {_conta.Saldo:C}";
        });
    }
}
