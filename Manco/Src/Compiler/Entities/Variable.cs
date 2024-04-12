using Language.Lexer.Enums;

namespace Language.Compiler.Entities
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
        /// Se é uma função aqui esta seus argumentos
        /// </summary>
        public List<Variable> ChildVariables { get; set; } = new List<Variable>();
    }
}
