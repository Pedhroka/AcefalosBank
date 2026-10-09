using System;
using System.Windows;
using System.Windows.Controls;
using MiniBanco.Ui.Views;

namespace MiniBanco.Ui;

/// <summary>
/// Área logada: menu lateral e área de conteúdo onde as telas são exibidas.
/// </summary>
public partial class MainWindow : Window
{
    private readonly Banco _banco;
    private readonly Conta _conta;

    public event EventHandler? SairSolicitado;

    public MainWindow(Banco banco, Conta conta)
    {
        InitializeComponent();
        _banco = banco;
        _conta = conta;

        TxtTitularSidebar.Text = conta.Titular;
        TxtContaSidebar.Text = $"Conta {conta.Numero}";

        Abrir("menu");
    }

    private void ItemMenu_Click(object sender, RoutedEventArgs e)
    {
        Abrir((string)((RadioButton)sender).Tag);
    }

    private void Inicio_Click(object sender, RoutedEventArgs e)
    {
        Abrir("menu");
    }

    private void Sair_Click(object sender, RoutedEventArgs e)
    {
        SairSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void Abrir(string chave)
    {
        foreach (RadioButton item in new[] { ItemConsultar, ItemDepositar, ItemSacar, ItemExtrato })
            item.IsChecked = (string)item.Tag == chave;

        switch (chave)
        {
            case "saldo":
                Mostrar(new ConsultarSaldoView(_conta), "Consultar saldo");
                break;
            case "depositar":
                Mostrar(new DepositarView(_banco, _conta), "Depositar");
                break;
            case "sacar":
                Mostrar(new SacarView(_banco, _conta), "Sacar");
                break;
            case "extrato":
                Mostrar(new ExtratoView(_conta), "Mostrar extrato");
                break;
            default:
                var menu = new MenuPrincipalView();
                menu.OpcaoSelecionada += (_, opcao) => Abrir(opcao);
                Mostrar(menu, "Menu principal");
                break;
        }
    }

    private void Mostrar(UIElement tela, string titulo)
    {
        Conteudo.Content = tela;
        TxtTituloAtual.Text = titulo;
    }
}
