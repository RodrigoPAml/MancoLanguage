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
    /// Valida atribuição, sem acesso a array por índice
    /// Não chega aqui atribuição de string por exemplo, só se for na declaração pois so pode modificar por índice
    /// </summary>
    public class Assign : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if(position >= tokens.Count())
                throw new SemanticException($"Invalid token {tokens[position - 1]}", tokens[position-1], ErrorCode.InvalidAssign);

            // basicamente se é decleração com atribuição ou modificação
            int indexVariable =
                tokens[0].Type == TokenType.IDENTIFIER
                ? 0
                : 1;

            var variable = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Name == tokens[indexVariable].Content)
                .FirstOrDefault();

            string varName = variable?.Name ?? string.Empty;
            var type = variable?.Type ?? TokenType.ANY;

            var expectedResult = TypeConverter.ExpectedResult(type, tokens[position-1]);
            var expr = new Expression(
                type == TokenType.STRING_DECL
                    ? ExpressionRestriction.StringDeclaration
                    : ExpressionRestriction.None,
                indexVariable != 0
                    ? varName
                    : string.Empty
           );

            // Valida expressão da atribuição
            expr.Validate(position, tokens, scopes);
            
            var result = expr.GetResult();

            if (!expr.IsResultValid(expectedResult))
                throw new SemanticException($"Expression type {result} is not valid with expected type {expectedResult}", tokens[position - 1], ErrorCode.InvalidAssign);
        }
    }
}
