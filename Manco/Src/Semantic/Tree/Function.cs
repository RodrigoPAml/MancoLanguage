using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;
using Manco.Semantic.Exceptions;

namespace Manco.Semantic.Tree
{
    /// <summary>
    /// Valida semantica de declaração de função
    /// </summary>
    public class Function : SemanticTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            // Verifica se a função com mesmo nome já não foi declarada
            if (scopes.First().Childrens.Any(x => x.Name == tokens[position].Content && x.Type == TokenType.FUNCTION))
                throw new SemanticException($"Function {tokens[position]} already declared", tokens[position], ErrorCode.FunctionDeclaration);

            var functionVar = new ScopeVariable()
            {
                Name = tokens[position].Content,
                Type = TokenType.FUNCTION,
            };

            // Adiciona função a filhos do escopo (global)
            scopes.First().Childrens.Add(functionVar);
            
            // Adiciona função como escopo global
            scopes.Push(new Scope(ScopeType.Function, tokens[position].Content));

            // Função sem argumentos
            if (tokens[position + 2].Type == TokenType.CLOSE)
                return;

            // Variaveis de scopo para função
            var variables = new List<ScopeVariable>();
            var state = FunctionState.FunctionName;

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
                                    throw new SemanticException($"Unexpected element {token} in function declaration, expecting IDENTIFIER", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SemanticException($"Unexpected element {token} in function declaration, expecting OPEN", token, ErrorCode.FunctionDeclaration);
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
                                    variables.Add(new ScopeVariable()
                                    {
                                        Type = token.Type,
                                        FromFunction = true,
                                        IsArray = token.Type == TokenType.STRING_DECL
                                            ? true
                                            : false
                                    });

                                    state = FunctionState.VariableName;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected element {token} in function variable declaration, expecting a TYPE_DECL or CLOSE", token, ErrorCode.FunctionDeclaration);
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
                                    variables.Add(new ScopeVariable()
                                    {
                                        Type = token.Type,
                                        FromFunction = true,
                                        IsArray = token.Type == TokenType.STRING_DECL
                                            ? true
                                            : false
                                    });

                                    state = FunctionState.VariableName;
                                    break;
                                default:
                                    throw new SemanticException($"Unexpected element {token} in function variable declaration, expecting a TYPE_DECL", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SemanticException($"Unexpected element {token} in function variable declaration, expecting a IDENTIFIER", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SemanticException($"Unexpected element {token} in function variable declaration, expecting a CLOSE, COMMA or LESS", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SemanticException($"Unexpected element {token} in function array variable declaration, expecting a GREATER", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SemanticException($"Unexpected element {token} in function array variable declaration, expecting a CLOSE or COMMA", token, ErrorCode.FunctionDeclaration);
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

            foreach(var variable in variables)
            {
                string functionName = tokens[position].Content;
                
                // Valida se mesmo argumento já não existe
                if (scopes.First().Childrens.Any(x => x.Name == variable.Name))
                {
                    var repeatedToken = tokens.Find(x => x.Content == variable.Name);
                    throw new SemanticException("Variable of function already declared", repeatedToken, ErrorCode.FunctionDeclaration);
                }

                // Valida argumento não tem mesmo nome que a função atual
                if (variable.Name == functionName)
                    throw new SemanticException("Variable of function have the same name of the function", tokens[position], ErrorCode.FunctionDeclaration);

                // Adiciona argumentos da função a filhos do escopo da função
                scopes.First().Childrens.Add(variable);

                // Adiciona tambem a variavel de escopo como filho dela mesma (global scope -> function scopes -> variables)
                functionVar.FunctionArguments.Add(variable);
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
