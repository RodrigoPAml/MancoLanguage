using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
{
    /// <summary>
    /// Valida comando de continue
    /// </summary>
    public class Continue : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            // Continue nunca pode estar no escopo global
            if (scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Invalid continue in global scope", tokens[position - 1], ErrorCode.Continue);

            // Continue precisa sempre estar dentro de um loop
            if (!scopes.Any(x => x.Type == ScopeType.Loop))
                throw new SyntaxException($"Invalid continue outside of loop scope", tokens[position - 1], ErrorCode.Continue);

            // Continue aparece sempre sozinho, caso contrario gera exceção
            if (position == tokens.Count())
                return;

            throw new SyntaxException($"Continue must appear alone", tokens[position - 1], ErrorCode.Continue);
        }
    }
}
