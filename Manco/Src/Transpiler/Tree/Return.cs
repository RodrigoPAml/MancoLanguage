using Manco.Lexer.Entities;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Enums;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila retorno de função
    /// </summary>
    public class Return : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            var function = scopes.ToList().FindLast(x => x.Type == ScopeType.Function);

            if (function.Name == "main")
            {
                if (!info.HaveTimerSet)
                {
                    info.AddLine("auto end = std::chrono::high_resolution_clock::now();");
                    info.AddLine("std::chrono::duration<double, std::milli> duration = end - start;");
                    info.AddLine("std::cout << std::endl << \"Execution time: \" << duration.count() << \" milliseconds\" << std::endl;");

                    info.HaveTimerSet = true;
                }

                info.AddLine("return 0;");
            }
            else
                info.AddLine("return;");
        }
    }
}
