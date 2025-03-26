using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida chamada de função
    /// </summary>
    public class FunctionCall : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count)
                throw new SyntaxException($"Function call expect more elements", tokens[position - 1], ErrorCode.FunctionCall);

            var currentTokens = tokens
                .Skip(position)
                .ToList();

            if(currentTokens.Count() < 2)
                throw new SyntaxException($"Function call expect more elements", tokens[1], ErrorCode.FunctionCall);

            // Valida abre e fecha da função
            if (currentTokens.First().Type != TokenType.OPEN)
                throw new SyntaxException($"Expecting OPEN element in function call", currentTokens.First(), ErrorCode.FunctionCall);

            if (currentTokens.Last().Type != TokenType.CLOSE)
                throw new SyntaxException($"Expecting CLOSE element in function call", currentTokens.Last(), ErrorCode.FunctionCall);

           // Funçao sem argumentos na chamada
           if (currentTokens.Count() == 2)
                return;

           // Divide argumentos da função
           var groups = SplitTokens(
                currentTokens
                    .Skip(1)
                    .Take(currentTokens.Count() - 2)
                    .ToList()
            );
            
            // Valida cada expressão da função
            foreach (var group in groups)
                new Expression().Validate(0, group, scopes);
        }

        private static List<List<Token>> SplitTokens(List<Token> tokens)
        {
            var tokenGroups = new List<List<Token>>();
            var currentGroup = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.COMMA)
                {
                    if(currentGroup.Count() == 0)
                        throw new SyntaxException($"Empty argument is not allowed in function call", token, ErrorCode.FunctionCall);

                    tokenGroups.Add(currentGroup);
                    currentGroup = new List<Token>();   
                }
                else
                {
                    currentGroup.Add(token);
                }
            }

            if (currentGroup.Count() == 0)
                throw new SyntaxException($"Empty argument is not allowed in function call", tokens.Last(), ErrorCode.FunctionCall);

            tokenGroups.Add(currentGroup);

            return tokenGroups
               .Select(x => x)
               .ToList();
        }
    }
}
