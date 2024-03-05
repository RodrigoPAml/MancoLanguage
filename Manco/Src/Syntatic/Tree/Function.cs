using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Syntatic.Base;
using Language.Syntatic.Entities;
using Language.Syntatic.Enums;
using Language.Syntatic.Exceptions;

namespace Language.Syntatic.Tree
{
    /// <summary>
    /// Valida sintaxe na declaração de uma função utilizando maquina de estados
    /// </summary>
    public class Function : SyntaxTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (position >= tokens.Count)
                throw new SyntaxException($"Invalid token {tokens[position - 1]}", tokens[position - 1], ErrorCode.FunctionDeclaration);

            // Função deve sempre estar no escopo global
            if (scopes.First().Type != ScopeType.Global)
                throw new SyntaxException($"Invalid token {tokens[position]} in this scope", tokens[position], ErrorCode.FunctionDeclaration);

            FunctionState state = FunctionState.FunctionName;

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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                                    throw new SyntaxException($"Unexpected token {token} in function declaration", token, ErrorCode.FunctionDeclaration);
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
                throw new SyntaxException($"Unexpected function declaration", tokens.Last(), ErrorCode.FunctionDeclaration);
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
