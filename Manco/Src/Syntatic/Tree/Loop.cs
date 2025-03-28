﻿using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Enums;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida sintaxe do loop
    /// </summary>
    public class Loop : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                throw new SyntaxException($"Invalid while, no expression", tokens[0], ErrorCode.Loop);

            // While não pode estar no escopo global
            if (scopes.First().Type == ScopeType.Global)
                throw new SyntaxException($"While cant appear in global scope", tokens[0], ErrorCode.Loop);

            // Insere novo escopo
            scopes.Push(new Scope(ScopeType.Loop));

            // Valida expressão do while 
            new Expression().Validate(position, tokens, scopes);
        }
    }
}
