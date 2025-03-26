using Manco.Transpiler.Enums;

namespace Manco.Transpiler.Entities
{
    /// <summary>
    /// Scopo
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Tipo do scopo
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
    }
}
