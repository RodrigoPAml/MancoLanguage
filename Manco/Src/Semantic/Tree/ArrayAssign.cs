using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;
using Language.Semantic.Utils;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida atribuição em array, por índice
    /// </summary>
    public class ArrayAssign : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                throw new SemanticException($"Invalid token {tokens[position - 1]}", tokens[position - 1]);

            bool isExistent = tokens[0].Type == TokenType.IDENTIFIER;
            var type = tokens[0].Type;

            if (isExistent)
            {
                var variable = scopes
                   .SelectMany(x => x.Variables)
                   .Where(x => x.Name == tokens[0].Content)
                   .FirstOrDefault();

                type = variable?.Type ?? TokenType.ANY;
            }

            var expectedResult = TypeConverter.ExpectedResult(type, tokens[position - 1]);
            var expr = new Expression(
                type == TokenType.STRING_DECL 
                    ? ExpressionRestriction.StringArrayIndex
                    : ExpressionRestriction.None,
                  isExistent
                    ? string.Empty
                    : tokens[0].Content
            );

            // Valida expressão da atribuição
            expr.Validate(position+3, tokens, scopes);

            var result = expr.GetResult();

            if (result != expectedResult)
                throw new SemanticException($"Expression type {result} is not valid with expected type {expectedResult}", tokens[position - 1]);
        }
    }
}
