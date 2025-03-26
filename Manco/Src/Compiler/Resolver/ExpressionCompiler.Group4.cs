using Manco.Lexer.Enums;
using Manco.Compiler.Entities;

namespace Manco.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Compila tokens AND then OR GRUPO 4
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<CompilerToken> ResolvePriorityTokensGroup4(List<CompilerToken> tokens, CompilationInfo info)
        {
            var resolved = new List<CompilerToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.AND:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.AND:
                                    resolved[resolved.Count() - 1] = And(resolved.Last(), token, info);
                                    break;
                            }

                            operation = null;
                        }
                        else resolved.Add(token);
                        break;
                }
            }

            return ResolvePriorityTokensGroup41(resolved, info);
        }

        /// <summary>
        /// Compila tokens AND then OR GRUPO 4
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<CompilerToken> ResolvePriorityTokensGroup41(List<CompilerToken> tokens, CompilationInfo info)
        {
            var resolved = new List<CompilerToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.OR:
                        operation = token.Type;
                        break;
                    default:
                        if (operation != null)
                        {
                            switch (operation)
                            {
                                case TokenType.OR:
                                    resolved[resolved.Count() - 1] = Or(resolved.Last(), token, info);
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
