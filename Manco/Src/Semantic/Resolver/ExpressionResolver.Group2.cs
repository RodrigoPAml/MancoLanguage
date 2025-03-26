using Manco.Lexer.Enums;
using Manco.Semantic.Entities;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve tokens + - GRUPO 2
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<SemanticToken> ResolvePriorityTokensGroup2(List<SemanticToken> tokens)
        {
            var resolved = new List<SemanticToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
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
