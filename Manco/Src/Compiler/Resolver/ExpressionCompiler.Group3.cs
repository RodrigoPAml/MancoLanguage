using Language.Lexer.Enums;
using Language.Compiler.Entities;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Compila tokens > < >= <= == != GRUPO 3
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<CompilerToken> ResolvePriorityTokensGroup3(List<CompilerToken> tokens, CompilationInfo info)
        {
            List<CompilerToken> resolved = new List<CompilerToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.GREATER:
                    case TokenType.GREATER_EQ:
                    case TokenType.LESS:
                    case TokenType.LESS_EQ:
                    case TokenType.EQUALS:
                    case TokenType.NOT_EQUAL:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.GREATER:
                                    resolved[resolved.Count() - 1] = Greater(resolved.Last(), token, info);
                                    break;
                                case TokenType.GREATER_EQ:
                                    resolved[resolved.Count() - 1] = GreaterEqual(resolved.Last(), token, info);
                                    break;
                                case TokenType.LESS:
                                    resolved[resolved.Count() - 1] = Less(resolved.Last(), token, info);
                                    break;
                                case TokenType.LESS_EQ:
                                    resolved[resolved.Count() - 1] = LessEqual(resolved.Last(), token, info);
                                    break;
                                case TokenType.EQUALS:
                                    resolved[resolved.Count() - 1] = Equals(resolved.Last(), token, info);
                                    break;
                                case TokenType.NOT_EQUAL:
                                    resolved[resolved.Count() - 1] = NotEquals(resolved.Last(), token, info);
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
