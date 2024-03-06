using Language.Common.Enums;
using Language.Common.Exceptions;
using Language.Lexer.Entities;

namespace Language.Semantic.Exceptions
{
    /// <summary>
    /// Exceção semantica
    /// </summary>
    public class SemanticException : BaseException
    {
        public SemanticException(string message, Token? token, ErrorCode? code) : base(message, token, code)
        {
        }
    }
}
