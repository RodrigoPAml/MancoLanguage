using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida sintaxe de uma expressão utilizando maquina de estados
    /// </summary>
    public class Expression : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count)
                throw new SyntaxException($"Expression with empty body not allowed", tokens[position - 1], ErrorCode.Expression);

            int delimiterCount = 0;
            bool isPreviousIdentifier = false;

            ExpressionState state = ExpressionState.Element;

            foreach (Token token in tokens.Skip(position))
            {
                switch (state)
                {
                    case ExpressionState.Element:
                        {
                            switch (token.Type)
                            {
                                case TokenType.IDENTIFIER:
                                    isPreviousIdentifier = true;
                                    state = ExpressionState.Operator;
                                    break;
                                case TokenType.CLOSE:
                                case TokenType.INTEGER_VAL:
                                case TokenType.BOOL_VAL:
                                case TokenType.DECIMAL_VAL:
                                case TokenType.STRING_VAL:
                                    state = ExpressionState.Operator;
                                    break;
                                case TokenType.OPEN:
                                    delimiterCount++;
                                    break;
                                case TokenType.MINUS:
                                    break;
                                default:
                                    throw new SyntaxException($"Element {token} forms an invalid expression", token, ErrorCode.Expression);
                            }
                            break;
                        }
                    case ExpressionState.Operator:
                        {
                            switch (token.Type)
                            {
                                case TokenType.OPEN_BRACKET:
                                    if(!isPreviousIdentifier)
                                        throw new SyntaxException($"Element {token} forms an invalid expression", token, ErrorCode.Expression);
                                    isPreviousIdentifier = false;
                                    state = ExpressionState.ArrayAccess;
                                    break;
                                case TokenType.DIVIDE:
                                case TokenType.PERCENTAGE:
                                case TokenType.EQUALS:
                                case TokenType.GREATER:
                                case TokenType.GREATER_EQ:
                                case TokenType.LESS:
                                case TokenType.LESS_EQ:
                                case TokenType.NOT_EQUAL:
                                case TokenType.MULT:
                                case TokenType.MINUS:
                                case TokenType.PLUS:
                                case TokenType.OR:
                                case TokenType.AND:
                                    isPreviousIdentifier = false;
                                    state = ExpressionState.Element;
                                    break;
                                case TokenType.CLOSE:
                                    isPreviousIdentifier = false;
                                    delimiterCount--;
                                    break;
                                default:
                                    throw new SyntaxException($"Element {token} forms an invalid expression", token, ErrorCode.Expression);
                            }
                            break;
                        }
                    case ExpressionState.ArrayAccess:
                        switch (token.Type)
                        {
                            case TokenType.IDENTIFIER:
                            case TokenType.INTEGER_VAL:
                                state = ExpressionState.ArrayEnd;
                                break;
                            default:
                                throw new SyntaxException($"Element {token} forms an invalid expression", token, ErrorCode.Expression);
                        }
                        break;
                    case ExpressionState.ArrayEnd:
                        switch (token.Type)
                        {
                            case TokenType.CLOSE_BRACKET:
                            case TokenType.INTEGER_VAL:
                                state = ExpressionState.Operator;
                                break;
                            default:
                                throw new SyntaxException($"Element {token} forms an invalid expression", token, ErrorCode.Expression);
                        }
                        break;
                }
            }

            List<TokenType> endTokensAllowed = new List<TokenType>()
            {
                TokenType.CLOSE,
                TokenType.IDENTIFIER,
                TokenType.INTEGER_VAL,
                TokenType.DECIMAL_VAL,
                TokenType.BOOL_VAL,
                TokenType.STRING_VAL,
                TokenType.CLOSE_BRACKET,
            };

            if (state == ExpressionState.String)
                throw new SyntaxException($"Expression with unterminated string", tokens.Last(), ErrorCode.Expression);

            if (!endTokensAllowed.Contains(tokens.Last().Type))
                throw new SyntaxException($"Expression not allowed to end with element", tokens.Last(), ErrorCode.Expression);

            if (delimiterCount != 0)
                throw new SyntaxException($"Expression with missing CLOSE or OPEN elements", tokens.Last(), ErrorCode.Expression);
        }

        internal enum ExpressionState
        {
            Element,
            Operator,
            String,
            ArrayAccess,
            ArrayEnd
        }
    }
}
