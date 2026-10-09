using System;
using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Autenticação por número da conta e senha.
/// </summary>
public partial class LoginView : UserControl
{
    private readonly Banco _banco;

    public event EventHandler<Conta>? Entrou;
    public event EventHandler? CriarContaSolicitada;

    public LoginView(Banco banco, string? mensagem = null)
    {
        InitializeComponent();
        _banco = banco;

        if (mensagem is not null) Feedback.Mostrar(TxtMensagem, mensagem, true);
    }

    private void Entrar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(TxtNumero.Text, out int numero))
                throw new ArgumentException("Informe um número de conta válido.");

            Conta conta = _banco.Autenticar(numero, TxtSenha.Password);
            Entrou?.Invoke(this, conta);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }

    private void CriarConta_Click(object sender, RoutedEventArgs e)
    {
        CriarContaSolicitada?.Invoke(this, EventArgs.Empty);
    }
}
