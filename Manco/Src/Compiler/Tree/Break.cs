using Manco.Lexer.Entities;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Compila break de loops
    /// </summary>
    public class Break : CompilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var lastWhileScope = scopes.ToList().First(x => x.Type == ScopeType.Loop);

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Fim scopo por break no loop limpando stack");
            info.Lines.Add($"addi sp sp {lastWhileScope.StackBegin - info.StackPointer}");
            info.Lines.Add($"j #END_LOOP{lastWhileScope.Id}");
        }
    }
}
