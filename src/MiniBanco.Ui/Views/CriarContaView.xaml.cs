using System;
using System.Windows;
using System.Windows.Controls;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Formulário para abrir uma nova conta.
/// </summary>
public partial class CriarContaView : UserControl
{
    private readonly Banco _banco;

    public event EventHandler? Voltou;
    public event EventHandler<int>? ContaCriada;

    public CriarContaView(Banco banco)
    {
        InitializeComponent();
        _banco = banco;
    }

    private void Criar_Click(object sender, RoutedEventArgs e)
    {
        bool criada = Mensagens.Executar(TxtMensagem, () =>
        {
            int numero = Entrada.Numero(TxtNumero.Text);
            if (TxtSenha.Password != TxtConfirmarSenha.Password)
                throw new ErroDeNegocio("As senhas não conferem.");

            _banco.CriarConta(numero, TxtTitular.Text, TxtSenha.Password, Entrada.Valor(TxtSaldo.Text));
            return "Conta criada com sucesso.";
        });

        if (criada) ContaCriada?.Invoke(this, Entrada.Numero(TxtNumero.Text));
    }

    private void Voltar_Click(object sender, RoutedEventArgs e)
    {
        Voltou?.Invoke(this, EventArgs.Empty);
    }
}
