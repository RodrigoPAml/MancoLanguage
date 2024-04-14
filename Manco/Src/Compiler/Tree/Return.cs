using Language.Lexer.Entities;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila retorno de função
    /// </summary>
    public class Return : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var function = scopes.ToList().FindLast(x => x.Type == ScopeType.Function);

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Fim do scopo por Return");

            if (function?.Name != "main")
            {
                info.Lines.Add($"lw ra {function?.StackBegin - info.StackPointer} sp");
                info.Lines.Add($"addi sp sp {function?.StackBegin - info.StackPointer}");
                info.Lines.Add($"jr ra");
            }
            else
            {
                info.Lines.Add($"addi sp sp {function?.StackBegin - info.StackPointer}");
                info.Lines.Add($"j end");
            }

            info.StackPointer = 0;
        }
    }
}
