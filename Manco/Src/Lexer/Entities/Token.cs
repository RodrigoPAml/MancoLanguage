using Language.Lexer.Enums;

namespace Language.Lexer.Entities
{
    /// <summary>
    /// Representa um token
    /// </summary>
    public class Token
    {
        public string Content { get; set; }

        public TokenType Type { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Line { get; set; }

        public int Size => End - Start;

        /// <summary>
        /// Se durante o parser havia mais algum outro token concatenado ao lado
        /// </summary>
        public bool MoreOnLeft { get; set; }

        public override string ToString()
        {
            return $"{Type}({Content}):{Line}-{Start}-{End}";
        }
    }
}
