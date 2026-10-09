using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MiniBanco.Core.Dominio;
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

        Navegar(Tela.Menu);
    }

    private void ItemMenu_Click(object sender, RoutedEventArgs e)
    {
        Navegar((Tela)((RadioButton)sender).Tag);
    }

    private void Inicio_Click(object sender, RoutedEventArgs e)
    {
        Navegar(Tela.Menu);
    }

    private void Sair_Click(object sender, RoutedEventArgs e)
    {
        SairSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void Navegar(Tela tela)
    {
        IEnumerable<RadioButton> itensMenu = new[] { ItemConsultar, ItemDepositar, ItemSacar, ItemExtrato };
        foreach (RadioButton item in itensMenu)
            item.IsChecked = (Tela)item.Tag == tela;

        switch (tela)
        {
            case Tela.Saldo:
                Mostrar(new ConsultarSaldoView(_conta), "Consultar saldo");
                break;
            case Tela.Depositar:
                Mostrar(new DepositarView(_banco, _conta), "Depositar");
                break;
            case Tela.Sacar:
                Mostrar(new SacarView(_banco, _conta), "Sacar");
                break;
            case Tela.Extrato:
                Mostrar(new ExtratoView(_conta), "Mostrar extrato");
                break;
            default:
                var menu = new MenuPrincipalView();
                menu.OpcaoSelecionada += (_, destino) => Navegar(destino);
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
