using Language.Common.Enums;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Operação AND GRUPO 4
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ReducedToken And(ReducedToken left, ReducedToken right)
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

                                return new ReducedToken()
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
        public ReducedToken Or(ReducedToken left, ReducedToken right)
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

                                return new ReducedToken()
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
