using Language.Lexer.Entities;
using Language.Semantic.Entities;

namespace Language.Semantic.Base
{
    public abstract class SemanticTree
    {
        abstract public void Validate(int position, List<Token> tokens, Stack<Scope> scope);
    }
}
