using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila código para declaração de variaveis
    /// </summary>
    public class Type : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            switch (tokens[position].Type)
            {
                case TokenType.IDENTIFIER:
                    new Name().Generate(position + 1, tokens, scopes, info);
                    break;
            }
        }
    }
}
