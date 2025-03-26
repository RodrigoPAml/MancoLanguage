using Manco.Lexer.Entities;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Enums;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila fim de escopos
    /// </summary>
    public class End : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            var removed = scopes.Pop();

            if (removed.Type == ScopeType.Function && removed.Name == "main")
            {
                if(!info.HaveTimerSet)
                {
                    info.AddLine("auto end = std::chrono::high_resolution_clock::now();");
                    info.AddLine("std::chrono::duration<double, std::milli> duration = end - start;");
                    info.AddLine("std::cout << std::endl << \"Execution time: \" << duration.count() << \" milliseconds\" << std::endl;");

                    info.HaveTimerSet = true;
                }
            }

            info.DecreaseIdentation();
            info.AddLine("}");
        }
    }
}
