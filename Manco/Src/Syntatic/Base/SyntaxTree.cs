using Language.Lexer.Entities;
using Language.Syntatic.Entities;

namespace Language.Syntatic.Base
{
    /// <summary>
    /// Estrutura base da árvore que valida a sintaxe do código
    /// </summary>
    public abstract class SyntaxTree
    {
        abstract public void Validate(int position, List<Token> tokens, Stack<Scope> scope);
    }
}
