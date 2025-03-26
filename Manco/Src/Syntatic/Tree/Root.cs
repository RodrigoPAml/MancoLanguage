using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida a sintaxe do código usando uma árvore de estados (FSM), em outras palavras uma maquina de estado finita
    /// Tambem utiliza uma stack para manter controle dos escopos do código
    /// Esta aqui é a raiz da árvore
    /// </summary>
    public class Root : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                return;

            // Adiciona comandos do escopo
            scopes.First().Commands.Add(tokens[position].Type);

            switch (tokens[position].Type)
            {
                // Valida return
                case TokenType.RETURN:
                    new Return().Validate(position + 1, tokens, scopes);
                    break;
                // Valida declaração de função
                case TokenType.FUNCTION:
                    new Function().Validate(position + 1, tokens, scopes);
                    break;
                // Valida while
                case TokenType.WHILE:
                    new Loop().Validate(position + 1, tokens, scopes);
                    break;
                // Valida fechamento de escopo
                case TokenType.END:
                    new End().Validate(position + 1, tokens, scopes);
                    break;
                // Valida else
                case TokenType.ELSE:
                    new Else().Validate(position + 1, tokens, scopes);
                    break;
                // Valida condição if e else if
                case TokenType.IF:
                case TokenType.ELSEIF:
                    new Condition().Validate(position + 1, tokens, scopes);
                    break;
                // Valida atribuição de variavel ou chamada de função
                case TokenType.IDENTIFIER:
                    new Name().Validate(position + 1, tokens, scopes);
                    break;
                case TokenType.COMMENT:
                    break;
                // Valida declaração de variavel
                case TokenType.INTEGER_DECL:
                case TokenType.STRING_DECL:
                case TokenType.BOOLEAN_DECL:
                case TokenType.DECIMAL_DECL:
                    new Type().Validate(position + 1, tokens, scopes);
                    break;
                case TokenType.CONTINUE:
                    new Continue().Validate(position + 1, tokens, scopes);
                    break;
                case TokenType.BREAK:
                    new Break().Validate(position + 1, tokens, scopes);
                    break;
                default:
                    throw new SyntaxException($"Invalid starting element", tokens[position], ErrorCode.InvalidToken);
            }
        }
    }
}
