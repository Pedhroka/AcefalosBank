using System;
using System.Collections.Generic;
using System.Linq;

public class Banco
{
    private readonly RepositorioContas _repositorio;
    private readonly Dictionary<int, Conta> _contas;

    public Banco(RepositorioContas repositorio)
    {
        _repositorio = repositorio;
        _contas = repositorio.Carregar().ToDictionary(c => c.Numero);
    }

    public void CriarConta(int numero, string titular, string senha, decimal saldoInicial)
    {
        if (_contas.ContainsKey(numero)) throw new InvalidOperationException("Número de conta já existe.");
        if (string.IsNullOrWhiteSpace(titular)) throw new ArgumentException("Informe o titular da conta.");
        if (string.IsNullOrWhiteSpace(senha)) throw new ArgumentException("Informe uma senha.");
        if (saldoInicial < 0) throw new ArgumentException("Saldo inicial não pode ser negativo.");

        var conta = new Conta(numero, titular.Trim(), Seguranca.GerarHash(senha));
        if (saldoInicial > 0) conta.Creditar(saldoInicial, "Abertura de conta");

        _contas[numero] = conta;
        Salvar();
    }

    public Conta Autenticar(int numero, string senha)
    {
        if (!_contas.TryGetValue(numero, out Conta? conta) || !Seguranca.Verificar(senha, conta.SenhaHash))
            throw new InvalidOperationException("Número da conta ou senha inválidos.");

        return conta;
    }

    public Conta BuscarConta(int numero)
    {
        if (!_contas.TryGetValue(numero, out Conta? conta)) throw new KeyNotFoundException("Conta não encontrada.");
        return conta;
    }

    public void Sacar(int numero, decimal valor)
    {
        BuscarConta(numero).Debitar(valor, "Saque");
        Salvar();
    }

    public void Transferir(int numeroOrigem, int numeroDestino, decimal valor)
    {
        if (numeroOrigem == numeroDestino) throw new InvalidOperationException("Não é possível depositar na própria conta.");

        Conta origem = BuscarConta(numeroOrigem);
        Conta destino = BuscarConta(numeroDestino);

        origem.Debitar(valor, $"Depósito enviado para {destino.Titular}", destino);
        destino.Creditar(valor, $"Depósito recebido de {origem.Titular}", origem);
        Salvar();
    }

    private void Salvar() => _repositorio.Salvar(_contas.Values);
}
