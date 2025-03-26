using Manco.Lexer.Entities;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;
using Manco.Compiler.Resolver;

namespace Manco.Compiler.Tree
{
    /// <summary> 
    /// Compila expressão
    /// </summary>
    public class Expression : CompilerTree
    {
        /// <summary>
        /// Resultado 
        /// </summary>
        private CompilerToken _result = null;

        /// <summary>
        /// Variavel atual, deve ser desconsiderado seu uso na expressão
        /// </summary>
        private readonly string _currentVariable;

        /// <summary>
        /// Restrição de formação da expressão
        /// </summary>
        private readonly ExpressionRestriction _restriction;

        public Expression(ExpressionRestriction restriction = ExpressionRestriction.None, string currentVariable = "")
        {
            _currentVariable = currentVariable;
            _restriction = restriction;
        }

        /// <summary>
        /// Retorna resultado
        /// </summary>
        /// <returns></returns>
        public CompilerToken GetResult()
        { 
            return _result; 
        }

        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            _result = new ExpressionCompiler().Evaluate(info, tokens.Skip(position).ToList(), scopes, _currentVariable, _restriction);
        }
    }
}
