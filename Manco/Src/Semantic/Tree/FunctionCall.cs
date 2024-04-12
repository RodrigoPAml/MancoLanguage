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
    /// Valida chamada de função
    /// </summary>
    public class FunctionCall : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            var currentTokens = tokens
                .Skip(position)
                .ToList();

            var functionName = tokens[position - 1].Content;
            var functionVar = scopes
                .SelectMany(x => x.Variables)
                .Where(x => x.Name == functionName && x.Type == TokenType.FUNCTION)
                .FirstOrDefault();

            if (functionVar == null)
                throw new SemanticException($"Function {functionName} was not founded in the global scope", tokens[position], ErrorCode.FunctionCall);

            // Função sem argumentos 
            if (currentTokens.Count() == 2 && functionVar.ChildVariables.Count == 0)
                return;

            var groups = SplitTokens(
                currentTokens
                    .Skip(1)
                    .Take(currentTokens.Count() - 2)
                    .ToList()
            );

            // Não bate número de argumentos
            if (groups.Count != functionVar.ChildVariables.Count)
                throw new SemanticException($"Function {functionName} expects {functionVar.ChildVariables.Count} arguments but recieved {groups.Count}", tokens[position], ErrorCode.FunctionCall);

            var indexVariable = 0;
            foreach (var group in groups)
            {
                var functionVariable = functionVar.ChildVariables[indexVariable];
                var restriction = ExpressionRestriction.None;

                List<TokenType> referenceTypes = new List<TokenType>()
                {
                    TokenType.BOOLEAN_DECL_REF,
                    TokenType.DECIMAL_DECL_REF,
                    TokenType.INTEGER_DECL_REF,
                };

                // Por referencia
                if (referenceTypes.Contains(functionVariable.Type))
                    restriction = ExpressionRestriction.SingleReferenceVariable;

                // Quando é array é referencia sempre
                if (functionVariable.IsArray)
                    restriction = ExpressionRestriction.ArrayReferenceVariable;

                if (functionName == "print")
                {
                    var expr = new Expression();

                    // Valida expressão do print
                    expr.Validate(0, group, scopes);
                }
                else
                {
                    var expectedResult = TypeConverter.ExpectedResult(functionVariable.Type, tokens[position]);
                    var expr = new Expression(restriction);

                    // Valida expressão da atribuição
                    expr.Validate(0, group, scopes);

                    var result = expr.GetResult();

                    if (!expr.IsResultValid(expectedResult))
                        throw new SemanticException($"Expression type {result} is not valid with expected function type {expectedResult}", tokens[position - 1], ErrorCode.FunctionCall);
                }

                indexVariable++;
            }
        }

        /// <summary>
        /// Conteudos das chamadas da função retornados aqui
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
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
