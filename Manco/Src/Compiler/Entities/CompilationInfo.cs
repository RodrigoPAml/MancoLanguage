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
        /// </summary>
        public int IdCounter { get; set; } = 0;

        /// <summary>
        /// Se precisa do código para printar inteiro
        /// </summary>
        public bool UsePrintInt { get; set; } = false;

        /// <summary>
        /// Se precisa do código para printar flutuante
        /// </summary>
        public bool UsePrintFloat { get; set; } = false;

        /// <summary>
        /// Se precisa do código para printar string
        /// </summary>
        public bool UsePrintString { get; set; } = false;
    }
}
