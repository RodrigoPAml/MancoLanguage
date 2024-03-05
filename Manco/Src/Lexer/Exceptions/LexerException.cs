using Language.Common.Enums;
using Language.Common.Exceptions;
using Language.Lexer.Entities;

namespace Language.Lexer.Exceptions
{
    /// <summary>
    /// Exceção de Lexer
    /// </summary>
    public class LexerException : BaseException
    {
        public LexerException(string message, Token token, ErrorCode code) : base(message, token, code)
        {
        }
    }
}

