using Language.Lexer.Entities;

namespace Language.Semantic.Entities
{
    /// <summary>
    /// Token formado por um ou mais tokens para simplificação da fase semantica
    /// </summary>
    public class SemanticToken : Token
    {
        /// <summary>
        /// Variavel relacionada a token
        /// </summary>
        public Variable? Variable { get; set; } = null;

        /// <summary>
        /// Se é array
        /// </summary>
        public bool IsArray { get; set; } = false;

        /// <summary>
        /// Valor de teste para validação de expressão
        /// </summary>
        public string TestValue { get; set; } = string.Empty;
    }
}
