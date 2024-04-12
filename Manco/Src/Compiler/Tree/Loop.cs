using Language.Lexer.Entities;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila while loop
    /// </summary>
    public class Loop : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Calculo de instrução de scopo loop");
         
            var name = string.Join(' ', tokens.Select(x => x.Content).ToArray());
            scopes.Push(new Scope(info.IdCounter++, ScopeType.Loop, name, info.StackPointer));

            var currentScope = scopes.First();

            info.Lines.Add($"#BEGIN_LOOP{currentScope.Id}:");

            var expr = new Expression();
            expr.Validate(position, tokens, scopes, info);
 
            info.Lines.Add($"beq t0 one #BODY_LOOP{currentScope.Id}");
            info.Lines.Add($"addi sp sp {currentScope.StackBegin - info.StackPointer}");
            info.Lines.Add($"j #END_LOOP{currentScope.Id}");

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Instrução loop {name} com id {currentScope.Id} e stack inicial {info.StackPointer}");
            info.Lines.Add($"#BODY_LOOP{currentScope.Id}:");
        }
    }
}
