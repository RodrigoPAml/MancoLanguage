using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida condicionais (somente expressões)
    /// </summary>
    public class Condition : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (tokens[position-1].Type == TokenType.IF)
                scopes.Push(new Scope(ScopeType.If));
            else
                scopes.Push(new Scope(ScopeType.ElseIf));

            var expr = new Expression();

            expr.Validate(position, tokens, scopes);
            var result = expr.GetResult();

            if(result != VariableType.Boolean)
                throw new SemanticException($"Expression of conditional is expected to be boolean but got {result}", tokens[position - 1], ErrorCode.Conditions);
        }
    }
}
