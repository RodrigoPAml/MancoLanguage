using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Enums;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida fechamento de escopo
    /// </summary>
    public class End : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            // End sempre sozinho
            if(position != tokens.Count())
                throw new SyntaxException($"End should appear alone", tokens[position - 1], ErrorCode.EndOfScope);

            // Caira aqui quando tiver mais fechamentos de escopo do que escopos
            if(scopes.Count() < 1)
                throw new SyntaxException($"Mismatch in ends of scope", tokens[position - 1], ErrorCode.EndOfScope);

            // Se tentar fechar o escopo global
            if(scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Closing in global scope", tokens[position - 1], ErrorCode.EndOfScope);

            scopes.Pop();
        }
    }
}
