using Language.Semantic.Enums;

namespace Language.Semantic.Entities
{
    /// <summary>
    /// Scopo
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Tipo
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

        public Scope(ScopeType type)
        {
            Type = type;
        }

        public Scope(ScopeType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
