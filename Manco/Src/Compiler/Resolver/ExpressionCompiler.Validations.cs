using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Entities;
using Language.Compiler.Exceptions;
using Language.Compiler.Utils;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Valida tokens utilizados e simplifica
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variables"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        /// <exception cref="CompilerException"></exception>
        private List<CompilerToken> Validate(List<Token> tokens, List<Variable> variables, CompilationInfo info)
        {
            tokens = TreatNegatives(tokens);

            List<CompilerToken> result = new List<CompilerToken>();
            for(var i = 0; i < tokens.Count; i++)
            {
                bool haveArr = i + 3 < tokens.Count;
                
                // Variavel accesso por indice
                if (haveArr && 
                    tokens[i].Type == TokenType.IDENTIFIER && 
                    tokens[i+1].Type == TokenType.OPEN_BRACKET
                )
                {
                    var arrVariable = variables
                        .Where(x => x.Name == tokens[i].Content)
                        .FirstOrDefault();

                    var arrType = TypeConverter.ArrayConvert(arrVariable.Type, tokens[i]);
                    int sizePerItem = arrType == TokenType.ARR_INDEX_STRING 
                        ? 1
                        : 4;

                    string indexVariable = string.Empty;

                    if(!int.TryParse(tokens[i + 2].Content, out int index))
                    {
                        index = 0;
                        indexVariable = tokens[i + 2].Content;
                    }
                    
                    result.Add(new CompilerToken()
                    {
                        End = tokens[i+3].End,
                        Start = tokens[i].Start,
                        Type = arrType,
                        Content = $"{ tokens[i].Content }{ tokens[i + 1].Content }{ tokens[i + 2].Content }{ tokens[i + 3].Content }",
                        Line = tokens[i].Line,
                        MoreOnLeft = tokens[i+3].MoreOnLeft,
                        Variable = arrVariable,
                        IsArray = true,
                        StackPos = arrVariable.StackPos + (index * sizePerItem),
                        StackRegisterMemory = arrVariable.AddressStackPos,
                        StackSize = sizePerItem,
                        IndexVariable = !string.IsNullOrEmpty(indexVariable)
                            ? variables.Find(x => x.Name == indexVariable) 
                            : null,
                    });

                    i = i + 3;
                }
                // Variavel normal
                else if (tokens[i].Type == TokenType.IDENTIFIER)
                {
                    var variable = variables
                       .Where(x => x.Name == tokens[i].Content)
                       .FirstOrDefault();

                    result.Add(new CompilerToken()
                    {
                        End = tokens[i].End,
                        Start = tokens[i].Start,
                        Type = TypeConverter.IdentifierConvert(variable.Type, tokens[i]),
                        Content = tokens[i].Content,
                        Line = tokens[i].Line,
                        MoreOnLeft = tokens[i].MoreOnLeft,
                        Variable = variable,
                        StackRegisterMemory = variable.AddressStackPos,
                        IsArray = variable.IsArray,
                        StackPos = variable.StackPos,
                        StackSize = variable.Size
                    });
                }
                // Sem variavel atrelada
                else
                {
                    result.Add(new CompilerToken()
                    {
                        End = tokens[i].End,
                        Start = tokens[i].Start,
                        Type = tokens[i].Type,
                        Content = tokens[i].Content,
                        Line = tokens[i].Line,
                        MoreOnLeft = tokens[i].MoreOnLeft,
                        Variable = null,
                    });
                }
            }

            return result; 
        }

        /// <summary>
        /// Simplica operações com negativo, botando operador antes em alguns casos
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

        /// <summary>
        /// Trata valores sozinhos, já que não existe operação pra eles, Exemplo:
        /// (10) vai para (0 + 10)
        /// "123" vai para ("" + "123")
        /// (true) para (false or true)
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private void TreatAlone(List<CompilerToken> toResolve)
        {
            if (toResolve.Count() != 1)
                return;

            List<TokenType> numbers = new List<TokenType>()
            {
                TokenType.INTEGER_VAL,
                TokenType.INTEGER_VAR,
                TokenType.ARR_INDEX_INTEGER,
                TokenType.DECIMAL_VAL,
                TokenType.DECIMAL_VAR,
                TokenType.ARR_INDEX_DECIMAL,
            };

            if (numbers.Contains(toResolve[0].Type))
            {
                toResolve.Insert(0, new CompilerToken()
                {
                    Content = "+",
                    Type = TokenType.PLUS
                });

                toResolve.Insert(0, new CompilerToken()
                {
                    Content = "0",
                    Type = TokenType.INTEGER_VAL
                });

                return;
            }

            List<TokenType> booleans = new List<TokenType>()
            {
                TokenType.BOOL_VAL,
                TokenType.BOOL_VAR,
                TokenType.ARR_INDEX_BOOL
            };

            if (booleans.Contains(toResolve[0].Type))
            {
                toResolve.Insert(0, new CompilerToken()
                {
                    Content = "OR",
                    Type = TokenType.OR
                });

                toResolve.Insert(0, new CompilerToken()
                {
                    Content = "false",
                    Type = TokenType.BOOL_VAL
                });

                return;
            }

            List<TokenType> strings = new List<TokenType>()
            {
                TokenType.STRING_VAL,
                TokenType.STR_VAR,
                TokenType.ARR_INDEX_STRING
            };

            if (strings.Contains(toResolve[0].Type))
            {
                toResolve.Add(new CompilerToken()
                {
                    Content = "+",
                    Type = TokenType.PLUS
                });

                toResolve.Add(new CompilerToken()
                {
                    Content = string.Empty,
                    Type = TokenType.STRING_VAL
                });

                return;
            }
        }
    }
}
