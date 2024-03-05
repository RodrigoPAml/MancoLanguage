using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
{
    /// <summary>
    /// Valida declaração de variavel
    /// </summary>
    public class Type : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if(position >= tokens.Count())
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.Type);

            // Não pode no escopo global
            if(scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Invalid token {tokens[position]} at global scope", tokens[position], ErrorCode.Type);

            switch (tokens[position].Type)
            {
                // Continua validação de declaração de variavel
                case TokenType.IDENTIFIER:
                    new Name().Validate(position + 1, tokens, scopes);
                    break;
                default:
                    throw new SyntaxException($"Invalid token {tokens[position]}", tokens[position], ErrorCode.Type);
            }
        }
    }
}
