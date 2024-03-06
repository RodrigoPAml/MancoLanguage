using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;
using Language.Semantic.Tree;

namespace Language.Semantic
{
    public class SemanticChecker
    {
        public void Parse(List<List<Token>> tokens)
        {
            Scope global = new Scope(ScopeType.Global);

            Stack<Scope> scopes = new Stack<Scope>();    
            scopes.Push(global);

            // Adiciona função print
            scopes.First().Variables.Add(new Variable()
            {
                Name = "print",
                Type = TokenType.FUNCTION,
                ChildVariables = new List<Variable>()
                {
                    new Variable()
                    {
                        Name= "any",
                        Type = TokenType.ANY 
                    }
                }
            });

            foreach (var line in tokens)
            {
                new Root().Validate(0, line, scopes);
            }

            if (!scopes.SelectMany(x => x.Variables).Any(x => x.Name == "main" && x.Type == TokenType.FUNCTION))
                throw new SemanticException($"No main founded", null, null);

            if (scopes.SelectMany(x => x.Variables).Any(x => x.Name == "main" && x.Type == TokenType.FUNCTION && x.ChildVariables.Any()))
                throw new SemanticException($"The main function can't have any arguments", null, null);
        }
    }
}
