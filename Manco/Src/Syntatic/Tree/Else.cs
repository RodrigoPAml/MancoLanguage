using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
{
    /// <summary>
    /// Valida sintaxe do else
    /// </summary>
    public class Else : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if(position != tokens.Count())
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.Else);

            // Não pode ser escopo global
            if (scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Invalid token {tokens[position]} at global scope", tokens[position], ErrorCode.Else);

            var scopeCommandsSize = scopes.First().Commands.Count();

            if (scopeCommandsSize < 2)
                throw new SyntaxException($"Invalid else token {tokens[position - 1]}", tokens[position - 1], ErrorCode.Else);

            // Verifica se existe um if ou else if anteriormente
            if(scopes.First().Commands[scopeCommandsSize - 2] != TokenType.IF &&
               scopes.First().Commands[scopeCommandsSize - 2] != TokenType.ELSEIF)
            {
                throw new SyntaxException($"Invalid else without if/elif at {tokens[position - 1]}", tokens[position - 1], ErrorCode.Else);
            }

            scopes.Push(new Scope(ScopeType.Else));
        }
    }
}
