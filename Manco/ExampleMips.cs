﻿using AssemblerSimulator;
using Manco.Common.Enums;
using System.Text;

namespace Manco
{
    /// <summary>
    /// Exemplo de como compilar para MIPS e executar com o simulador de assembly
    /// </summary>
    public class ExampleMips
    {
        // Callback quando valor do registrador muda
        public static void OnRegisterChange(string name, byte[] value)
        {
            //Console.WriteLine($"{nameof(OnRegisterChange)} {name} {string.Join(" ", value)}");
        }

        // Callback quando memoria muda
        public static void OnMemoryChange(int address, byte value)
        {
            //Console.WriteLine($"{nameof(OnMemoryChange)} {address} {value}");
        }

        // Callback para syscall
        public static void OnSyscall(int code, byte[] value)
        {
            if (code == 1)
            {
                Console.WriteLine(BitConverter.ToInt32(value));
            }
            else if (code == 2)
            {
                Console.WriteLine(BitConverter.ToSingle(value));
            }
            else if (code == 3)
            {
                var fl = Encoding.ASCII.GetString(new byte[] { value[0] });
                fl = fl.Replace("\n", Environment.NewLine);
                Console.WriteLine(fl);
            }
        }

        public static void Execute(string[] args)
        {
            MancoProvider provider = new MancoProvider();

            string code = "function main()\n" +
                          "    print(10 * 2)\n" +
                          "end\n";

            provider.SetCode(code);

            // Valida faz executar a parte lexica, sintatica e semantica, mas não compila o código
            provider.Validate();

            // ou então chame transform, que faz toda parte acima mais a compilação
            var compiledCode = provider.Transform(TransformerType.CompiledMIPS);

            Console.WriteLine("Código compilado:");
            Console.WriteLine();

            compiledCode.ForEach(x => Console.WriteLine(x));

            Console.WriteLine();
            Console.WriteLine("Saída:");
            Console.WriteLine();

            // Simula a execução do assembly MIPS
            Emulator emulator = new Emulator(OnMemoryChange, OnRegisterChange, OnSyscall);
            emulator.AddInstructions(compiledCode);
            emulator.ValidateInstructions();
            emulator.ExecuteAll();
        }
    }
}