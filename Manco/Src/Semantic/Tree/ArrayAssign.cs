using Language.Common.Enums;
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
    /// Foca em validar se o resultado da expressão bate com o esperado
    /// </summary>
    public class ArrayAssign : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count())
                throw new SemanticException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.ArrayIndexAssign);
          
            var variable = scopes
                .SelectMany(x => x.Variables)
                .Where(x => x.Name == tokens[0].Content)
                .FirstOrDefault();

            if(variable == null)
                throw new SemanticException($"Internal error", tokens[position - 1], ErrorCode.ArrayIndexAssign);

            var expectedResult = TypeConverter.ExpectedResult(variable.Type, tokens[position - 1]);
            var expr = new Expression(
                variable.Type == TokenType.STRING_DECL 
                    ? ExpressionRestriction.StringArrayIndex // Se for array de string, resultado precisa ser 1 caracter só 
                    : ExpressionRestriction.None,
                  tokens[0].Content
            );

            // Valida expressão da atribuição ao índice
            expr.Validate(position+3, tokens, scopes);

            var result = expr.GetResult();

            if (result != expectedResult)
                throw new SemanticException($"Expression type {result} is not valid with expected type {expectedResult}", tokens[position - 1], ErrorCode.ArrayIndexAssign);
        }
    }
}
