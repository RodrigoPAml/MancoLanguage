using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
{
    /// <summary>
    /// Valida chamada de função
    /// </summary>
    public class FunctionCall : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.FunctionCall);

            var currentTokens = tokens
                .Skip(position)
                .ToList();

            if(currentTokens.Count() < 2)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.FunctionCall);

           // Valida abre e fecha da função
           if(currentTokens.First().Type != TokenType.OPEN || currentTokens.Last().Type != TokenType.CLOSE)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.FunctionCall);

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
            {
                if(group.Count() == 0)
                    throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.FunctionCall);

                new Expression().Validate(0, group, scopes);
            }
        }

        private static List<List<Token>> SplitTokens(List<Token> tokens)
        {
            List<List<Token>> tokenGroups = new List<List<Token>>();
            List<Token> currentGroup = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.COMMA)
                {
                    tokenGroups.Add(currentGroup);
                    currentGroup = new List<Token>();   
                }
                else
                {
                    currentGroup.Add(token);
                }
            }

            tokenGroups.Add(currentGroup);

            return tokenGroups
               .Where(x => x.Any())
               .Select(x => x)
               .ToList();
        }
    }
}
