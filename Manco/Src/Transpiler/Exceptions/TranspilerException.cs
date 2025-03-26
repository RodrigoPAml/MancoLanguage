using Manco.Common.Enums;
using Manco.Common.Exceptions;
using Manco.Lexer.Entities;

namespace Manco.Transpiler.Exceptions
{
    /// <summary>
    /// Exceção de transpilação
    /// </summary>
    public class TranspilerException : BaseException
    {
        public TranspilerException(string message, Token token, ErrorCode code) : base(message, token, code)
        {
        }
    }
}
