using Manco.Lexer.Entities;
using Manco.Transpiler.Entities;

namespace Manco.Transpiler.Base
{
    /// <summary>
    /// Estrutura base da árvore que transpila o código iterativamente
    /// ao acessar nós do código (tokens)
    /// </summary>
    public abstract class TranspilerTree
    {
        abstract public void Generate(int position, List<Token> tokens, Stack<Scope> scope, TranspilationInfo info);
    }
}
