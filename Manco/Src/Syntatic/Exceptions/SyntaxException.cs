using Language.Common.Enums;
using Language.Common.Exceptions;
using Language.Lexer.Entities;

namespace Language.Syntatic.Exceptions
{
    /// <summary>
    /// Exceção de Sintaxe
    /// </summary>
    public class SyntaxException : BaseException
    {
        public SyntaxException(string message, Token token, ErrorCode code) : base(message, token, code)
        {
        }
    }
}
