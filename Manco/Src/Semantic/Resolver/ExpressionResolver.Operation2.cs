using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Operação de soma
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken Add(SemanticToken left, SemanticToken right)
        {
            switch (left.Type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    {
                        switch(right.Type)
                        {
                            case TokenType.INTEGER_VAL:
                            case TokenType.INTEGER_VAR:
                            case TokenType.ARR_INDEX_INTEGER:
                                {
                                    var result = (int.Parse(left.TestValue) + int.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.INTEGER_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = (int.Parse(left.TestValue) + float.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    var result = (float.Parse(left.TestValue) + int.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = (float.Parse(left.TestValue) + float.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                case TokenType.STRING_VAL:
                case TokenType.STR_VAR:
                case TokenType.ARR_INDEX_STRING:
                    {
                        var result = left.TestValue + right.TestValue;

                        switch (right.Type)
                        {
                            case TokenType.STRING_VAL:
                                return new SemanticToken()
                                {
                                    TestValue = result,
                                    Content = result,
                                    Type = TokenType.STRING_VAL
                                };
                            case TokenType.STR_VAR:
                                return new SemanticToken()
                                {
                                    TestValue = result,
                                    Content = result,
                                    Type = TokenType.STRING_VAL
                                };
                            case TokenType.ARR_INDEX_STRING:
                                return new SemanticToken()
                                {
                                    TestValue = result,
                                    Content = result,
                                    Type = TokenType.STRING_VAL
                                };
                            default:
                                throw new SemanticException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação de substração
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public SemanticToken Sub(SemanticToken left, SemanticToken right)
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
                                    var result = (int.Parse(left.TestValue) - int.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.INTEGER_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = (int.Parse(left.TestValue) - float.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot sub tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    var result = (float.Parse(left.TestValue) - int.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    var result = (int.Parse(left.TestValue) - float.Parse(right.TestValue)).ToString();

                                    return new SemanticToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot sub tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot sub tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }
    }
}
