using System.Text;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Enums;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila código para declaração de função
    /// </summary>
    public class Function : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
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
            scopes.Push(new Scope()
            {
                Name = functionName,
                Type = ScopeType.Function
            });

            var scope = scopes.First();

            info.AddLine(string.Empty);

            var functionBuilder = new StringBuilder();
            var variables = new List<ScopeVariable>();
            var state = FunctionState.FunctionName;

            // Parse saving information and transpiling
            foreach (Token token in tokens.Skip(position))
            {
                switch (state)
                {
                    case FunctionState.FunctionName:
                        {
                            switch (token.Type)
                            {
                                case TokenType.IDENTIFIER:
                                    var type = token.Content == "main" ? "int" : "void";
                                    functionBuilder.Append($"{type} {token.Content}");
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
                                    functionBuilder.Append($"(");

                                    state = FunctionState.VariableType;
                                    break;
                            }
                            break;
                        }
                    case FunctionState.VariableType:
                        {
                            switch (token.Type)
                            {
                                case TokenType.CLOSE:
                                    functionBuilder.Append($")");
                                    state = FunctionState.FunctionEnd;
                                    break;
                                case TokenType.DECIMAL_DECL:
                                    functionBuilder.Append($"float");
                                    state = FunctionState.VariableName;
                                    break;
                                case TokenType.INTEGER_DECL:
                                    functionBuilder.Append($"int");
                                    state = FunctionState.VariableName;
                                    break;
                                case TokenType.BOOLEAN_DECL:
                                    functionBuilder.Append($"bool");
                                    state = FunctionState.VariableName;
                                    break;
                                case TokenType.STRING_DECL:
                                    functionBuilder.Append($"std::string&");
                                    state = FunctionState.VariableName;
                                    break;
                                case TokenType.DECIMAL_DECL_REF:
                                    functionBuilder.Append($"float&");
                                    state = FunctionState.VariableName;
                                    break;
                                case TokenType.INTEGER_DECL_REF:
                                    functionBuilder.Append($"int&");
                                    state = FunctionState.VariableName;
                                    break;
                                case TokenType.BOOLEAN_DECL_REF:
                                    functionBuilder.Append($"bool&");
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
                                    
                                    variables.Add(new ScopeVariable()
                                    {
                                        Type = token.Type,
                                        FromFunction = true,
                                        Name = token.Content,
                                        IsArray = token.Type == TokenType.STRING_DECL
                                            ? true
                                            : false
                                    });

                                    functionBuilder.Append(" " + token.Content);

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
                                    functionBuilder.Append(")");
                                    state = FunctionState.FunctionEnd;
                                    break;
                                case TokenType.COMMA:
                                    functionBuilder.Append(", ");
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

                                    functionBuilder.Append("[]");
                                    
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
                                    functionBuilder.Append(")");
                                    state = FunctionState.FunctionEnd;
                                    break;
                                case TokenType.COMMA:
                                    functionBuilder.Append(", ");
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

            string result = functionBuilder.ToString();

            if (result.EndsWith(", "))
                result = result.Remove(result.Count() - 2, 2);

            info.AddLine(result);

            foreach (var variable in variables)
            {
                // Adiciona argumentos da função a filhos do escopo da função
                scopes.First().Childrens.Add(variable);

                // Adiciona tambem a variavel de escopo como filho dela mesma (global scope -> function scopes -> variables)
                functionVar.FunctionArguments.Add(variable);
            }

            info.AddLine("{");
            info.IncreaseIdentation();

            if(functionName == "main")
                info.AddLine("auto start = std::chrono::high_resolution_clock::now();");
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
