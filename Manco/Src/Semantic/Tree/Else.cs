using Language.Lexer.Entities;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Enums;

namespace Language.Semantic.Tree
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
