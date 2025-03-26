using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;
using Manco.Semantic.Exceptions;
using Manco.Semantic.Utils;

namespace Manco.Semantic.Tree
{
    /// <summary>
    /// Valida atribuição em array, por índice
    /// Foca em validar se o resultado da expressão bate com o esperado
    /// </summary>
    public class ArrayAssign : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                throw new SemanticException($"Unexcepted token at array index assign", tokens[position - 1], ErrorCode.ArrayIndexAssign);
          
            var variable = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Name == tokens[0].Content)
                .FirstOrDefault();

            if(variable == null)
                throw new SemanticException($"Internal error, should not happen", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            var expectedResult = TypeConverter.ExpectedResult(variable.Type, tokens[position - 1]);
            var expr = new Expression(
                variable.Type == TokenType.STRING_DECL 
                    ? ExpressionRestriction.StringArrayIndex // Se for assign de indice em array de string, resultado precisa ser 1 número, ou quantidade de caracter já determinada 
                    : ExpressionRestriction.None,
                  string.Empty
            );

            // Valida expressão da atribuição ao índice
            expr.Validate(position+3, tokens, scopes);

            var result = expr.GetResult();

            if (!expr.IsResultValid(expectedResult))
                throw new SemanticException($"Expression type {result} is not valid with expected type {expectedResult} in array assign", tokens[position - 1], ErrorCode.ArrayIndexAssign);
        }
    }
}
