using System.Text;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;

namespace Manco.Transpiler.Utils
{
    public static class Conversions
    {
        public static string ToType(TokenType type)
        {
            switch(type)
            {
                case TokenType.INTEGER_DECL:
                    return "int";
                case TokenType.DECIMAL_DECL:
                    return "float";
                case TokenType.BOOLEAN_DECL:
                    return "bool";
                case TokenType.STRING_DECL:
                    return "std::string";
                default:
                    return "?";
            }
        }

        public static string DefaultValue(TokenType type)
        {
            switch (type)
            {
                case TokenType.INTEGER_DECL:
                    return "0";
                case TokenType.DECIMAL_DECL:
                    return "0.0";
                case TokenType.BOOLEAN_DECL:
                    return "false";
                case TokenType.STRING_DECL:
                    return "\"\"";
                default:
                    return "?";
            }
        }

        public static string BuildExpression(List<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.AND:
                        result.Append("&& ");
                        break;
                    case TokenType.OR:
                        result.Append("|| ");
                        break;
                    case TokenType.STRING_VAL:
                        {
                           result.Append(token.Content.Replace("\n", "\\n") + " ");
                        }
                        break;
                    default:
                        result.Append(token.Content + " ");
                        break;
                }
            }

            return result.ToString().TrimEnd();
        }
    }
}
