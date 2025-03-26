using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Enums;
using Manco.Syntatic.Exceptions;
using Manco.Syntatic.Tree;

namespace Manco.Syntatic
{
    /// <summary>
    /// Verifica a sintaxe do código
    /// </summary>
    public class SyntaxChecker
    {
        public void Parse(List<List<Token>> tokens)
        {
            var global = new Scope(ScopeType.Global);

            var scopes = new Stack<Scope>();    
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
