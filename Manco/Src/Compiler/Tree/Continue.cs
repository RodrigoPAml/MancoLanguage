using Language.Lexer.Entities;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila continue de loops
    /// </summary>
    public class Continue : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var lastWhileScope = scopes.ToList().First(x => x.Type == ScopeType.Loop);

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Fim scopo por continue no loop limpando stack");
            info.Lines.Add($"addi sp sp {lastWhileScope.StackBegin - info.StackPointer}");
            info.Lines.Add($"j #BEGIN_LOOP{lastWhileScope.Id}");
        }
    }
}
