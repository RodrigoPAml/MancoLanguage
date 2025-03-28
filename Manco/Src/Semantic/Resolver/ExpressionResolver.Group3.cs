﻿using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Entities;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve tokens > < >= <= == != GRUPO 3
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<SemanticToken> ResolvePriorityTokensGroup3(List<SemanticToken> tokens)
        {
            var resolved = new List<SemanticToken>();
            TokenType? operation = null;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Operators
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
                                    resolved[resolved.Count() - 1] = Greater(resolved.Last(), token);
                                    break;
                                case TokenType.GREATER_EQ:
                                    resolved[resolved.Count() - 1] = GreaterEqual(resolved.Last(), token);
                                    break;
                                case TokenType.LESS:
                                    resolved[resolved.Count() - 1] = Less(resolved.Last(), token);
                                    break;
                                case TokenType.LESS_EQ:
                                    resolved[resolved.Count() - 1] = LessEqual(resolved.Last(), token);
                                    break;
                                case TokenType.EQUALS:
                                    resolved[resolved.Count() - 1] = Equals(resolved.Last(), token);
                                    break;
                                case TokenType.NOT_EQUAL:
                                    resolved[resolved.Count() - 1] = NotEquals(resolved.Last(), token);
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
