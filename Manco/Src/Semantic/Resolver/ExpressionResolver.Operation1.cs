using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Operação de multiplicação
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ReducedToken Mult(ReducedToken left, ReducedToken right)
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
                                    var result = (int.Parse(left.TestValue) * int.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
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
                                    var result = (int.Parse(left.TestValue) * float.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot multiply tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    var result = (float.Parse(left.TestValue) * int.Parse(right.TestValue)).ToString();
                                    return new ReducedToken()
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
                                    var result = (int.Parse(left.TestValue) * float.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot multiply tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot multiply tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação de divisão
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ReducedToken Divide(ReducedToken left, ReducedToken right)
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
                                    var result = (int.Parse(left.TestValue) / int.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
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
                                    var result = (int.Parse(left.TestValue) / float.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot divide tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    var result = (float.Parse(left.TestValue) / int.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
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
                                    var result = (int.Parse(left.TestValue) / float.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.DECIMAL_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot divide tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot divide tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Modulo % entre dois tokens
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ReducedToken Mod(ReducedToken left, ReducedToken right)
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
                                    var result = (int.Parse(left.TestValue) % int.Parse(right.TestValue)).ToString();

                                    return new ReducedToken()
                                    {
                                        TestValue = result,
                                        Content = result,
                                        Type = TokenType.INTEGER_VAL,
                                    };
                                }
                            default:
                                throw new SemanticException($"Cannot do modulus with tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new SemanticException($"Cannot do modulus with tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }
    }
}
