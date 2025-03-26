using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Gera código para declaração de função
    /// </summary>
    public class Function : CompilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var functionName = tokens[position].Content;
            var functionVar = new ScopeVariable()
            {
                Name = functionName,
                Type = TokenType.FUNCTION,
            };

            // Adiciona função a filhos do escopo (global)
            scopes.First().Childrens.Add(functionVar);

            // Adiciona função como escopo
            scopes.Push(new Scope(info.IdCounter++, ScopeType.Function, functionName, 0));

            // Escopo atual
            var scope = scopes.First();

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Instrução para função {functionName} with id {scope.Id}");
            info.Lines.Add($"{functionName}:");

            // Função main não foi chamada por nenhuma outra, não precisa de return address
            if(functionName != "main")
            {
                info.Lines.Add($"sw ra 0 sp");
                info.Lines.Add($"addi sp sp 4");
                info.StackPointer += 4;
            }

            // Função sem argumentos
            if (tokens[position + 2].Type == TokenType.CLOSE)
                return;

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

            int accumulatedStackPos = variables.Count() * 4;
            foreach(var variable in variables)
            {
                variable.StackPos = 0;
                variable.Size = -1;

                // Variaveis passada por função sempre terão so endereços na ordem -4, -8, -12... na stack, ante de chamar a função.
                variable.AddressStackPos = accumulatedStackPos;

                accumulatedStackPos -= 4; // Endereço = 4 bytes

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
