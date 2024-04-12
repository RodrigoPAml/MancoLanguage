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
    /// Valida IF/ELSE IF
    /// </summary>
    public class Condition : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                throw new SyntaxException($"Conditional without expression", tokens[position - 1], ErrorCode.Conditions);

            // Se for if só puxamos novo escopo
            if (tokens[position-1].Type == TokenType.IF)
                scopes.Push(new Scope(ScopeType.If));
            else
            {
                var scopeCommandsSize = scopes.First().Commands.Count();

                if (scopeCommandsSize < 2)
                    throw new SyntaxException($"Invalid elif without previous if", tokens[position - 1], ErrorCode.Conditions);

                // Case seja um else if, precisa ter um if ou else if anteriomente
                if (scopes.First().Commands[scopeCommandsSize - 2] != TokenType.IF &&
                    scopes.First().Commands[scopeCommandsSize - 2] != TokenType.ELSEIF)
                {
                    throw new SyntaxException($"Invalid elif without if/elif at {tokens[position - 1]}", tokens[position - 1], ErrorCode.Conditions);
                }

                // Puxa escopo
                scopes.Push(new Scope(ScopeType.ElseIf));
            }

            // Valida expressão do if/else if
            new Expression().Validate(position, tokens, scopes);
        }
    }
}
