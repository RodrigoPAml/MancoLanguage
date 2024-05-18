using Language.Compiler;
using Language.Lexer;
using Language.Lexer.Entities;
using Language.Semantic;
using Language.Syntatic;
using System.Globalization;

namespace Manco
{
    /// <summary>
    /// Provedor da linguagem manco, utilizado para sua validação e compilação
    /// </summary>
    public class MancoProvider
    {
        private string _text = string.Empty;

        public MancoProvider()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Seta código
        /// </summary>
        /// <param name="text"></param>
        public void SetCode(string text)
        {
            _text = text ?? string.Empty;
        }

        /// <summary>
        /// Valida código, fases lexica, sintatica e semantica
        /// Caso houver erro ira lançar exceção especifica para cada fase
        /// </summary>
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

        /// <summary>
        /// Retorna tokens em formato de string para visualização
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Retorna tokens em formato de lista, separado por linha
        /// </summary>
        /// <returns></returns>
        public List<List<Token>> GetTokens()
        {
            var lexer = new Lexer();
            lexer.Parse(_text);

            return lexer.GetResult();
        }

        /// <summary>
        /// Compila código e retorna instruções em assembly formato baseado em arquitetura MIPS
        /// </summary>
        /// <returns></returns>
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
