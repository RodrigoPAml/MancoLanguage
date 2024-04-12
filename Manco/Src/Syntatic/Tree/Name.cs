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
    /// Valida sintaxe de um assign de variavel ou chamada de função
    /// </summary>
    public class Name : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (tokens.Count == 1)
                throw new SyntaxException($"Missing content after identifier", tokens[0], ErrorCode.Identifier);

            if (position >= tokens.Count())
                return;

            // Nada disso no escopo global
            if (scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Identifier cant appear on global scope", tokens[position], ErrorCode.Identifier);

            switch (tokens[position].Type)
            {
                // Chamada de função
                case TokenType.OPEN:
                    new FunctionCall().Validate(position, tokens, scopes);
                    break;
                // Atribuição de variavel
                case TokenType.ASSIGN:
                    new Assign().Validate(position + 1, tokens, scopes);
                    break;
                // Array, mas verifica dentro se foi precedido de um type pelo tamanho
                case TokenType.LESS:
                    new Array().Validate(position + 1, tokens, scopes);
                    break;
                // Assign de Array, mas verifica se não foi precedido de um type pelo tamanho
                case TokenType.OPEN_BRACKET:
                    new ArrayAssign().Validate(position + 1, tokens, scopes);
                    break;
                default:
                    throw new SyntaxException($"Invalid identifier operation", tokens[position], ErrorCode.Identifier);
            }
        }
    }
}
