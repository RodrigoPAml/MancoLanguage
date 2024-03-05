using Language.Lexer.Entities;

namespace Language.Semantic.Exceptions
{
    public class SemanticException : Exception
    {
        public Token Token { get; set; }

        public SemanticException(string message, Token token) : base(message)
        {
            Token = token;
        }
    }
}
