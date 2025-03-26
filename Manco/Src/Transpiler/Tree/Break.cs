using Manco.Lexer.Entities;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila break de loops
    /// </summary>
    public class Break : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            info.AddLine($"break;");
        }
    }
}
