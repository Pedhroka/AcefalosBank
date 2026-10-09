using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Extrato da conta logada, com as movimentações mais recentes primeiro.
/// </summary>
public partial class ExtratoView : UserControl
{
    public ExtratoView(Conta conta)
    {
        InitializeComponent();

        TxtSaldoAtual.Text = conta.Saldo.ToString("C");

        var linhas = conta.Movimentos.Reverse().Select(LinhaExtrato.De).ToList();
        Tabela.ItemsSource = linhas;

        bool vazio = linhas.Count == 0;
        Tabela.Visibility = vazio ? Visibility.Collapsed : Visibility.Visible;
        TxtVazio.Visibility = vazio ? Visibility.Visible : Visibility.Collapsed;
    }
}
