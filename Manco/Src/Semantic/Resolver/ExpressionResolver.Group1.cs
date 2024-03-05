using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve tokens * / % GRUPO 1
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<ReducedToken> ResolvePriorityTokensGroup1(List<ReducedToken> tokens)
        {
            List<ReducedToken> resolved = new List<ReducedToken>();

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
                                    if (float.Parse(token.Content) == 0)
                                        throw new Exception("Division by zero");

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

        private List<ReducedToken> ResolvePriorityTokensGroup12(List<ReducedToken> tokens)
        {
            List<ReducedToken> resolved = new List<ReducedToken>();

            TokenType? operation = null;

            // Resolve division an multiplication first
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Operators
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
