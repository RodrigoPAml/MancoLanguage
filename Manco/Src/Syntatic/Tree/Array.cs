using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida declaração de um array
    /// </summary>
    public class Array : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if(tokens.Count() != 5)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if (tokens[0].Type == TokenType.STRING_DECL)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if (position != 3)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if (tokens[3].Type != TokenType.INTEGER_VAL)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if(int.Parse(tokens[3].Content) == 0)
                throw new SyntaxException($"Invalid array zero sized {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if (tokens[4].Type != TokenType.GREATER)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayDeclaration);
        }
    }
}
