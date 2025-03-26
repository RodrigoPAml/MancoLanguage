using Manco.Common.Enums;
using Manco.Common.Exceptions;
using Manco.Lexer.Entities;

namespace Manco.Syntatic.Exceptions
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
