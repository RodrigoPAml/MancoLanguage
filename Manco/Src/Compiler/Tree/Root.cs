using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Compila código utilizando uma arvore de estados e uma stack para controle escopo
    /// Este é o nó raiz
    /// Executada para cada linha
    /// </summary>
    public class Root : CompilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            if (position >= tokens.Count())
                return;

            switch (tokens[position].Type)
            {
                case TokenType.FUNCTION:
                    new Function().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.WHILE:
                    new Loop().Generate(position + 1, tokens, scopes, info);
                   break;
                case TokenType.ELSE:
                    new Else().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.IF:
                case TokenType.ELSEIF:
                    new Condition().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.END:
                    new End().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.IDENTIFIER:
                    new Name(true).Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.INTEGER_DECL:
                case TokenType.STRING_DECL:
                case TokenType.BOOLEAN_DECL:
                case TokenType.DECIMAL_DECL:
                    new Type().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.COMMENT:
                    break;
                case TokenType.BREAK:
                    new Break().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.CONTINUE:
                    new Continue().Generate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.RETURN:
                    new Return().Generate(position + 1, tokens, scopes, info);
                    break;
            }
        }
    }
}
