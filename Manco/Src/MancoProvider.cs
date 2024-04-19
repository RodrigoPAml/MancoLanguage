using Language.Compiler;
using Language.Lexer;
using Language.Semantic;
using Language.Syntatic;
using System.Globalization;

namespace Manco
{
    /// <summary>
    /// The Manco main class provider
    /// </summary>
    public class MancoProvider
    {
        private string _text = string.Empty;

        public MancoProvider()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }

        public void SetCode(string text)
        {
            _text = text ?? string.Empty;
        }

        public void Validate()
        {
            Lexer lexer = new Lexer();
            lexer.Parse(_text);

            var tokens = lexer.GetResult();

            SyntaxChecker checker = new SyntaxChecker();
            checker.Parse(tokens);

            SemanticChecker semanticChecker = new SemanticChecker();
            semanticChecker.Parse(tokens);
        }

        public string GetTokensToString()
        {
            string result = string.Empty;   

            Lexer lexer = new Lexer();
            lexer.Parse(_text);

            foreach(var tokens in lexer.GetResult())
            {
                foreach(var token in tokens)
                {
                    result += token.Type.ToString() + $" ({token.Content}) "; 
                }

                result += "\n";
            }

            return result;
        }

        public List<string> Compile()
        {
            Lexer lexer = new Lexer();
            lexer.Parse(_text);

            var tokens = lexer.GetResult();

            SyntaxChecker checker = new SyntaxChecker();
            checker.Parse(tokens);

            SemanticChecker semanticChecker = new SemanticChecker();
            semanticChecker.Parse(tokens);

            Compiler compiler = new Compiler(); 
            return compiler.Compile(tokens);
        }
    }
}
