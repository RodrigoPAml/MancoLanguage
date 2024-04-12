using Language.Compiler.Entities;
using Language.Lexer.Enums;

namespace Language.Compiler.Resolver
{
    public partial class ExpressionCompiler
    {
        /// <summary>
        /// Calcula deslocamento para acesso de variavel, guarda em t5
        /// </summary>
        /// <param name="tkn"></param>
        private void SetIndexToT5(CompilationInfo info, CompilerToken tkn)
        {
            if(tkn.StackRegisterMemory != -1)
            {
                info.Lines.Add($"lw t5 {-tkn.StackRegisterMemory - info.StackPointer} sp");
                tkn.StackPos = +(info.StackPointer) + tkn.StackPos;
            }
            else 
            {
                info.Lines.Add("move t5 sp");
            }

            // Calculo de endereço base + deslocamento
            if (tkn.IndexVariable != null)
            {
                int sizePerItem = tkn.Type == TokenType.ARR_INDEX_STRING
                      ? 1
                      : 4;

                // Then its accessed by address
                if(tkn.IndexVariable.FromFunction)
                {
                    info.Lines.Add($"lw t6 {-tkn.IndexVariable.AddressStackPos - info.StackPointer} sp");
                    info.Lines.Add($"lw t6 0 t6");
                }
                else
                {
                    info.Lines.Add($"lw t6 {tkn.IndexVariable.StackPos - info.StackPointer} sp");
                }

                info.Lines.Add($"muli t6 t6 {sizePerItem}");
                info.Lines.Add("add t5 t5 t6");
            }
        }

        /// <summary>
        /// Operação entre inteiros
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <param name="returnedType"></param>
        /// <returns></returns>
        private CompilerToken IntegerOp(CompilationInfo info, CompilerToken left, CompilerToken right, string op, TokenType returnedType)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t0 {left.Content}");

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t1 {right.Content}");

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"{op} t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = returnedType,
                StackSize = 4,
                StackPos = info.StackPointer - 4,
            };
        }

        /// <summary>
        /// Operações entre decimal e inteiro
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <param name="returnedType"></param>
        /// <returns></returns>
        private CompilerToken DecimalIntegerOp(CompilationInfo info, CompilerToken left, CompilerToken right, string op, TokenType returnedType)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lfr t0 {left.Content}");

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t1 {right.Content}");

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"cif t1 t1"); // convert integer to float 
            info.Lines.Add($"{op} t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = returnedType,
                StackSize = 4,
                StackPos = info.StackPointer - 4
            };
        }

        /// <summary>
        /// Operações entre inteiro e decimal
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <param name="returnedType"></param>
        /// <returns></returns>
        private CompilerToken IntegerDecimalOp(CompilationInfo info, CompilerToken left, CompilerToken right, string op, TokenType returnedType)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t0 {left.Content}");
            info.Lines.Add("cif t0 t0"); // Convert the integer to float

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lfr t1 {right.Content}");

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"{op} t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = returnedType,
                StackSize = 4,
                StackPos = info.StackPointer - 4
            };
        }

        /// <summary>
        /// Operações entre decimais
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <param name="returnedType"></param>
        /// <returns></returns>
        private CompilerToken DecimalOp(CompilationInfo info, CompilerToken left, CompilerToken right, string op, TokenType returnedType)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lfr t0 {left.Content}");

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lfr t1 {right.Content}");

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"{op} t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = returnedType,
                StackSize = 4,
                StackPos = info.StackPointer - 4
            };
        }

        /// <summary>
        /// Operações de divisão entre inteiros, transforma pra float
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <param name="returnedType"></param>
        /// <returns></returns>
        private CompilerToken DivOp(CompilationInfo info, CompilerToken left, CompilerToken right, TokenType returnedType)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t0 {left.Content}");

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t1 {right.Content}");

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"cif t0 t0");
            info.Lines.Add($"cif t1 t1");
            info.Lines.Add($"divf t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = returnedType,
                StackSize = 4,
                StackPos = info.StackPointer - 4
            };
        }

        /// <summary>
        /// Modulus entre inteiros
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken IntegerMod(CompilationInfo info, CompilerToken left, CompilerToken right)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t0 {left.Content}");

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
                info.Lines.Add($"lir t1 {right.Content}");

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"div t0 t0 t1");
            info.Lines.Add("sw re 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = TokenType.DECIMAL_VAL,
                StackSize = 4,
                StackPos = info.StackPointer - 4
            };
        }

        /// <summary>
        /// Operação AND
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken AndOp(CompilationInfo info, CompilerToken left, CompilerToken right)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
            {
                var content = left.Content == "true" ? '1' : '0';
                info.Lines.Add($"lir t0 {content}");
            }

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
            {
                var content = right.Content == "true" ? '1' : '0';
                info.Lines.Add($"lir t1 {content}");
            }

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"and t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = TokenType.BOOL_VAL,
                StackSize = 4,
                StackPos = info.StackPointer - 4,
            };
        }

        /// <summary>
        /// Operação OR
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken OrOp(CompilationInfo info, CompilerToken left, CompilerToken right)
        {
            // Significa que esta na stack, puxamos valor para t0
            if (left.StackPos != -1)
            {
                SetIndexToT5(info, left);
                info.Lines.Add($"lw t0 {left.StackPos - info.StackPointer} t5");
            }
            else
            {
                var content = left.Content == "true" ? '1' : '0';
                info.Lines.Add($"lir t0 {content}");
            }

            // Significa que esta na stack, puxamos valor para t1
            if (right.StackPos != -1)
            {
                SetIndexToT5(info, right);
                info.Lines.Add($"lw t1 {right.StackPos - info.StackPointer} t5");
            }
            else
            {
                var content = right.Content == "true" ? '1' : '0';
                info.Lines.Add($"lir t1 {content}");
            }

            // Faz operação e salva na stack na posição atual
            info.Lines.Add($"or t0 t0 t1");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            info.StackPointer += 4;

            return new CompilerToken()
            {
                Type = TokenType.BOOL_VAL,
                StackSize = 4,
                StackPos = info.StackPointer - 4,
            };
        }

        /// <summary>
        /// Soma de strings
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken StringSum(CompilationInfo info, CompilerToken left, CompilerToken right)
        {
            int leftSize = 0;
            int rightSize = 0;
            int index = 0;

            if (left.StackPos != -1)
            {
                for (int i = 0; i < left.StackSize; i++)
                {
                    SetIndexToT5(info, left);
                    info.Lines.Add($"lb t0 {i + left.StackPos - info.StackPointer} t5");
                    info.Lines.Add($"sb t0 {index} sp");
                    index++;
                }

                leftSize = left.StackSize;
            }
            else
            {
                left.Content = new string(left.Content.Where(c => c != '"').ToArray());

                foreach (var ch in left.Content)
                {
                    info.Lines.Add($"lir t0 {(int)ch}");
                    info.Lines.Add($"sb t0 {index} sp");
                    index++;
                }

                leftSize = left.Content.Length;
            }

            if (right.StackPos != -1)
            {
                for (int i = 0; i < right.StackSize; i++)
                {
                    SetIndexToT5(info, right);
                    info.Lines.Add($"lb t1 {i + right.StackPos - info.StackPointer} t5");
                    info.Lines.Add($"sb t1 {index} sp");
                    index++;
                }

                rightSize = right.StackSize;
            }
            else
            {
                right.Content = new string(right.Content.Where(c => c != '"').ToArray());

                foreach (var ch in right.Content)
                {
                    info.Lines.Add($"lir t1 {(int)ch}");
                    info.Lines.Add($"sb t1 {index} sp");
                    index++;
                }

                rightSize = right.Content.Length;
            }

            info.Lines.Add($"addi sp sp {rightSize + leftSize}");

            var newToken = new CompilerToken()
            {
                Type = TokenType.STRING_VAL,
                StackSize = rightSize + leftSize,
                StackPos = info.StackPointer,
            };

            info.StackPointer += rightSize + leftSize;

            return newToken;
        }

        /// <summary>
        /// Comparação de strings
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken StringEqual(CompilationInfo info, CompilerToken left, CompilerToken right)
        {
            int leftSize = 0;
            int rightSize = 0;

            if (left.StackPos != -1)
                leftSize = left.StackSize;
            else
            {
                left.Content = new string(left.Content.Where(c => c != '"').ToArray());
                leftSize = left.Content.Length;
            }

            if (right.StackPos != -1)
                rightSize = right.StackSize;
            else
            {
                right.Content = new string(right.Content.Where(c => c != '"').ToArray());
                rightSize = right.Content.Length;
            }

            // If no same size, then not equal
            if (leftSize != rightSize)
                info.Lines.Add($"lir t2 0");
            else
            {
                info.Lines.Add($"lir t2 1");

                for (int i = 0; i < leftSize; i++)
                {
                    if (left.StackPos != -1)
                    {
                        SetIndexToT5(info, left);
                        info.Lines.Add($"lb t0 {i + left.StackPos - info.StackPointer} t5");
                    }
                    else
                        info.Lines.Add($"lir t0 {(int)left.Content[i]}");

                    if (right.StackPos != -1)
                    {
                        SetIndexToT5(info, right);
                        info.Lines.Add($"lb t1 {i + right.StackPos - info.StackPointer} t5");
                    }
                    else
                        info.Lines.Add($"lir t1 {(int)right.Content[i]}");

                    info.Lines.Add($"se t3 t0 t1");
                    info.Lines.Add($"and t2 t2 t3");
                }
            }

            info.Lines.Add("move t0 t2");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            var newToken = new CompilerToken()
            {
                Type = TokenType.BOOL_VAL,
                StackSize = 4,
                StackPos = info.StackPointer,
            };

            info.StackPointer += 4;

            return newToken;
        }

        /// <summary>
        /// Comparação de string
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken StringNotEqual(CompilationInfo info, CompilerToken left, CompilerToken right)
        {
            int leftSize = 0;
            int rightSize = 0;

            if (left.StackPos != -1)
                leftSize = left.StackSize;
            else
            {
                left.Content = new string(left.Content.Where(c => c != '"').ToArray());
                leftSize = left.Content.Length;
            }

            if (right.StackPos != -1)
                rightSize = right.StackSize;
            else
            {
                right.Content = new string(right.Content.Where(c => c != '"').ToArray());
                rightSize = right.Content.Length;
            }

            // If no same size, then not equal
            if (leftSize != rightSize)
                info.Lines.Add($"lir t2 1");
            else
            {
                info.Lines.Add($"lir t2 0");

                for (int i = 0; i < leftSize; i++)
                {
                    if (left.StackPos != -1)
                    {
                        SetIndexToT5(info, left);
                        info.Lines.Add($"lb t0 {i + left.StackPos - info.StackPointer} t5");
                    }
                    else
                        info.Lines.Add($"lir t0 {(int)left.Content[i]}");

                    if (right.StackPos != -1)
                    {
                        SetIndexToT5(info, right);
                        info.Lines.Add($"lb t1 {i + right.StackPos - info.StackPointer} t5");
                    }
                    else
                        info.Lines.Add($"lir t1 {(int)right.Content[i]}");

                    info.Lines.Add($"sne t3 t0 t1");
                    info.Lines.Add($"or t2 t2 t3");
                }
            }

            info.Lines.Add("move t0 t2");
            info.Lines.Add("sw t0 0 sp");
            info.Lines.Add("addi sp sp 4");

            var newToken = new CompilerToken()
            {
                Type = TokenType.BOOL_VAL,
                StackSize = 4,
                StackPos = info.StackPointer,
            };

            info.StackPointer += 4;

            return newToken;
        }

        /// <summary>
        /// Aloca string com tamanho fixo vazia
        /// </summary>
        /// <param name="info"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private CompilerToken StringAllocByInt(CompilationInfo info, CompilerToken token)
        {
            var size = int.Parse(token.Content);

            info.Lines.Add("lcr t0 ' '");

            foreach (var n in Enumerable.Range(0, size))
            {
                info.Lines.Add("sb t0 0 sp");
                info.Lines.Add("addi sp sp 1");
            }

            var newToken = new CompilerToken()
            {
                Type = TokenType.STR_VAR,
                StackSize = size,
                StackPos = info.StackPointer,
            };

            info.StackPointer += size;

            return newToken;
        }
    }
}
