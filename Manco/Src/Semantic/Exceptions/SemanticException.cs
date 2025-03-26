using Manco.Common.Enums;
using Manco.Common.Exceptions;
using Manco.Lexer.Entities;

namespace Manco.Semantic.Exceptions
{
    /// <summary>
    /// Exceção semantica
    /// </summary>
    public class SemanticException : BaseException
    {
        public SemanticException(string message, Token token, ErrorCode? code) : base(message, token, code)
        {
        }
    }
}
