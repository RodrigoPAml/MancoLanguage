using Manco.Lexer.Entities;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;

namespace Manco.Semantic.Tree
{
    /// <summary>
    /// Valida fim de escopo
    /// </summary>
    public class End : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            scopes.Pop();
        }
    }
}
