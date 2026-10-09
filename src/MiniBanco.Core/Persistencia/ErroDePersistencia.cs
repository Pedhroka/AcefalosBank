using System;

namespace MiniBanco.Core.Persistencia;

/// <summary>
/// Falha ao ler ou gravar os dados. Não é um erro do usuário, por isso não deve ser tratado como ErroDeNegocio.
/// </summary>
public class ErroDePersistencia : Exception
{
    public ErroDePersistencia(string mensagem) : base(mensagem)
    {
    }
}
