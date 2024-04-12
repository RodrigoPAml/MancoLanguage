using Language.Compiler.Enums;

namespace Language.Compiler.Entities
{
    /// <summary>
    /// Scopo
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Compiler
        /// </summary>
        public ScopeType Type { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Variaveis do Scopo
        /// </summary>
        public List<Variable> Variables { get; set; } = new List<Variable>();

        /// <summary>
        /// Id do scopo
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Ultimo Id de if desse scopo
        /// </summary>
        public int LastIfId { get; set; } = 0;

        /// <summary>
        /// Onde é o começo da stack do scopo
        /// </summary>
        public int StackBegin { get; set; } = 0;

        public Scope(int id, ScopeType type)
        {
            Id = id;
            Type = type;
        }

        public Scope(int id, ScopeType type, string name, int stackBegin)
        {
            Id = id;
            Type = type;
            Name = name;
            StackBegin = stackBegin;
        }
    }
}
