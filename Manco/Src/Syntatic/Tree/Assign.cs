using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
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
