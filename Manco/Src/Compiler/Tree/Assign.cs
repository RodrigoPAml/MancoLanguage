using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila atribuições normais
    /// </summary>
    public class Assign : CompilerTree
    {
        private CompilerToken? _result = null;

        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
             int indexVariable =
                tokens[0].Type == TokenType.IDENTIFIER
                ? 0
                : 1;

            var variable = scopes
                .SelectMany(x => x.Variables)
                .Where(x => x.Name == tokens[indexVariable].Content)
                .FirstOrDefault();

            string varName = variable?.Name ?? string.Empty;
            var type = variable?.Type ?? TokenType.ANY;

            var expr = new Expression(
                type == TokenType.STRING_DECL
                    ? ExpressionRestriction.StringDeclaration
                    : ExpressionRestriction.None,
                indexVariable != 0
                    ? varName
                    : string.Empty
           );
      
            expr.Validate(position, tokens, scopes, info);
            _result = expr.GetResult();
        }

        /// <summary>
        /// Retorna resultado
        /// </summary>
        /// <returns></returns>
        public CompilerToken? GetResult()
        {
            return _result;
        }
    }
}
