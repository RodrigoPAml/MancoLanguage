using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;
using Language.Syntatic.Tree;

namespace Language.Syntatic
{
    /// <summary>
    /// Verifica a sintaxe do código
    /// </summary>
    public class SyntaxChecker
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

            if (scopes.Count != 1)
                throw new SyntaxException("Mismatch of scopes", tokens.Last().Last(), ErrorCode.EndOfScope);
        }
    }
}
