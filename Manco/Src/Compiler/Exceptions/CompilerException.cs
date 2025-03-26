using Manco.Common.Enums;
using Manco.Common.Exceptions;
using Manco.Lexer.Entities;

namespace Manco.Compiler.Exceptions
{
    /// <summary>
    /// Exceção semantica
    /// </summary>
    public class CompilerException : BaseException
    {
        public CompilerException(string message, Token token, ErrorCode code) : base(message, token, code)
        {
        }
    }
}
