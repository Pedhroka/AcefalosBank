using System;
using System.Windows;
using MiniBanco.Ui.Views;

namespace MiniBanco.Ui;

/// <summary>
/// Janela de entrada: alterna entre o login e a criação de conta.
/// </summary>
public partial class LoginWindow : Window
{
    private readonly Banco _banco;

    public event EventHandler<Conta>? Entrou;

    public LoginWindow(Banco banco)
    {
        InitializeComponent();
        _banco = banco;
        MostrarLogin();
    }

    private void MostrarLogin(string? mensagem = null)
    {
        var login = new LoginView(_banco, mensagem);
        login.Entrou += (_, conta) => Entrou?.Invoke(this, conta);
        login.CriarContaSolicitada += (_, _) => MostrarCriarConta();
        Conteudo.Content = login;
    }

    private void MostrarCriarConta()
    {
        var criar = new CriarContaView(_banco);
        criar.Voltou += (_, _) => MostrarLogin();
        criar.ContaCriada += (_, numero) => MostrarLogin($"Conta {numero} criada. Faça login para continuar.");
        Conteudo.Content = criar;
    }
}
