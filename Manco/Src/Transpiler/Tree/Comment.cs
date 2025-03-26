using Manco.Lexer.Entities;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila código de comentário
    /// </summary>
    public class Comment : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            info.AddLine($"// {string.Join(" ", tokens.Select(x => x.Content))}");
        }
    }
}
