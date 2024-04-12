using Language.Common.Enums;
using Language.Common.Exceptions;
using Language.Lexer.Entities;

namespace Language.Compiler.Exceptions
{
    /// <summary>
    /// Exceção semantica
    /// </summary>
    public class CompilerException : BaseException
    {
        public CompilerException(string message, Token? token, ErrorCode? code) : base(message, token, code)
        {
        }
    }
}
