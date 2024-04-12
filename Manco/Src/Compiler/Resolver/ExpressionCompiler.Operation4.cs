using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Compiler.Entities;
using Language.Compiler.Exceptions;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Operação AND GRUPO 4
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken And(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                return AndOp(info, left, right);
                            default:
                                throw new CompilerException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }

        /// <summary>
        /// Operação OR GRUPO 4
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private CompilerToken Or(CompilerToken left, CompilerToken right, CompilationInfo info)
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
                                return OrOp(info, left, right);
                            default:
                                throw new CompilerException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
                        }
                    }
                default:
                    throw new CompilerException($"Cannot compare tokens of types {left.Type} and {right.Type}", left, ErrorCode.Expression);
            }
        }
    }
}
