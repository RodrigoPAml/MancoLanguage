using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
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
                throw new SyntaxException($"Invalid token {tokens[position-1]} in this scope", tokens[position - 1], ErrorCode.Return);

            // Return precisa sempre estar dentro de uma função
            if (!scopes.Any(x => x.Type == ScopeType.Function))
                throw new SyntaxException($"Invalid token {tokens[position - 1]} outside of function scope", tokens[position - 1], ErrorCode.Return);

            // Return aparece sempre sozinho, caso contrario gera exceção
            if (position == tokens.Count())
                return;

            throw new SyntaxException($"Invalid token {tokens[position-1]}", tokens[position - 1], ErrorCode.Return);
        }
    }
}
