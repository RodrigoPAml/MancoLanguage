using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida assign por index de um array
    /// </summary>
    public class ArrayAssign : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (tokens[0].Type != TokenType.IDENTIFIER)
                throw new SyntaxException($"Identifier expected for array index assign", tokens[0], ErrorCode.ArrayIndexAssign);

            if (position >= tokens.Count())
                throw new SyntaxException($"Array index assign is missing elements", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if(tokens.Count() < 6)
                throw new SyntaxException($"Array index assign is missing elements", tokens.Last(), ErrorCode.ArrayIndexAssign);

            if (position != 2)
                throw new SyntaxException($"Array index assign is missing elements", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            if (tokens[2].Type != TokenType.INTEGER_VAL && tokens[2].Type != TokenType.IDENTIFIER)
                throw new SyntaxException($"Index the array must be and INTEGER_VAL or IDENTIFIER, instead got {tokens[2]}", tokens[2], ErrorCode.ArrayIndexAssign);

            if (tokens[3].Type != TokenType.CLOSE_BRACKET)
                throw new SyntaxException($"Array index assign expects a CLOSE_BRACKET but got {tokens[3]}", tokens[3], ErrorCode.ArrayIndexAssign);

            if (tokens[4].Type != TokenType.ASSIGN)
                throw new SyntaxException($"Array index assign expects an ASSIGN but got {tokens[4]}", tokens[4], ErrorCode.ArrayIndexAssign);

            new Expression().Validate(5, tokens, scopes);
        }
    }
}
