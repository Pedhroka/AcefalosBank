using MiniBanco.Core.Dominio;

namespace MiniBanco.Core.Testes.Dominio;

public class BancoTests
{
    private static (Banco banco, RepositorioEmMemoria repositorio) CriarBancoComDuasContas()
    {
        var repositorio = new RepositorioEmMemoria();
        var banco = new Banco(repositorio);
        banco.CriarConta(1001, "Ana Souza", "senha1", 500m);
        banco.CriarConta(1002, "Bruno Lima", "senha2", 0m);
        return (banco, repositorio);
    }

    [Fact]
    public void CriarConta_NumeroJaExistente_LancaErroDeNegocio()
    {
        var (banco, _) = CriarBancoComDuasContas();

        ErroDeNegocio erro = Assert.Throws<ErroDeNegocio>(() => banco.CriarConta(1001, "Outra", "senha", 0m));

        Assert.Equal("Número de conta já existe.", erro.Message);
    }

    [Fact]
    public void CriarConta_SalvaNoRepositorio()
    {
        var repositorio = new RepositorioEmMemoria();
        var banco = new Banco(repositorio);

        banco.CriarConta(1001, "Ana", "senha", 0m);

        Assert.Equal(1, repositorio.Salvamentos);
        Assert.Single(repositorio.Carregar());
    }

    [Fact]
    public void Autenticar_ComSenhaCorreta_RetornaAConta()
    {
        var (banco, _) = CriarBancoComDuasContas();

        Conta conta = banco.Autenticar(1001, "senha1");

        Assert.Equal("Ana Souza", conta.Titular);
    }

    [Theory]
    [InlineData(1001, "senhaErrada")]
    [InlineData(9999, "senha1")]
    public void Autenticar_ComDadosInvalidos_LancaErroGenericoDeCredenciais(int numero, string senha)
    {
        var (banco, _) = CriarBancoComDuasContas();

        ErroDeNegocio erro = Assert.Throws<ErroDeNegocio>(() => banco.Autenticar(numero, senha));

        Assert.Equal("Número da conta ou senha inválidos.", erro.Message);
    }

    [Fact]
    public void Sacar_ReduzSaldoEPersiste()
    {
        var (banco, repositorio) = CriarBancoComDuasContas();
        int salvamentosAntes = repositorio.Salvamentos;

        banco.Sacar(1001, 120m);

        Assert.Equal(380m, banco.BuscarConta(1001).Saldo);
        Assert.Equal(salvamentosAntes + 1, repositorio.Salvamentos);
    }

    [Fact]
    public void Transferir_MoveValorEntreAsDuasContas()
    {
        var (banco, _) = CriarBancoComDuasContas();

        Conta destino = banco.Transferir(1001, 1002, 200m);

        Assert.Same(banco.BuscarConta(1002), destino);
        Assert.Equal(300m, banco.BuscarConta(1001).Saldo);
        Assert.Equal(200m, banco.BuscarConta(1002).Saldo);
    }

    [Fact]
    public void Transferir_RegistraLancamentoNaOrigemENoDestino()
    {
        var (banco, _) = CriarBancoComDuasContas();

        banco.Transferir(1001, 1002, 200m);

        Movimento enviado = banco.BuscarConta(1001).Movimentos[^1];
        Movimento recebido = banco.BuscarConta(1002).Movimentos[^1];

        Assert.Equal(TipoMovimento.DepositoEnviado, enviado.Tipo);
        Assert.Equal(-200m, enviado.Valor);
        Assert.Equal("Bruno Lima", enviado.NomeContraparte);

        Assert.Equal(TipoMovimento.DepositoRecebido, recebido.Tipo);
        Assert.Equal(200m, recebido.Valor);
        Assert.Equal("Ana Souza", recebido.NomeContraparte);
        Assert.Equal(1001, recebido.NumeroContraparte);
    }

    [Fact]
    public void Transferir_SaldoInsuficiente_NaoAlteraNenhumaConta()
    {
        var (banco, _) = CriarBancoComDuasContas();

        Assert.Throws<ErroDeNegocio>(() => banco.Transferir(1002, 1001, 1m));

        Assert.Equal(0m, banco.BuscarConta(1002).Saldo);
        Assert.Equal(500m, banco.BuscarConta(1001).Saldo);
        Assert.Empty(banco.BuscarConta(1002).Movimentos);
    }

    [Fact]
    public void Transferir_ParaPropriaConta_LancaErroDeNegocio()
    {
        var (banco, _) = CriarBancoComDuasContas();

        Assert.Throws<ErroDeNegocio>(() => banco.Transferir(1001, 1001, 10m));
    }

    [Fact]
    public void Transferir_ParaContaInexistente_LancaErroDeNegocio()
    {
        var (banco, _) = CriarBancoComDuasContas();

        ErroDeNegocio erro = Assert.Throws<ErroDeNegocio>(() => banco.Transferir(1001, 9999, 10m));

        Assert.Equal("Conta não encontrada.", erro.Message);
        Assert.Equal(500m, banco.BuscarConta(1001).Saldo);
    }
}
