using Language.Lexer.Enums;
using Language.Compiler.Entities;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Compila tokens + - GRUPO 2
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<CompilerToken> ResolvePriorityTokensGroup2(List<CompilerToken> tokens, CompilationInfo info)
        {
            List<CompilerToken> resolved = new List<CompilerToken>();
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
                                    resolved[resolved.Count() - 1] = Add(resolved.Last(), token, info);
                                    break;
                                case TokenType.MINUS:
                                    resolved[resolved.Count() - 1] = Sub(resolved.Last(), token, info);
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
