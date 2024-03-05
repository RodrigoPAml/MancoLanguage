using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve tokens + - GRUPO 2
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<ReducedToken> ResolvePriorityTokensGroup2(List<ReducedToken> tokens)
        {
            List<ReducedToken> resolved = new List<ReducedToken>();
            TokenType? operation = null;

            if (tokens[0].Type == TokenType.MINUS)
            {
                tokens.Insert(0, new ReducedToken()
                {
                    Content = "0",
                    Type = TokenType.INTEGER_VAL,
                });
            }

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Operators
                    case TokenType.PLUS:
                    case TokenType.MINUS:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.PLUS:
                                    resolved[resolved.Count() - 1] = Add(resolved.Last(), token);
                                    break;
                                case TokenType.MINUS:
                                    resolved[resolved.Count() - 1] = Sub(resolved.Last(), token);
                                    break;
                            }

                            operation = null;
                        }
                        else resolved.Add(token);
                        break;
                }
            }

            return resolved;
        }
    }
}
