using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Compiler.Entities;
using Language.Compiler.Exceptions;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Operação de soma
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken Add(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                    return IntegerOp(info, left, right, "add", TokenType.INTEGER_VAL);
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return IntegerDecimalOp(info, left, right, "addf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    return DecimalIntegerOp(info, left, right, "addf", TokenType.DECIMAL_VAL);

                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return DecimalOp(info, left, right, "addf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    return StringSum(info, left, right);
                                }
                            default:
                                throw new CompilerException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot add tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação de substração
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken Sub(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                    return IntegerOp(info, left, right, "sub", TokenType.INTEGER_VAL);
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return IntegerDecimalOp(info, left, right, "subf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot sub tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    return DecimalIntegerOp(info, left, right, "subf", TokenType.DECIMAL_VAL);

                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return DecimalOp(info, left, right, "subf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot sub tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot sub tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }
    }
}
