using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;
using Manco.Compiler.Tree;
using Manco.Common.Interfaces;

namespace Manco.Compiler
{
    /// <summary>
    /// Compilador do código
    /// Compila código assembly baseado na arquitetura MIPS
    /// </summary>
    public class MipsCompiler : ITransformer
    {
        public List<string> Execute(List<List<Token>> tokens)
        {
            var info = new CompilationInfo();
            info.Lines.Add("-- Código compilado para arquitetura baseada em MIPS architecture");
            info.Lines.Add("j main");

            var global = new Scope(info.IdCounter++, ScopeType.Global);

            var scopes = new Stack<Scope>();    
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

            if(info.UsePrintString)
                AddPrintStringFunction(info);
            
            if(info.UsePrintInt)
                AddPrintIntegerFunction(info);

            if (info.UsePrintFloat)
                AddPrintDecimalFunction(info);

            info.Lines.Add("end:");
            return info.Lines;
        }

        private void AddPrintStringFunction(CompilationInfo info)
        {
            info.Lines.Add(string.Empty);

            info.Lines.Add("#print_string:");
            info.Lines.Add("lir v0 3");

            info.Lines.Add("bne t6 zero #begin_print_str");
            info.Lines.Add("j #end_print_str");
            info.Lines.Add("#begin_print_str:");
            info.Lines.Add("sub t7 sp t6");
            info.Lines.Add("lw a0 0 t7");
            info.Lines.Add("syscall");

            info.Lines.Add("sub t6 t6 one");
            info.Lines.Add("j #print_string");
            info.Lines.Add("#end_print_str:");
            info.Lines.Add("jr ra");
        }

        private void AddPrintIntegerFunction(CompilationInfo info)
        {
            info.Lines.Add(string.Empty);

            info.Lines.Add("#print_int:");
            info.Lines.Add("lw t0 -4 sp");
            info.Lines.Add("lir v0 1");
            info.Lines.Add("move a0 t0");
            info.Lines.Add("syscall");
            info.Lines.Add("jr ra");
        }

        private void AddPrintDecimalFunction(CompilationInfo info)
        {
            info.Lines.Add(string.Empty);

            info.Lines.Add("#print_decimal:");
            info.Lines.Add("lw t0 -4 sp");
            info.Lines.Add("lir v0 2");
            info.Lines.Add("move a0 t0");
            info.Lines.Add("syscall");
            info.Lines.Add("jr ra");
        }
    }
}
