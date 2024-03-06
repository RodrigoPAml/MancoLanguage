using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Operação > GRUPO 3
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ReducedToken Greater(ReducedToken left, ReducedToken right)
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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
        public ReducedToken GreaterEqual(ReducedToken left, ReducedToken right)
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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
        public ReducedToken Less(ReducedToken left, ReducedToken right)
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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
        public ReducedToken LessEqual(ReducedToken left, ReducedToken right)
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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
        public ReducedToken Equals(ReducedToken left, ReducedToken right)
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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
        public ReducedToken NotEquals(ReducedToken left, ReducedToken right)
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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

                                    return new ReducedToken()
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