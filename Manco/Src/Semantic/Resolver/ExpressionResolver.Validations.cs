using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;
using Language.Semantic.Utils;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        public List<ReducedToken> Validate(List<Token> tokens, List<Variable> variables, ExpressionRestriction restriction)
        {
            Console.WriteLine();
            tokens = TreatNegatives(tokens);

            List<ReducedToken> result = new List<ReducedToken>();
            for(var i = 0; i < tokens.Count; i++)
            {
                bool haveArr = i + 3 < tokens.Count;
                
                // Reduce array access by index to a simplified token
                if (haveArr && 
                    tokens[i].Type == TokenType.IDENTIFIER && 
                    tokens[i+1].Type == TokenType.OPEN_BRACKET
                )
                {
                    var arrVariable = variables
                        .Where(x => x.Name == tokens[i].Content)
                        .FirstOrDefault();
                    
                    if (arrVariable ==  null)
                        throw new SemanticException("Canno't access by index, variable dont exist", tokens[i]);

                    if (!arrVariable.IsArray)
                        throw new SemanticException("Canno't access by index, variable is not an array", tokens[i]);

                    var indexVariable = variables
                        .Where(x => x.Name == tokens[i+2].Content)
                        .FirstOrDefault();

                    if (tokens[i+2].Type != TokenType.INTEGER_VAL)
                    {
                        if (indexVariable == null)
                            throw new SemanticException("Canno't access by index, variable of index dont exist", tokens[i]);

                        if (indexVariable.IsArray)
                            throw new SemanticException("Canno't access by index, variable of index is an array", tokens[i]);
                    }

                    Console.WriteLine($"Mocked value: {tokens[i].Content}{tokens[i + 1].Content}{tokens[i + 2].Content}{tokens[i + 3].Content} = 1");

                    result.Add(new ReducedToken()
                    {
                        End = tokens[i+3].End,
                        Start = tokens[i].Start,
                        Type = TypeConverter.ArrayConvert(arrVariable.Type, tokens[i]),
                        Content = $"{ tokens[i].Content }{ tokens[i + 1].Content }{ tokens[i + 2].Content }{ tokens[i + 3].Content }",
                        Line = tokens[i].Line,
                        MoreOnLeft = tokens[i+3].MoreOnLeft,
                        Variable = arrVariable,
                        IsArray = true,
                        TestValue = "1",
                    });

                    i = i + 3;
                }
                else if (tokens[i].Type == TokenType.IDENTIFIER)
                {
                    var variable = variables
                       .Where(x => x.Name == tokens[i].Content)
                       .FirstOrDefault();

                    if (variable == null)
                        throw new SemanticException("Variable dont exist", tokens[i]);

                    // Identificadores que são array e que podem ser usados diretamos e sem indice, somente string
                    if (variable.IsArray && variable.Type != TokenType.STRING_DECL && restriction != ExpressionRestriction.ArrayReferenceVariable)
                        throw new SemanticException("Canno't use an array directly", tokens[i]);

                    Console.WriteLine($"Mocked value: {tokens[i].Content} = 1");

                    result.Add(new ReducedToken()
                    {
                        End = tokens[i].End,
                        Start = tokens[i].Start,
                        Type = TypeConverter.IdentifierConvert(variable.Type, tokens[i]),
                        Content = tokens[i].Content,
                        Line = tokens[i].Line,
                        MoreOnLeft = tokens[i].MoreOnLeft,
                        Variable = variable,
                        TestValue = "1",
                        IsArray = variable.IsArray
                    });
                }
                else
                {
                    result.Add(new ReducedToken()
                    {
                        End = tokens[i].End,
                        Start = tokens[i].Start,
                        Type = tokens[i].Type,
                        Content = tokens[i].Content,
                        Line = tokens[i].Line,
                        MoreOnLeft = tokens[i].MoreOnLeft,
                        Variable = null,
                        TestValue = tokens[i].Content,
                    });
                }
            }

            return result; 
        }

        /// <summary>
        /// Valida restrições de expressão
        /// </summary>
        /// <param name="restriction"></param>
        /// <param name="tokens"></param>
        /// <param name="variables"></param>
        private void ValidateRestrictions(ExpressionRestriction restriction, List<ReducedToken> tokens, List<Variable> variables)
        {
            switch(restriction)
            {
                case ExpressionRestriction.None:
                    break;
                case ExpressionRestriction.StringArrayIndex:
                    {
                        if (tokens.Count() != 1)
                            throw new SemanticException("String index assignment must be with only a single operation", tokens[0]);

                        if (tokens[0].Type != TokenType.ARR_INDEX_STRING && (tokens[0].Type != TokenType.STRING_VAL && tokens[0].Content.Count() != 1))
                            throw new SemanticException("String index assignment must come from another single string character", tokens[0]);
                    }
                    break;
                case ExpressionRestriction.StringDeclaration:
                    {
                        List<TokenType> allowed = new List<TokenType>()
                        {
                            TokenType.STRING_VAL,
                            TokenType.STR_VAR,
                            TokenType.ARR_INDEX_STRING,
                            TokenType.PLUS
                        };

                        if (tokens.Any(x => !allowed.Contains(x.Type)))
                            throw new SemanticException("String declaration with invalid composion", tokens[0]);

                        if(tokens.Any(x => x.Variable != null && x.Variable.FromFunction))
                            throw new SemanticException("String declaration with invalid composion because of undetermined size", tokens[0]);

                        break;
                    }
                case ExpressionRestriction.SingleReferenceVariable:
                    {
                        if (tokens.Count() != 1)
                            throw new SemanticException("Reference argument must be provided by a previous declared variable", tokens[0]);

                        List<TokenType> allowed = new List<TokenType>()
                        {
                            TokenType.STR_VAR,
                            TokenType.BOOL_VAR,
                            TokenType.DECIMAL_VAR,
                            TokenType.INTEGER_VAR,
                            TokenType.ARR_INDEX_BOOL,
                            TokenType.ARR_INDEX_DECIMAL,
                            TokenType.ARR_INDEX_INTEGER,
                            TokenType.ARR_INDEX_STRING,
                        };

                        if (tokens.Any(x => !allowed.Contains(x.Type)))
                            throw new SemanticException("Reference argument with invalid composition, need a referenciable one", tokens[0]);

                        if(!variables.Where(x => x.Name == tokens[0].Content && !x.IsArray).Any())
                            throw new SemanticException($"Argument {tokens[0].Content} can't be an array here", tokens[0]);

                        break;
                    }
                case ExpressionRestriction.ArrayReferenceVariable:
                    {
                        if (tokens.Count() != 1)
                            throw new SemanticException("Reference argument must be provided by a previous declared variable", tokens[0]);

                        List<TokenType> allowed = new List<TokenType>()
                        {
                            TokenType.STR_VAR,
                            TokenType.BOOL_VAR,
                            TokenType.DECIMAL_VAR,
                            TokenType.INTEGER_VAR,
                        };

                        if (tokens.Any(x => !allowed.Contains(x.Type)))
                            throw new SemanticException("Reference argument with invalid compositon, need an array", tokens[0]);

                        if (!variables.Where(x => x.Name == tokens[0].Content && x.IsArray).Any())
                            throw new SemanticException($"Argument {tokens[0].Content} must be an array here", tokens[0]);

                        break;
                    }
            }
        }

        /// <summary>
        /// Put zeros before minus to simplify operations
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private List<Token> TreatNegatives(List<Token> tokens)
        {
            List<Token> newTokens = new List<Token>();
            List<TokenType> allowedTypes = new List<TokenType>()
            {
                TokenType.INTEGER_VAL,
                TokenType.BOOL_VAL,
                TokenType.STRING_VAL,
                TokenType.DECIMAL_VAL,
                TokenType.IDENTIFIER,
            };

            int index = 0;
            foreach (Token token in tokens)
            {
                if (token.Type == TokenType.MINUS)
                {
                    if(index == 0)
                    {
                        newTokens.Add(new Token()
                        {
                            Type= TokenType.INTEGER_VAL,
                            Content = "0"
                        });
                    }
                    else if (!allowedTypes.Contains(tokens[index-1].Type))
                    {
                        newTokens.Add(new Token()
                        {
                            Type = TokenType.INTEGER_VAL,
                            Content = "0"
                        });
                    }
                }

                newTokens.Add(token);
                index++;
            }

            return newTokens;
        }
    }
}
