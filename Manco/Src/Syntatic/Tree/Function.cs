using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Syntatic.Base;
using Manco.Syntatic.Entities;
using Manco.Syntatic.Enums;
using Manco.Syntatic.Exceptions;

namespace Manco.Syntatic.Tree
{
    /// <summary>
    /// Valida sintaxe na declaração de uma função utilizando maquina de estados
    /// </summary>
    public class Function : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count)
                throw new SyntaxException($"Unexpected token in function declaration", tokens[position - 1], ErrorCode.FunctionDeclaration);

            // Função deve sempre estar no escopo global
            if (scopes.First().Type != ScopeType.Global)
                throw new SyntaxException($"Function declaration must be in global scope", tokens[position], ErrorCode.FunctionDeclaration);

            var state = FunctionState.FunctionName;

            // Insere escopo da função
            scopes.Push(new Scope(ScopeType.Function));

            // Valida sintaxe da função
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
                                    throw new SyntaxException($"Expecintg IDENTIFIER but got element {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Expecintg OPEN but got element {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    state = FunctionState.VariableName;
                                    break;
                                default:
                                    throw new SyntaxException($"Unexpected element {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    state = FunctionState.VariableName;
                                    break;
                                default:
                                    throw new SyntaxException($"Unexpected element {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.VariableName:
                        {
                            switch (token.Type)
                            {
                                case TokenType.IDENTIFIER:
                                    state = FunctionState.Comma;
                                    break;
                                default:
                                    throw new SyntaxException($"Expected IDENTIFIER but got {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected element  {token} in function declaration", token, ErrorCode.FunctionDeclaration);
                            }
                            break;
                        }
                    case FunctionState.ArrayClose:
                        {
                            switch (token.Type)
                            {
                                case TokenType.GREATER:
                                    state = FunctionState.ArrayPosClose;
                                    break;
                                default:
                                    throw new SyntaxException($"Unexpected element {token} in array of function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected element {token} in array of function declaration", token, ErrorCode.FunctionDeclaration);
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

            if (state != FunctionState.FunctionEnd)
                throw new SyntaxException($"Unexpected end of function declaration", tokens.Last(), ErrorCode.FunctionDeclaration);
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
