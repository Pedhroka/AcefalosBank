using System.Windows;
using System.Windows.Controls;

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
        try
        {
            if (!int.TryParse(TxtDestino.Text, out int numeroDestino))
                throw new ArgumentException("Informe um número de conta válido.");
            if (!Feedback.TryLerValor(TxtValor.Text, out decimal valor))
                throw new ArgumentException("Informe um valor válido.");

            _banco.Transferir(_conta.Numero, numeroDestino, valor);
            string titularDestino = _banco.BuscarConta(numeroDestino).Titular;

            Feedback.Mostrar(TxtMensagem, $"Depósito de {valor:C} enviado para {titularDestino}. Novo saldo: {_conta.Saldo:C}", true);
            TxtDestino.Clear();
            TxtValor.Clear();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
        {
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }
}
