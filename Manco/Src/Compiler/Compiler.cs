using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Entities;
using Language.Compiler.Enums;
using Language.Compiler.Tree;

namespace Language.Compiler
{
    /// <summary>
    /// Compilador do código
    /// Compila código assembly baseado na arquitetura MIPS
    /// </summary>
    public class Compiler
    {
        public List<string> Compile(List<List<Token>> tokens)
        {
            CompilationInfo info = new CompilationInfo();
            info.Lines.Add("j main");

            Scope global = new Scope(info.IdCounter++, ScopeType.Global);

            Stack<Scope> scopes = new Stack<Scope>();    
            scopes.Push(global);

            // Adiciona função print, nativa do systema
            scopes.First().Variables.Add(new Variable()
            {
                Name = "print",
                Type = TokenType.FUNCTION,
                ChildVariables = new List<Variable>()
                {
                    new Variable()
                    {
                        Name= "any",
                        Type = TokenType.ANY 
                    }
                }
            });

            foreach (var line in tokens)
                new Root().Validate(0, line, scopes, info);

            AddPrintStringFunction(info);
            AddPrintIntegerFunction(info);
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
