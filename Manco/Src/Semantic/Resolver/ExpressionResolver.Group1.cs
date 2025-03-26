using Manco.Lexer.Enums;
using Manco.Semantic.Entities;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve tokens * / % GRUPO 1
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<SemanticToken> ResolvePriorityTokensGroup1(List<SemanticToken> tokens)
        {
            var resolved = new List<SemanticToken>();

            TokenType? operation = null;

            // Resolve division an multiplication first
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Operators
                    case TokenType.DIVIDE:
                    case TokenType.MULT:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.DIVIDE:
                                    if (float.TryParse(token.Content, out float number))
                                    {
                                        if(number == 0)
                                            throw new Exception("Division by zero");
                                    }

                                    resolved[resolved.Count() - 1] = Divide(resolved.Last(), token);
                                    break;
                                case TokenType.MULT:
                                    resolved[resolved.Count() - 1] = Mult(resolved.Last(), token);
                                    break;
                            }

                            operation = null;
                        }
                        else resolved.Add(token);
                        break;
                }
            }

            return ResolvePriorityTokensGroup12(resolved);
        }

        /// <summary>
        /// Resolve tokens * / % GRUPO 1
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private List<SemanticToken> ResolvePriorityTokensGroup12(List<SemanticToken> tokens)
        {
            var resolved = new List<SemanticToken>();

            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.PERCENTAGE:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.PERCENTAGE:
                                    resolved[resolved.Count() - 1] = Mod(resolved.Last(), token);
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
