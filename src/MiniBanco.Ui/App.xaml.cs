using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using MiniBanco.Core.Dominio;
using MiniBanco.Core.Persistencia;

namespace MiniBanco.Ui;

/// <summary>
/// Ponto de entrada da interface: configura a cultura, carrega o banco e controla a troca entre login e área logada.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ConfigurarCultura();

        Banco banco;
        try
        {
            banco = new Banco(new RepositorioContas(CaminhoDados()));
        }
        catch (ErroDePersistencia erro)
        {
            MessageBox.Show(erro.Message, "Acefalos Bank", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }

        AbrirLogin(banco);
    }

    private void AbrirLogin(Banco banco)
    {
        var login = new LoginWindow(banco);
        login.Entrou += (_, conta) =>
        {
            AbrirPrincipal(banco, conta);
            login.Close();
        };

        MainWindow = login;
        login.Show();
    }

    private void AbrirPrincipal(Banco banco, Conta conta)
    {
        var principal = new MainWindow(banco, conta);
        principal.SairSolicitado += (_, _) =>
        {
            AbrirLogin(banco);
            principal.Close();
        };

        MainWindow = principal;
        principal.Show();
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
