using System;
using System.Windows;
using System.Windows.Controls;

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
        try
        {
            if (!int.TryParse(TxtNumero.Text, out int numero))
                throw new ArgumentException("Informe um número inteiro.");
            if (TxtSenha.Password != TxtConfirmarSenha.Password)
                throw new ArgumentException("As senhas não conferem.");
            if (!Feedback.TryLerValor(TxtSaldo.Text, out decimal saldo))
                throw new ArgumentException("Informe um saldo inicial válido.");

            _banco.CriarConta(numero, TxtTitular.Text, TxtSenha.Password, saldo);
            ContaCriada?.Invoke(this, numero);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            Feedback.Mostrar(TxtMensagem, ex.Message, false);
        }
    }

    private void Voltar_Click(object sender, RoutedEventArgs e)
    {
        Voltou?.Invoke(this, EventArgs.Empty);
    }
}
