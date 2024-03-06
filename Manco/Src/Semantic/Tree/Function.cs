using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida semantica de declaração de função
    /// </summary>
    public class Function : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            // Verifica se a função com mesmo nome já não foi declarada
            if (scopes.First().Variables.Any(x => x.Name == tokens[position].Content && x.Type == TokenType.FUNCTION))
                throw new SemanticException($"Function {tokens[position]} already declared", tokens[position], ErrorCode.FunctionDeclaration);

            // Adiciona função a variaveis do escopo (global)
            var functionVar = new Variable()
            {
                Name = tokens[position].Content,
                Type = TokenType.FUNCTION,
            };

            scopes.First().Variables.Add(functionVar);
            
            scopes.Push(new Scope(ScopeType.Function, tokens[position].Content));

            // Função sem argumentos
            if (tokens[position + 2].Type == TokenType.CLOSE)
                return;

            List<Variable> variables = new List<Variable>();
            FunctionState state = FunctionState.FunctionName;

            // Itera novamente, para validar suas variaveis
            foreach (Token token in tokens.Skip(position))
            {
                switch (state)
                {
                    case FunctionState.FunctionName:
                        {
                            switch (token.Type)
                            {
                                case TokenType.IDENTIFIER:
                                    state = FunctionState.Open;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.Open:
                        {
                            switch (token.Type)
                            {
                                case TokenType.OPEN:
                                    state = FunctionState.FirstVariableType;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.FirstVariableType:
                        {
                            switch (token.Type)
                            {
                                case TokenType.CLOSE:
                                    state = FunctionState.FunctionEnd;
                                    break;
                                case TokenType.DECIMAL_DECL:
                                case TokenType.INTEGER_DECL:
                                case TokenType.BOOLEAN_DECL:
                                case TokenType.STRING_DECL:
                                case TokenType.DECIMAL_DECL_REF:
                                case TokenType.INTEGER_DECL_REF:
                                case TokenType.BOOLEAN_DECL_REF:
                                    variables.Add(new Variable()
                                    {
                                        Type = token.Type,
                                        FromFunction = true,
                                    });

                                    state = FunctionState.VariableName;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.VariableType:
                        {
                            switch (token.Type)
                            {
                                case TokenType.DECIMAL_DECL:
                                case TokenType.INTEGER_DECL:
                                case TokenType.BOOLEAN_DECL:
                                case TokenType.STRING_DECL:
                                case TokenType.DECIMAL_DECL_REF:
                                case TokenType.INTEGER_DECL_REF:
                                case TokenType.BOOLEAN_DECL_REF:
                                    variables.Add(new Variable()
                                    {
                                        Type = token.Type,
                                        FromFunction = true,
                                    });

                                    state = FunctionState.VariableName;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.VariableName:
                        {
                            switch (token.Type)
                            {
                                case TokenType.IDENTIFIER:
                                    variables[variables.Count() - 1].Name = token.Content;
                                    state = FunctionState.Comma;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.Comma:
                        {
                            switch (token.Type)
                            {
                                case TokenType.CLOSE:
                                    state = FunctionState.FunctionEnd;
                                    break;
                                case TokenType.COMMA:
                                    state = FunctionState.VariableType;
                                    break;
                                case TokenType.LESS:
                                    state = FunctionState.ArrayClose;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.ArrayClose:
                        {
                            switch (token.Type)
                            {
                                case TokenType.GREATER:
                                    variables[variables.Count() - 1].IsArray = true;
                                    variables[variables.Count() - 1].FromFunction = true;

                                    state = FunctionState.ArrayPosClose;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.ArrayPosClose:
                        {
                            switch (token.Type)
                            {
                                case TokenType.CLOSE:
                                    state = FunctionState.FunctionEnd;
                                    break;
                                case TokenType.COMMA:
                                    state = FunctionState.VariableType;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.FunctionEnd:
                        {
                            state = FunctionState.Exceed;
                            break;
                        }
                    case FunctionState.Exceed:
                        break;
                }
            }

            // Adiciona argumentos da função a variaveis do escopo da função
            foreach(var variable in variables)
            {
                string functionName = tokens[position].Content;

                // Valida se mesmo argumento já não existe
                if (scopes.First().Variables.Any(x => x.Name == variable.Name))
                    throw new SemanticException("Variable of function already declared", tokens[position], ErrorCode.FunctionDeclaration);

                // Valida argumento não tem mesmo nome que a função atual
                if (variable.Name == functionName)
                    throw new SemanticException("Variable of function have the same name of the function", tokens[position], ErrorCode.FunctionDeclaration);

                // Add functions variable in the scope, since its the first one, dont verify if already exists
                scopes.First().Variables.Add(variable);
                functionVar.ChildVariables.Add(variable);
            }
        }

        internal enum FunctionState
        {
            FunctionName,
            Open,
            FirstVariableType,
            VariableType,
            VariableName,
            Comma,
            Close,
            FunctionEnd,
            ArrayOpen,
            ArrayClose,
            ArrayPosClose,
            Exceed
        }
    }
}
