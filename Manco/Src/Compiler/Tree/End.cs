using Language.Lexer.Entities;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila fim de escopos
    /// </summary>
    public class End : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var removed = scopes.Pop();

            // Fim de scopo de if
            if(removed.Type == ScopeType.If)
            {
                info.Lines.Add(string.Empty);
                info.Lines.Add($"-- Fim scopo limpando stack");
                info.Lines.Add($"addi sp sp {removed.StackBegin-info.StackPointer}");

                info.StackPointer += removed.StackBegin - info.StackPointer;

                info.Lines.Add($"j #ENDALL_{removed.Id}");
                info.Lines.Add($"#ENDALL_{removed.Id}:");
                info.Lines.Add($"#END_{removed.Id}:");
            }

            // Fim de scopo de else if
            if (removed.Type == ScopeType.ElseIf)
            {
                info.Lines.Add(string.Empty);
                info.Lines.Add($"-- Fim scopo limpando stack");
                info.Lines.Add($"addi sp sp {removed.StackBegin - info.StackPointer}");

                info.StackPointer += removed.StackBegin - info.StackPointer;

                var previousScope = scopes.First().LastIfId;

                info.Lines.Add($"j #ENDALL_{previousScope}");
                info.Lines.Add($"#ENDALL_{previousScope}:");
                info.Lines.Add($"#END_{removed.Id}:");
            }

            // Fim de scopo do else
            if (removed.Type == ScopeType.Else)
            {
                info.Lines.Add(string.Empty);
                info.Lines.Add($"-- Fim scopo limpando stack");
                info.Lines.Add($"addi sp sp {removed.StackBegin - info.StackPointer}");

                info.StackPointer += removed.StackBegin - info.StackPointer;

                var previousScope = scopes.First().LastIfId;
                info.Lines.Add($"#ENDALL_{previousScope}:");
            }

            // Fim de scopo do while
            if (removed.Type == ScopeType.Loop)
            {
                info.Lines.Add(string.Empty);
                info.Lines.Add($"-- Fim scopo do loop limpando stack");
                info.Lines.Add($"addi sp sp {removed.StackBegin - info.StackPointer}");
                info.StackPointer += removed.StackBegin - info.StackPointer;
                info.Lines.Add($"j #BEGIN_LOOP{removed.Id}");
                info.Lines.Add($"#END_LOOP{removed.Id}:");
            }

            // Fim de scopo de uma função
            if (removed.Type == ScopeType.Function)
            {
                info.Lines.Add(string.Empty);
                info.Lines.Add($"-- Fim scopo da função {removed.Name}");

                if(removed.Name != "main")
                {
                    info.Lines.Add($"lw ra {removed.StackBegin - info.StackPointer} sp");
                    info.Lines.Add($"addi sp sp {removed.StackBegin - info.StackPointer}");
                    info.Lines.Add($"jr ra");
                }
                else
                {
                    info.Lines.Add($"addi sp sp {removed.StackBegin - info.StackPointer}");
                    info.Lines.Add($"j end");
                }

                info.StackPointer += removed.StackBegin - info.StackPointer;
            }
        }
    }
}
