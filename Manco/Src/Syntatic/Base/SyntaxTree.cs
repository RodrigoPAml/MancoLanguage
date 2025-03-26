using Manco.Lexer.Entities;
using Manco.Syntatic.Entities;

namespace Manco.Syntatic.Base
{
    /// <summary>
    /// Estrutura base da árvore que valida a sintaxe do código iterativamente por
    /// cada token
    /// </summary>
    public abstract class SyntaxTree
    {
        abstract public void Validate(int position, List<Token> tokens, Stack<Scope> scope);
    }
}
