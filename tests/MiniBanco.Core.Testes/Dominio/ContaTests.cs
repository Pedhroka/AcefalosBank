using MiniBanco.Core.Dominio;

namespace MiniBanco.Core.Testes.Dominio;

public class ContaTests
{
    [Fact]
    public void Abrir_ComSaldoInicial_RegistraAberturaComoPrimeiraMovimentacao()
    {
        Conta conta = Conta.Abrir(1001, "Ana Souza", "senha", 100m);

        Assert.Equal(100m, conta.Saldo);
        Movimento movimento = Assert.Single(conta.Movimentos);
        Assert.Equal(TipoMovimento.Abertura, movimento.Tipo);
        Assert.Equal(100m, movimento.SaldoApos);
    }

    [Fact]
    public void Abrir_ComSaldoZero_NaoRegistraMovimentacao()
    {
        Conta conta = Conta.Abrir(1001, "Ana Souza", "senha", 0m);

        Assert.Empty(conta.Movimentos);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public void Abrir_SemTitular_LancaErroDeNegocio(string titular)
    {
        Assert.Throws<ErroDeNegocio>(() => Conta.Abrir(1001, titular, "senha", 0m));
    }

    [Fact]
    public void Abrir_ComSaldoNegativo_LancaErroDeNegocio()
    {
        Assert.Throws<ErroDeNegocio>(() => Conta.Abrir(1001, "Ana", "senha", -1m));
    }

    [Fact]
    public void Debitar_ComSaldoSuficiente_ReduzSaldoERegistraValorNegativo()
    {
        Conta conta = Conta.Abrir(1001, "Ana", "senha", 200m);

        conta.Debitar(50m, TipoMovimento.Saque);

        Assert.Equal(150m, conta.Saldo);
        Movimento saque = conta.Movimentos[^1];
        Assert.Equal(TipoMovimento.Saque, saque.Tipo);
        Assert.Equal(-50m, saque.Valor);
        Assert.Equal(150m, saque.SaldoApos);
    }

    [Fact]
    public void Debitar_ComSaldoInsuficiente_NaoAlteraSaldoNemMovimentacoes()
    {
        Conta conta = Conta.Abrir(1001, "Ana", "senha", 10m);

        ErroDeNegocio erro = Assert.Throws<ErroDeNegocio>(() => conta.Debitar(50m, TipoMovimento.Saque));

        Assert.Equal("Saldo insuficiente.", erro.Message);
        Assert.Equal(10m, conta.Saldo);
        Assert.Single(conta.Movimentos);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Creditar_ComValorNaoPositivo_LancaErroDeNegocio(decimal valor)
    {
        Conta conta = Conta.Abrir(1001, "Ana", "senha", 0m);

        Assert.Throws<ErroDeNegocio>(() => conta.Creditar(valor, TipoMovimento.DepositoRecebido));
    }

    [Fact]
    public void Creditar_RegistraContraparteComNumeroENome()
    {
        Conta origem = Conta.Abrir(1002, "Bruno Lima", "senha", 100m);
        Conta destino = Conta.Abrir(1001, "Ana Souza", "senha", 0m);

        destino.Creditar(30m, TipoMovimento.DepositoRecebido, origem);

        Movimento recebido = destino.Movimentos[^1];
        Assert.Equal(1002, recebido.NumeroContraparte);
        Assert.Equal("Bruno Lima", recebido.NomeContraparte);
        Assert.Equal(30m, recebido.Valor);
    }

    [Fact]
    public void VerificarSenha_AceitaSenhaCorretaERecusaIncorreta()
    {
        Conta conta = Conta.Abrir(1001, "Ana", "segredo", 0m);

        Assert.True(conta.VerificarSenha("segredo"));
        Assert.False(conta.VerificarSenha("outra"));
    }
}
