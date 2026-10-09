using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Lista todas as contas cadastradas com número, titular e saldo.
/// </summary>
public partial class ListarContasView : UserControl
{
    private readonly Banco _banco;

    public ListarContasView(Banco banco)
    {
        InitializeComponent();
        _banco = banco;
        Carregar();
    }

    private void Atualizar_Click(object sender, RoutedEventArgs e)
    {
        Carregar();
    }

    private void Carregar()
    {
        var contas = _banco.ListarContas().ToList();

        Tabela.ItemsSource = contas;
        TxtQuantidade.Text = contas.Count == 1 ? "1 conta no Mini-Banco" : $"{contas.Count} contas no Mini-Banco";

        bool vazio = contas.Count == 0;
        Tabela.Visibility = vazio ? Visibility.Collapsed : Visibility.Visible;
        TxtVazio.Visibility = vazio ? Visibility.Visible : Visibility.Collapsed;
    }
}
