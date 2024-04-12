using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida semantica do código utilizando uma arvore de estados e uma stack para controle escopo
    /// Este é o nó raiz
    /// </summary>
    public class Root : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                return;

            switch (tokens[position].Type)
            {
                // Valida semantica de declaração da função
                case TokenType.FUNCTION:
                    new Function().Validate(position + 1, tokens, scopes);
                    break;
                // Valida loop
                case TokenType.WHILE:
                    new Loop().Validate(position + 1, tokens, scopes);
                   break;
                case TokenType.END:
                    new End().Validate(position + 1, tokens, scopes);
                    break;
                case TokenType.ELSE:
                    new Else().Validate(position + 1, tokens, scopes);
                    break;
                // Valida condicionais
                case TokenType.IF:
                case TokenType.ELSEIF:
                   new Condition().Validate(position + 1, tokens, scopes);
                    break;
                // Valida atribuição ou chamada de função
                case TokenType.IDENTIFIER:
                    new Name(true).Validate(position + 1, tokens, scopes);
                    break;
                // Valida semantica na declaração de variaveis
                case TokenType.INTEGER_DECL:
                case TokenType.STRING_DECL:
                case TokenType.BOOLEAN_DECL:
                case TokenType.DECIMAL_DECL:
                    new Type().Validate(position + 1, tokens, scopes);
                    break;
                case TokenType.COMMENT:
                case TokenType.BREAK:
                case TokenType.CONTINUE:
                case TokenType.RETURN:
                    break;
                default:
                    throw new SemanticException($"Invalid semantic at {tokens[position]}", tokens[position], null);
            }
        }
    }
}
