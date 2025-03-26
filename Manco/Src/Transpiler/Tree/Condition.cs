using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Enums;
using Manco.Transpiler.Utils;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila condicionais
    /// </summary>
    public class Condition : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            var name = string.Join(' ', tokens.Select(x => x.Content).ToArray());
            bool isIf = tokens[position - 1].Type == TokenType.IF;

            scopes.Push(new Scope()
            {
                Name = name,
                Type = isIf
                    ? ScopeType.If
                    : ScopeType.ElseIf,
            });

            var expr = Conversions.BuildExpression(tokens.Skip(1).ToList());
          
            info.AddLine($"if ({expr})");
            info.AddLine("{");
            info.IncreaseIdentation();
        }
    }
}
