using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;
using Language.Semantic.Utils;

namespace Language.Semantic.Resolver
{
    /// Prioridade dos tokens
    /// * / then % GRUPO 1
    /// + - GRUPO 2
    /// > < >= <= == != GRUPO 3
    /// and then or GRUPO 4
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Resolve expression, () by () first priority
        /// </summary>
        /// <param name="originalTokens"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        /// <exception cref="SemanticException"></exception>
        public VariableType Evaluate(
            List<Token> originalTokens, 
            Stack<Scope> scopes, 
            string currentVarName,
            ExpressionRestriction restriction
        )
        {
            List<Variable> variables = scopes
                .SelectMany(x => x.Variables)
                .Where(x => x.Name != currentVarName)
                .ToList();

            List<ReducedToken> tokens = Validate(originalTokens, variables, restriction);
            ValidateRestrictions(restriction, tokens, variables);

            tokens.Insert(0, new ReducedToken()
            { 
                Type= TokenType.OPEN,
                Content = "(",
            });

            tokens.Add(new ReducedToken()
            {
                Type = TokenType.CLOSE,
                Content = ")",
            });

            Print(tokens);

            int index = 0;
            List<ReducedToken> result = new List<ReducedToken>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    // Acumula, quando achar ) resolve até o ( mais proximo, até o fim
                    case TokenType.INTEGER_VAL:
                    case TokenType.DECIMAL_VAL:
                    case TokenType.BOOL_VAL:
                    case TokenType.STRING_VAL:
                    case TokenType.PLUS:
                    case TokenType.MINUS:
                    case TokenType.DIVIDE:
                    case TokenType.MULT:
                    case TokenType.PERCENTAGE:
                    case TokenType.GREATER:
                    case TokenType.GREATER_EQ:
                    case TokenType.LESS:
                    case TokenType.LESS_EQ:
                    case TokenType.EQUALS:
                    case TokenType.NOT_EQUAL:
                    case TokenType.IDENTIFIER:
                    case TokenType.OR:
                    case TokenType.AND:
                    case TokenType.STR_VAR:
                    case TokenType.INTEGER_VAR:
                    case TokenType.DECIMAL_VAR:
                    case TokenType.BOOL_VAR:
                    case TokenType.ARR_INDEX_BOOL:
                    case TokenType.ARR_INDEX_DECIMAL:
                    case TokenType.ARR_INDEX_INTEGER:
                    case TokenType.ARR_INDEX_STRING:
                    case TokenType.OPEN:
                        result.Add(token);
                        break;
                    case TokenType.CLOSE:
                        var lastOpen = result.FindLastIndex(x => x.Type == TokenType.OPEN);

                        if (lastOpen == -1)
                            throw new SemanticException("Internal error", token);

                        var toResolve = result.Skip(lastOpen + 1).Take(index - lastOpen - 1).ToList();

                        var resolved = ResolvePriorityTokensGroup1(toResolve);
                        resolved = ResolvePriorityTokensGroup2(resolved);
                        resolved = ResolvePriorityTokensGroup3(resolved);
                        resolved = ResolvePriorityTokensGroup4(resolved);

                        Print(toResolve, resolved);

                        if (resolved.Count() != 1)
                            throw new SemanticException("Failed to resolve expression", token);

                        result = result.Take(lastOpen).ToList();
                        result.AddRange(resolved);
                        break;
                    default:
                        throw new SemanticException($"Internal expression error, {token} not allowed in expression", token);
                }

                index++;
            }

            Console.WriteLine("Resultado final: " + result[0]);
            return TypeConverter.ExpressionResult(result[0].Type, result[0]);
        }
    }
}
