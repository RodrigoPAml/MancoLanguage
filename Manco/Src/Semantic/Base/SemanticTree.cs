﻿using Language.Lexer.Entities;
using Language.Semantic.Entities;

namespace Language.Semantic.Base
{
    /// <summary>
    /// Estrutura base da árvore que valida a semantica do código iterativamente atráves dos
    /// tokens
    /// </summary>
    public abstract class SemanticTree
    {
        abstract public void Validate(int position, List<Token> tokens, Stack<Scope> scope);
    }
}
