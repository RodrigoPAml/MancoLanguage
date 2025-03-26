using Manco.Lexer.Entities;

namespace Manco.Compiler.Entities
{
    /// <summary>
    /// Token formado por um ou mais tokens para simplificação da compilação
    /// </summary>
    public class CompilerToken : Token
    {
        /// <summary>
        /// Variavel relacionada a token
        /// </summary>
        public ScopeVariable Variable { get; set; } = null;

        /// <summary>
        /// Se é array
        /// </summary>
        public bool IsArray { get; set; } = false;

        /// <summary>
        /// Onde na stack essa variavel esta guardada
        /// </summary>
        public int StackPos = -1;

        /// <summary>
        /// Onde na stack essa variavel esta guardada (endereco guardado)
        /// </summary>
        public int StackRegisterMemory = -1;

        /// <summary>
        /// Variavel de indice, usualmente usado para acesso por indice de array na stack
        /// </summary>
        public ScopeVariable IndexVariable = null;

        /// <summary>
        /// Tamamho da stack que ocupa
        /// </summary>
        public int StackSize = -1;
    }
}
