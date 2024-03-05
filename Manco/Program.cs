using Language.Lexer;
using Language.Lexer.Exceptions;
using Language.Semantic;
using Language.Semantic.Exceptions;
using Language.Syntatic;
using Language.Syntatic.Exceptions;
using System.Globalization;

public class Program
{
    private static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        Lexer lexer = new Lexer();

        try
        {
            lexer.Parse("C:\\Users\\Rodrigo\\Desktop\\FiniteStateMachine\\Files\\code.txt");
        }
        catch (LexerException le)
        {
            Console.WriteLine("Lexical error:" + le.Message);
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine("Fatal error:" + e.Message);
            return;
        }

        var tokens = lexer.GetResult();
        lexer.Print();

        SyntaxChecker checker = new SyntaxChecker();

        try
        {
            checker.Parse(tokens);
        }
        catch (SyntaxException le)
        {
            Console.WriteLine("Syntax error:" + le.Message);
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine("Fatal error:" + e.Message);
            return;
        }

        SemanticChecker semanticChecker = new SemanticChecker();
        semanticChecker.Parse(tokens);

        try
        {
        }
        catch (SemanticException le)
        {
            Console.WriteLine("Semantic error:" + le.Message);
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine("Fatal error:" + e.Message);
            return;
        }
    }
}
