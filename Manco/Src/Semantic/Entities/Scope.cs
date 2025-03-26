using Manco.Semantic.Enums;

namespace Manco.Semantic.Entities
{
    /// <summary>
    /// Scopo
    /// Um scopo não contêm outro scopo, apesar de poder estar dentro de um
    /// Stack determina isso
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
        /// Filhos do Scopo
        /// </summary>
        public List<ScopeVariable> Childrens { get; set; } = new List<ScopeVariable>();

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
