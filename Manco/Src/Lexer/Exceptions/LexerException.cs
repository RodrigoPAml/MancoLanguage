using Manco.Common.Enums;
using Manco.Common.Exceptions;
using Manco.Lexer.Entities;

namespace Manco.Lexer.Exceptions
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

