using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Tree
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
                    throw new SemanticException($"Invalid syntax at {tokens[position]}", tokens[position]);
            }
        }
    }
}
