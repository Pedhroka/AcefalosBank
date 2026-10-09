using System;

namespace MiniBanco.Core.Dominio;

/// <summary>
/// Violação de uma regra de negócio. A mensagem pode ser exibida diretamente ao usuário.
/// </summary>
public class ErroDeNegocio : Exception
{
    public ErroDeNegocio(string mensagem) : base(mensagem)
    {
    }
}
