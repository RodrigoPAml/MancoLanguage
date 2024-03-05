﻿using Language.Lexer.Entities;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida semantica do loop (apenas expressão)
    /// </summary>
    public class Loop : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            scopes.Push(new Scope(ScopeType.Loop));

            var expr = new Expression();

            expr.Validate(position, tokens, scopes);
            var result = expr.GetResult();

            if(result != VariableType.Boolean)
                throw new SemanticException($"Expression type {result} is not valid with expected type {VariableType.Boolean}", tokens[position - 1]);
        }
    }
}
