using Language.Lexer.Entities;

namespace Language.Semantic.Entities
{
    public class ReducedToken : Token
    {
        public Variable? Variable { get; set; } = null;

        public bool IsArray { get; set; } = false;

        public string TestValue { get; set; } = string.Empty;
    }
}
