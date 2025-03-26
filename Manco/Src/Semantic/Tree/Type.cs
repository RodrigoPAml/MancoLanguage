using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;
using Manco.Semantic.Exceptions;

namespace Manco.Semantic.Tree
{
    /// <summary>
    /// Valida semantica na declaração de variavel
    /// </summary>
    public class Type : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            switch (tokens[position].Type)
            {
                case TokenType.IDENTIFIER:
                    new Name().Validate(position + 1, tokens, scopes);
                    break;
                default:
                    throw new SemanticException($"Invalid element after type, expected IDENTIFIER but got at {tokens[position]}", tokens[position], ErrorCode.Type);
            }
        }
    }
}
