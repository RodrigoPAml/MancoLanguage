using Manco.Lexer.Entities;
using Manco.Compiler.Entities;

namespace Manco.Compiler.Base
{
    /// <summary>
    /// Estrutura base da árvore que compila o código iterativamente
    /// ao acessar nós do código (tokens)
    /// </summary>
    public abstract class CompilerTree
    {
        abstract public void Generate(int position, List<Token> tokens, Stack<Scope> scope, CompilationInfo info);
    }
}
