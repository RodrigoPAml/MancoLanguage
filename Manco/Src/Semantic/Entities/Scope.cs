using Language.Semantic.Enums;

namespace Language.Semantic.Entities
{
    public class Scope
    {
        /// <summary>
        /// Scope Type
        /// </summary>
        public ScopeType Type { get; set; }

        /// <summary>
        /// Scope Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Variables of this scope
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
