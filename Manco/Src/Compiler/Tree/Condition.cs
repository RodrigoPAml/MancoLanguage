using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Base;
using Language.Compiler.Entities;
using Language.Compiler.Enums;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Compila condicionais
    /// </summary>
    public class Condition : CompilerTree
    {
        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            info.Lines.Add(string.Empty);
            info.Lines.Add($"-- Calculo de instrução de scopo");

            // Expressão da condição é do scopo de fora
            var expr = new Expression();
            expr.Validate(position, tokens, scopes, info);

            info.Lines.Add(string.Empty);
            var name = string.Join(' ', tokens.Select(x => x.Content).ToArray());
            var fatherScope = scopes.First();

            if (tokens[position - 1].Type == TokenType.IF)
            {
                info.Lines.Add($"-- Instrução condicional {name} com id {info.IdCounter} e stack inicial {info.StackPointer}");
                scopes.Push(new Scope(info.IdCounter++, ScopeType.If, name, info.StackPointer));
                fatherScope.LastIfId = info.IdCounter-1;
            }
            else
            {
                info.Lines.Add($"-- Instrução senão condicional {name} com id {info.IdCounter}");
                scopes.Push(new Scope(info.IdCounter++, ScopeType.ElseIf, name, info.StackPointer));
            }

            var currentScope = scopes.First();
            info.Lines.Add(string.Empty);

            if(currentScope.Type == ScopeType.If)
            {
                info.Lines.Add($"beq t0 one #IF_{currentScope.Id}");
                info.Lines.Add($"j #END_{currentScope.Id}");
                info.Lines.Add($"#IF_{currentScope.Id}:");
            }
            else
            {
                var previousScope = fatherScope.LastIfId;

                // ja que o fim do grupo é aqui, removemos do anterior
                info.Lines.Remove($"#ENDALL_{previousScope}:");

                info.Lines.Add($"beq t0 one #ELIF_{currentScope.Id}");
                info.Lines.Add($"j #END_{currentScope.Id}");
                info.Lines.Add($"#ELIF_{currentScope.Id}:");
            }
        }
    }
}
