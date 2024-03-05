using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve tokens AND then OR GRUPO 4
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<ReducedToken> ResolvePriorityTokensGroup4(List<ReducedToken> tokens)
        {
            List<ReducedToken> resolved = new List<ReducedToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Operators
                    case TokenType.AND:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.AND:
                                    resolved[resolved.Count() - 1] = And(resolved.Last(), token);
                                    break;
                            }

                            operation = null;
                        }
                        else resolved.Add(token);
                        break;
                }
            }

            return ResolvePriorityTokensGroup41(resolved);
        }

        private List<ReducedToken> ResolvePriorityTokensGroup41(List<ReducedToken> tokens)
        {
            List<ReducedToken> resolved = new List<ReducedToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Operators
                    case TokenType.OR:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.OR:
                                    resolved[resolved.Count() - 1] = Or(resolved.Last(), token);
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
