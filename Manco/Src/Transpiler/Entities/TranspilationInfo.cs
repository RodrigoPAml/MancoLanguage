namespace Manco.Transpiler.Entities
{
    /// <summary>
    /// Informações para compilação
    /// </summary>
    public class TranspilationInfo 
    {
        /// <summary>
        /// Linhas compiladas
        /// </summary>
        private List<string> Lines = new();

        /// <summary>
        /// Current identation
        /// </summary>
        public string Identation = string.Empty;

        /// <summary>
        /// Se já contabizou o tempo de execução no final da main
        /// </summary>
        public bool HaveTimerSet = false;

        public void AddLine(string line)
        {
            Lines.Add(Identation + line);
        }

        public void IncreaseIdentation()
        {
            Identation += "    ";
        }

        public void DecreaseIdentation()
        {
            Identation = Identation.Substring(4);
        }   

        public List<string> GetLines()
        {
            return Lines;
        }
    }
}
