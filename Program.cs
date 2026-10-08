using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var banco = new Banco();

        while (true)
        {
            Console.WriteLine("\n1 - Criar conta");
            Console.WriteLine("2 - Consultar saldo");
            Console.WriteLine("3 - Depositar");
            Console.WriteLine("4 - Sacar");
            Console.WriteLine("5 - Listar contas");
            Console.WriteLine("0 - Sair");
            Console.Write("Opção: ");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Número da conta: ");
                        int numero = int.Parse(Console.ReadLine());
                        Console.Write("Titular: ");
                        string titular = Console.ReadLine();
                        Console.Write("Saldo inicial: ");
                        decimal saldoInicial = decimal.Parse(Console.ReadLine());
                        banco.CriarConta(numero, titular, saldoInicial);
                        Console.WriteLine("Conta criada com sucesso.");
                        break;

                    case "2":
                        Console.Write("Número da conta: ");
                        Conta consulta = banco.BuscarConta(int.Parse(Console.ReadLine()));
                        Console.WriteLine($"Saldo: {consulta.Saldo:C}");
                        break;

                    case "3":
                        Console.Write("Número da conta: ");
                        Conta deposito = banco.BuscarConta(int.Parse(Console.ReadLine()));
                        Console.Write("Valor: ");
                        deposito.Depositar(decimal.Parse(Console.ReadLine()));
                        Console.WriteLine($"Novo saldo: {deposito.Saldo:C}");
                        break;

                    case "4":
                        Console.Write("Número da conta: ");
                        Conta saque = banco.BuscarConta(int.Parse(Console.ReadLine()));
                        Console.Write("Valor: ");
                        saque.Sacar(decimal.Parse(Console.ReadLine()));
                        Console.WriteLine($"Novo saldo: {saque.Saldo:C}");
                        break;

                    case "5":
                        foreach (Conta conta in banco.ListarContas())
                            Console.WriteLine($"Conta: {conta.Numero} | Titular: {conta.Titular} | Saldo: {conta.Saldo:C}");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
    }
}