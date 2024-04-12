using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Base;
using Language.Compiler.Entities;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila código utilizando uma arvore de estados e uma stack para controle escopo
    /// Este é o nó raiz
    /// Executada para cada linha
    /// </summary>
    public class Root : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            if (position >= tokens.Count())
                return;

            switch (tokens[position].Type)
            {
                case TokenType.FUNCTION:
                    new Function().Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.WHILE:
                    new Loop().Validate(position + 1, tokens, scopes, info);
                   break;
                case TokenType.ELSE:
                    new Else().Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.IF:
                case TokenType.ELSEIF:
                    new Condition().Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.END:
                    new End().Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.IDENTIFIER:
                    info.Lines.Add(string.Empty);
                    info.Lines.Add($"-- Instrução {string.Join(' ', tokens.Select(x => x.Content.Replace("\n", "\\n")))}");
                    new Name(true).Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.INTEGER_DECL:
                case TokenType.STRING_DECL:
                case TokenType.BOOLEAN_DECL:
                case TokenType.DECIMAL_DECL:
                    new Type().Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.COMMENT:
                    break;
                case TokenType.BREAK:
                    new Break().Validate(position + 1, tokens, scopes, info);
                    break;
                case TokenType.CONTINUE:
                    new Continue().Validate(position + 1, tokens, scopes, info);
                    break;
                    /*case TokenType.RETURN:
                        break;*/
            }
        }
    }
}
