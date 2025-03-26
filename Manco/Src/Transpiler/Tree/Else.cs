using Manco.Lexer.Entities;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Enums;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila código para else
    /// </summary>
    public class Else : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            info.AddLine("else");
            info.AddLine("{");
            info.IncreaseIdentation();

            scopes.Push(new Scope()
            {
                Name = "",
                Type = ScopeType.Else,
            });
        }
    }
}
