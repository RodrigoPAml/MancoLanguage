using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Base;
using Language.Compiler.Entities;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Gera código para declaração de variaveis
    /// </summary>
    public class Type : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Instrução para declaração de {string.Join(' ', tokens.Select(x => x.Content.Replace("\n", "\\n")))}");

            switch (tokens[position].Type)
            {
                case TokenType.IDENTIFIER:
                    new Name().Validate(position + 1, tokens, scopes, info);
                    break;
            }
        }
    }
}
