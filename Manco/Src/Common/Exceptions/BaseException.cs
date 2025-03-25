using Language.Common.Enums;
using Language.Lexer.Entities;

namespace Language.Common.Exceptions
{
    /// <summary>
    /// Base language exception
    /// </summary>
    public class BaseException : Exception
    {
        public Token Token { get; set; }

        public ErrorCode? Code { get; set; }

        public BaseException(string message, Token token, ErrorCode? code) : base(message)
        {
            Token = token;
            Code = code;
        }
    }
}
