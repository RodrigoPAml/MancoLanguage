using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Enums;
using Manco.Transpiler.Tree;
using Manco.Common.Interfaces;

namespace Manco.Transpiler
{
    /// <summary>
    /// Transpilador do código
    /// Transpila código manco para c++
    /// </summary>
    public class CPlusPlusTranspiler : ITransformer
    {
        public List<string> Execute(List<List<Token>> tokens)
        {
            var info = new TranspilationInfo();
            
            info.AddLine("// Transpiled code by manco language to C++");
            info.AddLine("#include <iostream>");
            info.AddLine("#include <chrono>");

            Scope global = new Scope()
            {
                Name = "Global",
                Type = ScopeType.Global
            };

            Stack<Scope> scopes = new Stack<Scope>();    
            scopes.Push(global);

            // Adiciona função print, nativa do systema
            scopes.First().Childrens.Add(new ScopeVariable()
            {
                Name = "print",
                Type = TokenType.FUNCTION,
                FunctionArguments = new List<ScopeVariable>()
                {
                    new ScopeVariable()
                    {
                        Name= "any",
                        Type = TokenType.ANY
                    }
                }
            });

            foreach (var line in tokens)
                new Root().Generate(0, line, scopes, info);

            return info.GetLines();
        }
    }
}
