using Language.Lexer.Enums;
using Language.Syntatic.Enums;

namespace Language.Syntatic.Entities
{
    /// <summary>
    /// Representa o scopo, tambem guarda commandos que pertence a um scopo
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Tipo do escopo
        /// </summary>
        public ScopeType Type { get; set; }

        /// <summary>
        /// Comandos do scopo
        /// </summary>
        public List<TokenType> Commands { get; set; } = new List<TokenType>();

        public Scope(ScopeType type)
        {
            Type = type;
        }
    }
}
