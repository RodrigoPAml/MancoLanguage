using Language.Lexer.Entities;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila código para else
    /// </summary>
    public class Else : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var fatherScope = scopes.First();

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Instrução senão com id {info.IdCounter}");
            scopes.Push(new Scope(info.IdCounter++, ScopeType.Else, string.Empty, info.StackPointer));

            var currentScope = scopes.First();
            var previousScope = fatherScope.LastIfId;

            // ja que o fim do grupo é aqui, removemos do anterior
            info.Lines.Remove($"#ENDALL_{previousScope}:");
            info.Lines.Add($"#ELSE{currentScope.Id}:");
        }
    }
}
