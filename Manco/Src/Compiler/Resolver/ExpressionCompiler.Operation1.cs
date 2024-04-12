using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Compiler.Entities;
using Language.Compiler.Exceptions;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Operação de multiplicação
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken Mult(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                    return IntegerOp(info, left, right, "mul", TokenType.INTEGER_VAL);
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return IntegerDecimalOp(info, left, right, "mulf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot multiply tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    return DecimalIntegerOp(info, left, right, "mulf", TokenType.DECIMAL_VAL);
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return DecimalOp(info, left, right, "mulf", TokenType.DECIMAL_VAL);

                                }
                            default:
                                throw new CompilerException($"Cannot multiply tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot multiply tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação de divisão
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken Divide(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                    return DivOp(info, left, right, TokenType.DECIMAL_VAL);
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return IntegerDecimalOp(info, left, right, "divf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot divide tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
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
                                    return DecimalIntegerOp(info, left, right, "divf", TokenType.DECIMAL_VAL);
                                }
                            case TokenType.DECIMAL_VAL:
                            case TokenType.DECIMAL_VAR:
                            case TokenType.ARR_INDEX_DECIMAL:
                                {
                                    return DecimalOp(info, left, right, "divf", TokenType.DECIMAL_VAL);
                                }
                            default:
                                throw new CompilerException($"Cannot divide tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot divide tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Modulo % entre dois tokens
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken Mod(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                    return IntegerMod(info, left, right);
                                }
                            default:
                                throw new CompilerException($"Cannot do modulus with tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot do modulus with tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }
    }
}
