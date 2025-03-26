using Manco.Common.Enums;
using Manco.Common.Interfaces;
using Manco.Compiler;
using Manco.Lexer;
using Manco.Lexer.Entities;
using Manco.Semantic;
using Manco.Syntatic;
using Manco.Transpiler;
using System.Globalization;

namespace Manco
{
    /// <summary>
    /// Provedor da linguagem manco, utilizado para sua validação e compilação/transpilação
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
            var lexer = new LexerProvider();
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

            var lexer = new LexerProvider();
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
            var lexer = new LexerProvider();
            lexer.Parse(_text);

            return lexer.GetResult();
        }

        /// <summary>
        /// Transforma código e retorna as novas instruções
        /// </summary>
        /// <returns></returns>
        public List<string> Transform(TransformerType type)
        {
            var lexer = new LexerProvider();
            lexer.Parse(_text);

            var tokens = lexer.GetResult();

            SyntaxChecker checker = new SyntaxChecker();
            checker.Parse(tokens);

            SemanticChecker semanticChecker = new SemanticChecker();
            semanticChecker.Parse(tokens);

            ITransformer transformer = null;

            switch(type)
            {
                case TransformerType.CompiledMIPS:
                    transformer = new MipsCompiler();
                    break;
                case TransformerType.TranspiledCPlusPlus:
                    transformer = new CPlusPlusTranspiler();
                    break;
                default:
                    throw new Exception($"Invalid transformer type {type}");
            }

            return transformer.Execute(tokens);
        }
    }
}
