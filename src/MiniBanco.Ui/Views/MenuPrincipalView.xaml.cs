using System.Windows;
using System.Windows.Controls;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Tela inicial com atalhos para as operações. Informa a opção escolhida pela Tag do botão.
/// </summary>
public partial class MenuPrincipalView : UserControl
{
    public event EventHandler<string>? OpcaoSelecionada;

    public MenuPrincipalView()
    {
        InitializeComponent();
    }

    private void Card_Click(object sender, RoutedEventArgs e)
    {
        OpcaoSelecionada?.Invoke(this, (string)((Button)sender).Tag);
    }
}
