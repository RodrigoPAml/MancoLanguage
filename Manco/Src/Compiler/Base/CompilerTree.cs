using Language.Lexer.Entities;
using Language.Compiler.Entities;

namespace Language.Compiler.Base
{
    /// <summary>
    /// Estrutura base da árvore que compila o código iterativamente
    /// ao acessar nós do código (tokens)
    /// </summary>
    public abstract class CompilerTree
    {
        abstract public void Validate(int position, List<Token> tokens, Stack<Scope> scope, CompilationInfo info);
    }
}
