using System.Windows;
using System.Windows.Controls;
using MiniBanco.Ui.Views;

namespace MiniBanco.Ui;

/// <summary>
/// Janela principal: menu lateral e área de conteúdo onde as telas são exibidas.
/// </summary>
public partial class MainWindow : Window
{
    private readonly Banco _banco = new Banco();

    public MainWindow()
    {
        InitializeComponent();
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
        Close();
    }

    private void Abrir(string chave)
    {
        foreach (RadioButton item in new[] { ItemCriar, ItemConsultar, ItemDepositar, ItemSacar, ItemListar })
            item.IsChecked = (string)item.Tag == chave;

        switch (chave)
        {
            case "criar":
                Mostrar(new CriarContaView(_banco), "Criar conta");
                break;
            case "consultar":
                Mostrar(new ConsultarSaldoView(_banco), "Consultar saldo");
                break;
            case "depositar":
                Mostrar(new DepositarView(_banco), "Depositar");
                break;
            case "sacar":
                Mostrar(new SacarView(_banco), "Sacar");
                break;
            case "listar":
                Mostrar(new ListarContasView(_banco), "Listar contas");
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
