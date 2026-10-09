using MiniBanco.Core.Dominio;
using MiniBanco.Core.Persistencia;

namespace MiniBanco.Core.Testes.Persistencia;

public class RepositorioContasTests : IDisposable
{
    private readonly string _pasta = Path.Combine(Path.GetTempPath(), "MiniBanco.Testes", Guid.NewGuid().ToString());
    private readonly string _caminho;

    public RepositorioContasTests()
    {
        _caminho = Path.Combine(_pasta, "contas.json");
    }

    public void Dispose()
    {
        if (Directory.Exists(_pasta)) Directory.Delete(_pasta, recursive: true);
    }

    [Fact]
    public void SalvarECarregar_PreservaSaldoMovimentosESenha()
    {
        var repositorio = new RepositorioContas(_caminho);
        Conta origem = Conta.Abrir(1001, "Ana Souza", "senha1", 300m);
        Conta destino = Conta.Abrir(1002, "Bruno Lima", "senha2", 0m);
        origem.Debitar(100m, TipoMovimento.DepositoEnviado, destino);
        destino.Creditar(100m, TipoMovimento.DepositoRecebido, origem);

        repositorio.Salvar(new[] { origem, destino });
        IReadOnlyList<Conta> carregadas = repositorio.Carregar();

        Conta ana = carregadas.Single(c => c.Numero == 1001);
        Assert.Equal("Ana Souza", ana.Titular);
        Assert.Equal(200m, ana.Saldo);
        Assert.Equal(2, ana.Movimentos.Count);
        Assert.Equal(TipoMovimento.DepositoEnviado, ana.Movimentos[^1].Tipo);
        Assert.Equal("Bruno Lima", ana.Movimentos[^1].NomeContraparte);
        Assert.True(ana.VerificarSenha("senha1"));
    }

    [Fact]
    public void Carregar_ArquivoInexistente_RetornaListaVazia()
    {
        var repositorio = new RepositorioContas(_caminho);

        Assert.Empty(repositorio.Carregar());
    }

    [Fact]
    public void Carregar_ArquivoInvalido_LancaErroDePersistencia()
    {
        Directory.CreateDirectory(_pasta);
        File.WriteAllText(_caminho, "{ conteudo corrompido");
        var repositorio = new RepositorioContas(_caminho);

        Assert.Throws<ErroDePersistencia>(() => repositorio.Carregar());
    }

    [Fact]
    public void Salvar_NaoDeixaArquivoTemporarioParaTras()
    {
        var repositorio = new RepositorioContas(_caminho);

        repositorio.Salvar(new[] { Conta.Abrir(1001, "Ana", "senha", 0m) });

        Assert.True(File.Exists(_caminho));
        Assert.False(File.Exists(_caminho + ".tmp"));
    }
}
