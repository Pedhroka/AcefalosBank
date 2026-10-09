using System;
using System.Windows;
using System.Windows.Controls;
using MiniBanco.Core.Dominio;

namespace MiniBanco.Ui.Views;

/// <summary>
/// Executa uma ação de tela e exibe o resultado no bloco de mensagens.
/// Só trata ErroDeNegocio, que contém mensagens pensadas para o usuário.
/// </summary>
internal static class Mensagens
{
    /// <summary>Executa a ação, mostra a mensagem de sucesso e devolve true; em caso de erro, mostra o erro e devolve false.</summary>
    public static bool Executar(TextBlock destino, Func<string> acao)
    {
        try
        {
            Mostrar(destino, acao(), sucesso: true);
            return true;
        }
        catch (ErroDeNegocio erro)
        {
            Mostrar(destino, erro.Message, sucesso: false);
            return false;
        }
    }

    /// <summary>Executa a função e devolve o resultado; em caso de erro, mostra a mensagem e devolve null.</summary>
    public static T? Tentar<T>(TextBlock destino, Func<T> funcao) where T : class
    {
        try
        {
            Ocultar(destino);
            return funcao();
        }
        catch (ErroDeNegocio erro)
        {
            Mostrar(destino, erro.Message, sucesso: false);
            return null;
        }
    }

    public static void Mostrar(TextBlock destino, string texto, bool sucesso)
    {
        destino.Text = texto;
        destino.Style = (Style)Application.Current.FindResource(sucesso ? "TextoMensagemSucesso" : "TextoMensagemErro");
        destino.Visibility = Visibility.Visible;
    }

    public static void Ocultar(TextBlock destino)
    {
        destino.Visibility = Visibility.Collapsed;
    }
}
