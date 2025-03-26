using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Enums;
using Manco.Compiler.Exceptions;

namespace Manco.Compiler.Utils
{
    /// <summary>
    /// Conversão entre tipos
    /// </summary>
    public static class TypeConverter
    {
        public static VariableType ExpectedResult(TokenType type, Token token)
        {
            switch (type)
            {
                case TokenType.INTEGER_DECL:
                case TokenType.INTEGER_DECL_REF:
                    return VariableType.Integer;
                case TokenType.BOOLEAN_DECL:
                case TokenType.BOOLEAN_DECL_REF:
                    return VariableType.Boolean;
                case TokenType.DECIMAL_DECL:
                case TokenType.DECIMAL_DECL_REF:
                    return VariableType.Decimal;
                case TokenType.STRING_DECL:
                    return VariableType.String;
                default:
                    throw new CompilerException($"Failed to recognize type {type}", token, ErrorCode.Type);
            }
        }

        public static TokenType ArrayConvert(TokenType type, Token token)
        {
            switch (type)
            {
                case TokenType.INTEGER_DECL:
                case TokenType.INTEGER_DECL_REF:
                    return TokenType.ARR_INDEX_INTEGER;
                case TokenType.BOOLEAN_DECL:
                case TokenType.BOOLEAN_DECL_REF:
                    return TokenType.ARR_INDEX_BOOL;
                case TokenType.DECIMAL_DECL:
                case TokenType.DECIMAL_DECL_REF:
                    return TokenType.ARR_INDEX_DECIMAL  ;
                case TokenType.STRING_DECL:
                    return TokenType.ARR_INDEX_STRING;
                default:
                    throw new CompilerException($"Failed to recognize array type {type}", token, ErrorCode.Type);
            }
        }

        public static TokenType IdentifierConvert(TokenType type, Token token)
        {
            switch (type)
            {
                case TokenType.INTEGER_DECL:
                case TokenType.INTEGER_DECL_REF:
                    return TokenType.INTEGER_VAR;
                case TokenType.BOOLEAN_DECL:
                case TokenType.BOOLEAN_DECL_REF:
                    return TokenType.BOOL_VAR;
                case TokenType.DECIMAL_DECL:
                case TokenType.DECIMAL_DECL_REF:
                    return TokenType.DECIMAL_VAR;
                case TokenType.STRING_DECL:
                    return TokenType.STR_VAR;
                default:
                    throw new CompilerException($"Failed to recognize identifier type {type}", token, ErrorCode.Type);
            }
        }

        public static VariableType ExpressionResult(TokenType type, Token token)
        {
            switch (type)
            {
                case TokenType.INTEGER_VAL:
                case TokenType.INTEGER_VAR:
                case TokenType.ARR_INDEX_INTEGER:
                    return VariableType.Integer;
                case TokenType.DECIMAL_VAL:
                case TokenType.DECIMAL_VAR:
                case TokenType.ARR_INDEX_DECIMAL:
                    return VariableType.Decimal;
                case TokenType.STRING_VAL:
                case TokenType.STR_VAR:
                case TokenType.ARR_INDEX_STRING:
                    return VariableType.String;
                case TokenType.BOOL_VAL:
                case TokenType.BOOL_VAR:
                case TokenType.ARR_INDEX_BOOL:
                    return VariableType.Boolean;
                default:
                    throw new CompilerException($"Failed to recognize type return by expression: {type}", token, ErrorCode.Type);
            }
        }
    }
}
