using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida quando atribuia a uma variavel
    /// </summary>
    public class Assign : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if(position >= tokens.Count())
                throw new SyntaxException($"Assignment without content is not allowed", tokens[position-1], ErrorCode.InvalidAssign);

            // Valida expressão da atribuição
            new Expression().Validate(position, tokens, scopes);
        }
    }
}
