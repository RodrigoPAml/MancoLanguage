using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
{
    /// <summary>
    /// Valida assign por index de um array
    /// </summary>
    public class ArrayAssign : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (tokens[0].Type != TokenType.IDENTIFIER)
                throw new SyntaxException($"Invalid token {tokens[0]}", tokens[0], ErrorCode.ArrayIndexAssign);

            if (position >= tokens.Count())
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if(tokens.Count() < 6)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if (position != 2)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if (tokens[2].Type != TokenType.INTEGER_VAL && tokens[2].Type != TokenType.IDENTIFIER)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if (tokens[3].Type != TokenType.CLOSE_BRACKET)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if (tokens[4].Type != TokenType.ASSIGN)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            new Expression().Validate(5, tokens, scopes);
        }
    }
}
