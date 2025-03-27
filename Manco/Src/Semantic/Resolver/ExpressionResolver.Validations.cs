using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;
using Manco.Semantic.Exceptions;
using Manco.Semantic.Utils;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Valida tokens utilizados e simplifica
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variables"></param>
        /// <param name="restriction"></param>
        /// <returns></returns>
        /// <exception cref="SemanticException"></exception>
        public List<SemanticToken> Validate(List<Token> tokens, List<ScopeVariable> variables, ExpressionRestriction restriction)
        {
            tokens = TreatNegatives(tokens);

            var result = new List<SemanticToken>();
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
                        throw new SemanticException("Canno't access by index, variable dont exist", tokens[i], ErrorCode.Expression);

                    if (!arrVariable.IsArray)
                        throw new SemanticException("Canno't access by index, variable is not an array", tokens[i], ErrorCode.Expression);

                    var indexVariable = variables
                        .Where(x => x.Name == tokens[i+2].Content)
                        .FirstOrDefault();

                    if (tokens[i+2].Type != TokenType.INTEGER_VAL)
                    {
                        if (indexVariable == null)
                            throw new SemanticException("Canno't access by index, variable of index dont exist", tokens[i], ErrorCode.Expression);

                        if (indexVariable.IsArray)
                            throw new SemanticException("Canno't access by index, variable of index is an array", tokens[i], ErrorCode.Expression);
                    }

                    result.Add(new SemanticToken()
                    {
                        End = tokens[i+3].End,
                        Start = tokens[i].Start,
                        Type = TypeConverter.ArrayConvert(arrVariable.Type, tokens[i]),
                        Content = $"{ tokens[i].Content }{ tokens[i + 1].Content }{ tokens[i + 2].Content }{ tokens[i + 3].Content }",
                        Line = tokens[i].Line,
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
                        throw new SemanticException("Variable dont exist", tokens[i], ErrorCode.Expression);

                    // Identificadores que são array e que podem ser usados diretamos e sem indice, somente string
                    if (variable.IsArray && variable.Type != TokenType.STRING_DECL && restriction != ExpressionRestriction.ArrayReferenceVariable)
                        throw new SemanticException("Canno't use an array directly", tokens[i], ErrorCode.Expression);

                    result.Add(new SemanticToken()
                    {
                        End = tokens[i].End,
                        Start = tokens[i].Start,
                        Type = TypeConverter.IdentifierConvert(variable.Type, tokens[i]),
                        Content = tokens[i].Content,
                        Line = tokens[i].Line,
                        Variable = variable,
                        TestValue = "1",
                        IsArray = variable.IsArray,
                    });
                }
                else
                {
                    result.Add(new SemanticToken()
                    {
                        End = tokens[i].End,
                        Start = tokens[i].Start,
                        Type = tokens[i].Type,
                        Content = tokens[i].Content,
                        Line = tokens[i].Line,
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
        private void ValidateRestrictions(ExpressionRestriction restriction, List<SemanticToken> tokens, List<ScopeVariable> variables)
        {
            switch(restriction)
            {
                case ExpressionRestriction.None:
                    break;
                case ExpressionRestriction.StringArrayIndex:
                    {
                        if (tokens.Count() != 1)
                            throw new SemanticException("String index assignment must be with only a single operation", tokens[0], ErrorCode.Expression);

                        var allowed = new List<TokenType>()
                        {
                            TokenType.STRING_VAL,
                            TokenType.ARR_INDEX_STRING,
                        };

                        if (tokens.Any(x => !allowed.Contains(x.Type)))
                            throw new SemanticException($"String index assign with invalid element {tokens[0]} in composition", tokens[0], ErrorCode.Expression);

                        if (tokens[0].Content.Count() != 3 && tokens[0].Type == TokenType.STRING_VAL)
                            throw new SemanticException("String index assignment must be only with single character", tokens[0], ErrorCode.Expression);

                        if (tokens[0].Type != TokenType.ARR_INDEX_STRING && (tokens[0].Type != TokenType.STRING_VAL && tokens[0].Content.Count() != 1))
                            throw new SemanticException("String index assignment must come from another single string character", tokens[0], ErrorCode.Expression);
                    }
                    break;
                case ExpressionRestriction.StringDeclaration:
                    {
                        var allowed = new List<TokenType>()
                        {
                            TokenType.STRING_VAL,
                            TokenType.STR_VAR,
                            TokenType.ARR_INDEX_STRING,
                            TokenType.PLUS,
                        };

                        if (tokens.Count() == 1 && tokens[0].Type == TokenType.INTEGER_VAL && int.Parse(tokens[0].Content) != 0)
                            break;

                        var invalidToken = tokens.Find(x => !allowed.Contains(x.Type));
                        if (invalidToken != null)
                            throw new SemanticException("String declaration with invalid composition", invalidToken, ErrorCode.Expression);

                        invalidToken = tokens.Find(x => x.Variable != null && x.Variable.FromFunction && x.Type != TokenType.ARR_INDEX_STRING);
                        if (invalidToken != null)
                            throw new SemanticException("String declaration with invalid composion because of undetermined size", invalidToken, ErrorCode.Expression);

                        break;
                    }
                case ExpressionRestriction.SingleReferenceVariable:
                    {
                        if (tokens.Count() != 1)
                            throw new SemanticException("Reference argument must be provided by a previous declared variable", tokens[0], ErrorCode.Expression);

                        var allowed = new List<TokenType>()
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
                            throw new SemanticException("Reference argument with invalid composition, need a referenciable one", tokens[0], ErrorCode.Expression);

                        if(!variables.Where(x => x.Name == tokens[0].Content && !x.IsArray).Any())
                            throw new SemanticException($"Argument '{tokens[0].Content}' can't be an array here", tokens[0], ErrorCode.Expression);

                        break;
                    }
                case ExpressionRestriction.ArrayReferenceVariable:
                    {
                        if (tokens.Count() != 1)
                            throw new SemanticException("Reference argument must be provided by a previous declared variable", tokens[0], ErrorCode.Expression);

                        var allowed = new List<TokenType>()
                        {
                            TokenType.STR_VAR,
                            TokenType.BOOL_VAR,
                            TokenType.DECIMAL_VAR,
                            TokenType.INTEGER_VAR,
                        };

                        if (tokens.Any(x => !allowed.Contains(x.Type)))
                            throw new SemanticException("Reference argument with invalid compositon, need an array", tokens[0], ErrorCode.Expression);

                        if (!variables.Where(x => x.Name == tokens[0].Content && x.IsArray).Any())
                            throw new SemanticException($"Argument '{tokens[0].Content}' must be an array here", tokens[0], ErrorCode.Expression);

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
            var newTokens = new List<Token>();
            var allowedTypes = new List<TokenType>()
            {
                TokenType.INTEGER_VAL,
                TokenType.BOOL_VAL,
                TokenType.STRING_VAL,
                TokenType.DECIMAL_VAL,
                TokenType.IDENTIFIER,
                TokenType.CLOSE,
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
