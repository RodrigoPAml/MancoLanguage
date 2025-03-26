using Manco.Lexer.Enums;

namespace Manco.Compiler.Entities
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
        /// Posição da variavel na stack do scopo atual
        /// </summary>
        public int StackPos { get; set; } = -1;

        /// <summary>
        /// Tamanho da variavel em bytes na stack
        /// </summary>
        public int Size { get; set; } = 0;

        /// <summary>
        /// Quando for mudar a variavel por referencia precisamos do endereço dinamico, e não relativo a stack
        /// </summary>
        public int AddressStackPos { get; set; } = -1;

        /// <summary>
        /// Normalmente uma variavel de escopo não pode ter filhos
        /// Mas em caso do escopo global, ele possui uma função como filho, então aqui é guardados os argumentos da função
        /// </summary>
        public List<ScopeVariable> FunctionArguments { get; set; } = new List<ScopeVariable>();
    }
}
