using Manco;

public class Program
{
    private static void Main(string[] args)
    {
        MancoProvider provider = new MancoProvider();

        string code = "function main()\n" +
                      "    print(10 * 2)\n" +
                      "end";

        provider.SetCode(code);

        // Valida faz executar a parte lexica, sintatica e semantica, mas não compila o código
        provider.Validate();

        // ou então chame compilar, que faz toda parte acima mais a compilação
        var compiledCode = provider.Compile();

        compiledCode.ForEach(x => Console.WriteLine(x));
    }
}
