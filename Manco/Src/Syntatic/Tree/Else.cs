﻿using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Enums;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida sintaxe do else
    /// </summary>
    public class Else : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if(position != tokens.Count())
                throw new SyntaxException($"Else cant have content", tokens[position - 1], ErrorCode.Else);

            // Não pode ser escopo global
            if (scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"Else cant appear in global scope", tokens[position], ErrorCode.Else);

            var scopeCommandsSize = scopes.First().Commands.Count();

            if (scopeCommandsSize < 2)
                throw new SyntaxException($"Else need to have previous if or elif", tokens[position - 1], ErrorCode.Else);

            // Verifica se existe um if ou else if anteriormente
            if(scopes.First().Commands[scopeCommandsSize - 2] != TokenType.IF &&
               scopes.First().Commands[scopeCommandsSize - 2] != TokenType.ELSEIF)
            {
                throw new SyntaxException($"Invalid else without if/elif", tokens[position - 1], ErrorCode.Else);
            }

            scopes.Push(new Scope(ScopeType.Else));
        }
    }
}
