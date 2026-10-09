using System;
using System.IO;
using System.Windows;

namespace MiniBanco.Ui;

/// <summary>
/// Inicializa o banco a partir do arquivo JSON e abre a janela principal.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            var banco = new Banco(new RepositorioContas(CaminhoDados()));
            new MainWindow(banco).Show();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Mini-Banco", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    private static string CaminhoDados()
    {
        string pasta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(pasta, "MiniBanco", "contas.json");
    }
}
