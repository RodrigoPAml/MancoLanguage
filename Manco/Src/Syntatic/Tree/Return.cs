using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Enums;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida comando de return
    /// </summary>
    public class Return : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            // Return nunca pode estar no escopo global
            if (scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Invalid return in global scope", tokens[position - 1], ErrorCode.Return);

            // Return precisa sempre estar dentro de uma função
            if (!scopes.Any(x => x.Type == ScopeType.Function))
                throw new SyntaxException($"Invalid return outside of function scope", tokens[position - 1], ErrorCode.Return);

            // Return aparece sempre sozinho, caso contrario gera exceção
            if (position == tokens.Count())
                return;

            throw new SyntaxException($"Return must appear alone", tokens[position - 1], ErrorCode.Return);
        }
    }
}
