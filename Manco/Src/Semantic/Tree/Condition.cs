using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;
using Manco.Semantic.Exceptions;

namespace Manco.Semantic.Tree
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
