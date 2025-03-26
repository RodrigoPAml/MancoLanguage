using Manco.Compiler.Enums;

namespace Manco.Compiler.Entities
{
    /// <summary>
    /// Scopo
    /// Um scopo não contêm outro scopo, apesar de poder estar dentro de um
    /// Stack determina isso
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Id do scopo
        /// </summary>
        public int Id { get; private set; }

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

        /// <summary>
        /// Ultimo Id de if/else/elif desse scopo
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
