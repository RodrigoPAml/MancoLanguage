using Manco.Common.Enums;
using Manco.Lexer.Enums;
using Manco.Semantic.Entities;
using Manco.Semantic.Exceptions;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Operação > GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken Greater(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = int.Parse(left.TestValue) > int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = int.Parse(left.TestValue) > float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = float.Parse(left.TestValue) > int.Parse(right.TestValue)
                                       ? "true"
                                       : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = float.Parse(left.TestValue) > float.Parse(right.TestValue)
                                       ? "true"
                                       : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operaçãp >= GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken GreaterEqual(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = int.Parse(left.TestValue) >= int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = int.Parse(left.TestValue) >= float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = float.Parse(left.TestValue) >= int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = float.Parse(left.TestValue) >= float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operations < GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken Less(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = int.Parse(left.TestValue) < int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = int.Parse(left.TestValue) < float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = float.Parse(left.TestValue) < int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = float.Parse(left.TestValue) < float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação <= GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken LessEqual(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = int.Parse(left.TestValue) <= int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = int.Parse(left.TestValue) <= float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = float.Parse(left.TestValue) <= int.Parse(right.TestValue)
                                       ? "true"
                                       : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = float.Parse(left.TestValue) <= float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação == GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken Equals(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = int.Parse(left.TestValue) == int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = int.Parse(left.TestValue) == float.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = float.Parse(left.TestValue) == int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = float.Parse(left.TestValue) == float.Parse(right.TestValue)
                                       ? "true"
                                       : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.STRING_VAL:
                case TokenType.STR_VAR:
                case TokenType.ARR_INDEX_STRING:
                    {
                        switch (right.Type)
                        {
                            case TokenType.STRING_VAL:
                            case TokenType.STR_VAR:
                            case TokenType.ARR_INDEX_STRING:
                                {
                                    var result = left.TestValue == right.TestValue
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação != GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken NotEquals(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = int.Parse(left.TestValue) != int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = int.Parse(left.TestValue) != float.Parse(right.TestValue)
                                       ? "true"
                                       : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    {
                        switch (right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = float.Parse(left.TestValue) != int.Parse(right.TestValue)
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = float.Parse(left.TestValue) != float.Parse(right.TestValue)
                                       ? "true"
                                       : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.STRING_VAL:
                case TokenType.STR_VAR:
                case TokenType.ARR_INDEX_STRING:
                    {
                        switch (right.Type)
                        {
                            case TokenType.STRING_VAL:
                            case TokenType.STR_VAR:
                            case TokenType.ARR_INDEX_STRING:
                                {
                                    var result = left.TestValue != right.TestValue
                                        ? "true"
                                        : "false";

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.BOOL_VAL,
                                    };
                                }
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