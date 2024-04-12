using Language.Lexer.Enums;
using Language.Compiler.Entities;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Compila tokens * / % GRUPO 1
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<CompilerToken> ResolvePriorityTokensGroup1(List<CompilerToken> tokens, CompilationInfo info)
        {
            List<CompilerToken> resolved = new List<CompilerToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
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
                                    resolved[resolved.Count() - 1] = Divide(resolved.Last(), token, info);
                                    break;
                                case TokenType.MULT:
                                    resolved[resolved.Count() - 1] = Mult(resolved.Last(), token, info);
                                    break;
                            }

                            operation = null;
                        }
                        else resolved.Add(token);
                        break;
                }
            }

            return ResolvePriorityTokensGroup12(resolved, info);
        }

        /// <summary>
        /// Compila tokens * / % GRUPO 1
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<CompilerToken> ResolvePriorityTokensGroup12(List<CompilerToken> tokens, CompilationInfo info)
        {
            List<CompilerToken> resolved = new List<CompilerToken>();

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
                                    resolved[resolved.Count() - 1] = Mod(resolved.Last(), token, info);
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
