﻿using Manco.Common.Enums;
using Manco.Lexer.Enums;
using Manco.Semantic.Entities;
using Manco.Semantic.Exceptions;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Operação AND GRUPO 4
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken And(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.BOOL_VAL:
                case TokenType.BOOL_VAR:
                case TokenType.ARR_INDEX_BOOL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.BOOL_VAL:
                            case TokenType.BOOL_VAR:
                            case TokenType.ARR_INDEX_BOOL:

                                var result = left.TestValue == "true" && right.TestValue == "true"
                                        ? "true"
                                        : "false";

                                return new SemanticToken()
                                {
                                    Content = result,
                                    TestValue = result,
                                    Type = TokenType.BOOL_VAL,    
                                };
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação OR GRUPO 4
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken Or(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.BOOL_VAL:
                case TokenType.BOOL_VAR:
                case TokenType.ARR_INDEX_BOOL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.BOOL_VAL:
                            case TokenType.BOOL_VAR:
                            case TokenType.ARR_INDEX_BOOL:
                                var result = left.TestValue == "true" || right.TestValue == "true"
                                        ? "true"
                                        : "false";

                                return new SemanticToken()
                                {
                                    Content = result,
                                    TestValue = result,
                                    Type = TokenType.BOOL_VAL,
                                };
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }
    }
}
