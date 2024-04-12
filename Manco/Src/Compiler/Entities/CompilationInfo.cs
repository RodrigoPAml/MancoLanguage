namespace Language.Compiler.Entities
{
    /// <summary>
    /// Informações para compilação
    /// </summary>
    public class CompilationInfo 
    {
        /// <summary>
        /// Linhas compiladas
        /// </summary>
        public List<string> Lines = new List<string>();

        /// <summary>
        /// Guarda a pilha atual, normalmente só para o escopo da função
        /// </summary>
        public int StackPointer { get; set; } = 0;

        /// <summary>
        /// Counter pra labels
        public int IdCounter { get; set; } = 0;
    }
}
