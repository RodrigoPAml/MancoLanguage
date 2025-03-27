using Manco.Common.Enums;
using Manco.Common.Wrapper;

namespace Manco
{
    /// <summary>
    /// Example on how to use the Manco to C++ transpilation
    /// </summary>
    public class ExampleCpp
    {
        public static void Execute(string[] args)
        {
            MancoProvider provider = new MancoProvider();

            string code = "function main()\n" +
                          "    print(10 * 2)\n" +
                          "    if 1 + 1 == 2\n" +
                          "        print(\"\\n1 + 1 is 2\")\n" +
                          "    end\n" +
                          "end\n";
            Console.WriteLine(code);

            provider.SetCode(code);

            // Valida faz executar a parte lexica, sintatica e semantica, mas não transpila o código
            provider.Validate();

            // ou então chame transformar, que faz toda parte acima mais a transpilação
            var compiledCode = provider.Transform(TransformerType.TranspiledCPlusPlus);

            Console.WriteLine("Código transpilado em C++:");
            Console.WriteLine();

            compiledCode.ForEach(x => Console.WriteLine(x));

            // Com o código em mãos pode invocar g++ ou outro compilador de c++
            // Ao rodar a UI ele irá invocar o g++ mas deve estar instalado
            // Faremos o mesmo aqui

            Console.WriteLine("Invocando g++");
            var result = CPlusPlusWraper.Compile(string.Join('\n', compiledCode.ToArray()));
            
            Console.WriteLine($"{result.Output}");

            if(!result.Error)
            {
                Console.WriteLine("Invocando programa compilado:\n");
                result = CPlusPlusWraper.Execute();

                Console.WriteLine($"{result.Output}");
            }
        }
    }
}