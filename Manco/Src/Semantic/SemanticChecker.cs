using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;
using Manco.Semantic.Exceptions;
using Manco.Semantic.Tree;

namespace Manco.Semantic
{
    /// <summary>
    /// Validador semantico
    /// </summary>
    public class SemanticChecker
    {
        public void Parse(List<List<Token>> tokens)
        {
            var global = new Scope(ScopeType.Global);

            var scopes = new Stack<Scope>();    
            scopes.Push(global);

            // Adiciona função print
            scopes.First().Childrens.Add(new ScopeVariable()
            {
                Name = "print",
                Type = TokenType.FUNCTION,
                FunctionArguments = new List<ScopeVariable>()
                {
                    new ScopeVariable()
                    {
                        Name= "any",
                        Type = TokenType.ANY 
                    }
                }
            });

            foreach (var line in tokens)
                new Root().Validate(0, line, scopes);

            if (!scopes.SelectMany(x => x.Childrens).Any(x => x.Name == "main" && x.Type == TokenType.FUNCTION))
                throw new SemanticException($"No main founded", null, null);

            if (scopes.SelectMany(x => x.Childrens).Any(x => x.Name == "main" && x.Type == TokenType.FUNCTION && x.FunctionArguments.Any()))
                throw new SemanticException($"The main function can't have any arguments", null, null);
        }
    }
}
