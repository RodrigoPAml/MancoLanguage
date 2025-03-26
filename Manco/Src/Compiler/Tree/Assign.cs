using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Compila atribuições normais
    /// </summary>
    public class Assign : CompilerTree
    {
        private CompilerToken _result = null;

        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
             int indexVariable =
                tokens[0].Type == TokenType.IDENTIFIER
                ? 0
                : 1;

            var variable = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Name == tokens[indexVariable].Content)
                .FirstOrDefault();

            var varName = variable?.Name ?? string.Empty;
            var type = variable?.Type ?? TokenType.ANY;

            var expr = new Expression(
                type == TokenType.STRING_DECL
                    ? ExpressionRestriction.StringDeclaration
                    : ExpressionRestriction.None,
                indexVariable != 0
                    ? varName
                    : string.Empty
           );
      
            expr.Generate(position, tokens, scopes, info);
            _result = expr.GetResult();
        }

        /// <summary>
        /// Retorna resultado
        /// </summary>
        /// <returns></returns>
        public CompilerToken GetResult()
        {
            return _result;
        }
    }
}
