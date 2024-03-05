using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;
using Language.Semantic.Tree;

// Print especial function allowance
// Review all semantic, map errors

namespace Language.Semantic
{
    public class SemanticChecker
    {
        public void Parse(List<List<Token>> tokens)
        {
            Scope global = new Scope(ScopeType.Global);

            Stack<Scope> scopes = new Stack<Scope>();    
            scopes.Push(global);

            foreach (var line in tokens)
            {
                new Root().Validate(0, line, scopes);
            }

            if (!scopes.SelectMany(x => x.Variables).Any(x => x.Name == "main" && x.Type == TokenType.FUNCTION))
                throw new SemanticException($"No main founded", null);
        }
    }
}
