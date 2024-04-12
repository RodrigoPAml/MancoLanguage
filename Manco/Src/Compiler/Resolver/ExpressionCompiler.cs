using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Entities;
using Language.Compiler.Enums;
using Language.Compiler.Exceptions;

namespace Language.Compiler.Resolver
{
    /// <summary>
    /// Compila expressões para assembly e retorna
    /// token de compilação com informações, de onde esta na stack 
    /// e qual tamanho ocupa
    /// </summary>
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Resolve expressão e retorna tipo
        /// </summary>
        /// <param name="info"></param>
        /// <param name="originalTokens"></param>
        /// <param name="scopes"></param>
        /// <param name="currentVarName">Variavel atual não pode ser usada dentro da expressão</param>
        /// <param name="restriction"></param>
        /// <returns></returns>
        /// <exception cref="CompilerException"></exception>
        public CompilerToken Evaluate(
            CompilationInfo info,
            List<Token> originalTokens, 
            Stack<Scope> scopes, 
            string currentVarName,
            ExpressionRestriction restriction
        )
        {
            // Variaveis atuais disponiveis
            List<Variable> variables = scopes
                .SelectMany(x => x.Variables)
                .Where(x => x.Name != currentVarName)
                .ToList();

            // Reduz tokens para simplificação da compilação
            List<CompilerToken> tokens = Validate(originalTokens, variables, info);
          
            // Declaração de string por inteiro (alocação)
            if(tokens.Count() == 1 && restriction == ExpressionRestriction.StringDeclaration && tokens[0].Type == TokenType.INTEGER_VAL)
                return StringAllocByInt(info, tokens[0]);

            tokens.Insert(0, new CompilerToken()
            { 
                Type= TokenType.OPEN,
                Content = "(",
            });

            tokens.Add(new CompilerToken()
            {
                Type = TokenType.CLOSE,
                Content = ")",
            });

            int index = 0;
            int startingStack = info.StackPointer;
            List<CompilerToken> result = new List<CompilerToken>();

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
                        var toResolve = result.Skip(lastOpen + 1).Take(index - lastOpen - 1).ToList();

                        TreatAlone(toResolve);

                        // Resolve por prioridade o que ta dentro do (...)
                        var resolved = ResolvePriorityTokensGroup1(toResolve, info);
                        resolved = ResolvePriorityTokensGroup2(resolved, info);
                        resolved = ResolvePriorityTokensGroup3(resolved, info);
                        resolved = ResolvePriorityTokensGroup4(resolved, info);
                        
                        result = result.Take(lastOpen).ToList();
                        result.AddRange(resolved);

                        break;
                }
                index++;
            }

            return result[0];
        }
    }
}
