using Manco.Lexer.Entities;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;

namespace Manco.Semantic.Tree
{
    /// <summary>
    /// Valida else
    /// </summary>
    public class Else : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            scopes.Push(new Scope(ScopeType.Else));
        }
    }
}
