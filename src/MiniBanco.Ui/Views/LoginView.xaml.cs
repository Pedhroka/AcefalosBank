using System;
using System.Windows;
using System.Windows.Controls;
using MiniBanco.Core.Dominio;

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

        if (mensagem is not null) Mensagens.Mostrar(TxtMensagem, mensagem, sucesso: true);
    }

    private void Entrar_Click(object sender, RoutedEventArgs e)
    {
        Conta? conta = Mensagens.Tentar(TxtMensagem, () => _banco.Autenticar(Entrada.Numero(TxtNumero.Text), TxtSenha.Password));
        if (conta is not null) Entrou?.Invoke(this, conta);
    }

    private void CriarConta_Click(object sender, RoutedEventArgs e)
    {
        CriarContaSolicitada?.Invoke(this, EventArgs.Empty);
    }
}
