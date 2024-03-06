using Language.Lexer.Enums;

namespace Language.Semantic.Entities
{
    /// <summary>
    /// Variavel declarada
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Nome da Variavel
        /// </summary>
        public string Name { get; set; } = string.Empty; 

        /// <summary>
        /// Tipo do token relacionado a variavel
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// Se variavel é array
        /// </summary>
        public bool IsArray { get; set; } = false;

        /// <summary>
        /// Se variavel veio da função atual
        /// </summary>
        public bool FromFunction { get; set; } = false;

        /// <summary>
        /// Se é uma função aqui esta seus argumentos
        /// </summary>
        public List<Variable> ChildVariables { get; set; } = new List<Variable>();
    }
}
