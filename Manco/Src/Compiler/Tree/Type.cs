using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Gera código para declaração de variaveis
    /// </summary>
    public class Type : CompilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Instrução para declaração de {string.Join(' ', tokens.Select(x => x.Content.Replace("\n", "\\n")))}");

            switch (tokens[position].Type)
            {
                case TokenType.IDENTIFIER:
                    new Name().Generate(position + 1, tokens, scopes, info);
                    break;
            }
        }
    }
}
