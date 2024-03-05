using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
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
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.EndOfScope);

            // Caira aqui quando tiver mais fechamentos de escopo do que escopos
            if(scopes.Count() < 1)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.EndOfScope);

            // Se tentar fechar o escopo global
            if(scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Invalid token {tokens[position - 1]} in global scope", tokens[position - 1], ErrorCode.EndOfScope);

            scopes.Pop();
        }
    }
}
