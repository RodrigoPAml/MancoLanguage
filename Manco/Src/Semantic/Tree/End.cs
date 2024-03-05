using Language.Lexer.Entities;
using Language.Semantic.Base;
using Language.Semantic.Entities;

namespace Language.Semantic.Tree
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
