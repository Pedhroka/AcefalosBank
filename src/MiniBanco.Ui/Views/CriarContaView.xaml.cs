using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Formulário para cadastrar uma nova conta.
/// </summary>
public partial class CriarContaView : UserControl
{
    private readonly Banco _banco;

    public CriarContaView(Banco banco)
    {
        InitializeComponent();
        _banco = banco;
    }

    private void Criar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(TxtNumero.Text, out int numero))
                throw new ArgumentException("Informe um número inteiro.");
            if (string.IsNullOrWhiteSpace(TxtTitular.Text))
                throw new ArgumentException("Informe o titular da conta.");
            if (!Feedback.TryLerValor(TxtSaldo.Text, out decimal saldo))
                throw new ArgumentException("Informe um saldo inicial válido.");

            _banco.CriarConta(numero, TxtTitular.Text.Trim(), saldo);
            Feedback.Mostrar(TxtMensagem, "Conta criada com sucesso.", true);
            Limpar();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }

    private void Cancelar_Click(object sender, RoutedEventArgs e)
    {
        Limpar();
        Feedback.Ocultar(TxtMensagem);
    }

    private void Limpar()
    {
        TxtNumero.Clear();
        TxtTitular.Clear();
        TxtSaldo.Clear();
    }
}
