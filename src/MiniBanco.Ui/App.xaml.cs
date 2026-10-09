using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace MiniBanco.Ui;

/// <summary>
/// Inicializa o banco a partir do arquivo JSON e abre a janela principal.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ConfigurarCultura();

        try
        {
            var banco = new Banco(new RepositorioContas(CaminhoDados()));
            new MainWindow(banco).Show();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Acefalos Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    private static void ConfigurarCultura()
    {
        var cultura = new CultureInfo("pt-BR");

        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;
        Thread.CurrentThread.CurrentCulture = cultura;
        Thread.CurrentThread.CurrentUICulture = cultura;

        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(cultura.IetfLanguageTag)));
    }

    private static string CaminhoDados()
    {
        string pasta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(pasta, "MiniBanco", "contas.json");
    }
}
