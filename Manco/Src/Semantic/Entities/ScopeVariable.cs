using Manco.Lexer.Enums;

namespace Manco.Semantic.Entities
{
    /// <summary>
    /// Variavel do escopo é o que um escopo vê imediatamente abaixo do seu nível
    /// Por exemplo o escopo global vê declarações de funções
    /// Os escopos de função veem variáveis
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
        /// Normalmente uma variavel de escopo não pode ter filhos
        /// Mas em caso do escopo global, ele possui uma função como filho, então aqui é guardados os argumentos da função
        /// </summary>
        public List<ScopeVariable> FunctionArguments { get; set; } = new List<ScopeVariable>();
    }
}
