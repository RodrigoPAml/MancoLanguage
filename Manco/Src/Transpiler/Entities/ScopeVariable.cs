using Manco.Lexer.Enums;

namespace Manco.Transpiler.Entities
{
    /// <summary>
    /// Variavel de escopo são o que um escopo vê imediantamente abaixo dele
    /// </summary>
    public class ScopeVariable
    {
        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; } = string.Empty; 

        /// <summary>
        /// Tipo do token relacionado
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// Se variavel é array
        /// </summary>
        public bool IsArray { get; set; } = false;

        /// <summary>
        /// Se variavel foi declarada da função atual (scopo = função)
        /// </summary>
        public bool FromFunction { get; set; } = false;

        /// <summary>
        /// Usado somente em caso de função, o escopo global vê uma função, mas também vê suas variaveis
        /// </summary>
        public List<ScopeVariable> FunctionArguments { get; set; } = new List<ScopeVariable>();
    }
}
