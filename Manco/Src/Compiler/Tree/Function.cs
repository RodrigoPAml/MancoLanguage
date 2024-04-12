using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Gera código para declaração de função
    /// </summary>
    public class Function : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            // Adiciona função a variaveis do escopo (global)
            var functionVar = new Variable()
            {
                Name = tokens[position].Content,
                Type = TokenType.FUNCTION,
            };

            scopes.First().Variables.Add(functionVar);
            scopes.Push(new Scope(info.IdCounter++, ScopeType.Function, tokens[position].Content, 0));

            var scope = scopes.First();
            string functionName = tokens[position].Content;

            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Instrução para função {tokens[position].Content} with id {scope.Id}");
            info.Lines.Add($"{functionName}:");

            if(functionName != "main")
            {
                info.Lines.Add($"sw ra 0 sp");
                info.Lines.Add($"addi sp sp 4");
                info.StackPointer += 4;
            }

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
                                    variables.Add(new Variable()
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
                                    variables.Add(new Variable()
                                    {
                                        Type = token.Type,
                                        FromFunction = true,
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
            // Adiciona argumentos da função a variaveis do escopo da função
            foreach(var variable in variables)
            {
                variable.StackPos = 0;
                variable.Size = -1;
                variable.AddressStackPos = accumulatedStackPos;
                accumulatedStackPos -= 4;

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
