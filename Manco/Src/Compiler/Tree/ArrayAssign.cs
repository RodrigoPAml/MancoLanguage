﻿using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;
using Manco.Compiler.Utils;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Compila atribuição em array, por índice
    /// </summary>
    public class ArrayAssign : CompilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var variable = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Name == tokens[0].Content)
                .First();

            var expectedResult = TypeConverter.ExpectedResult(variable.Type, tokens[position - 1]);
            var expr = new Expression(ExpressionRestriction.None);

            // Valida/Compila expressão da atribuição ao índice
            expr.Generate(position+3, tokens, scopes, info);

            var indexVariable = scopes
              .SelectMany(x => x.Childrens)
              .Where(x => x.Name == tokens[2].Content)
              .FirstOrDefault();

            var arrType = TypeConverter.ArrayConvert(variable.Type, tokens[0]);
            int sizePerItem = arrType == TokenType.ARR_INDEX_STRING ? 1 : 4;
            string opType = arrType == TokenType.ARR_INDEX_STRING ? "b" : "w";

            if (!int.TryParse(tokens[2].Content, out int index))
                index = 0;

            info.Lines.Add(string.Empty);
            info.Lines.Add("-- Carregando endereço base no array");
            if (variable.AddressStackPos != -1)
            {
                info.Lines.Add($"lw t5 {-variable.AddressStackPos - info.StackPointer} sp");
            }
            else
            {
                info.Lines.Add($"move t5 sp");
                info.Lines.Add($"addi t5 t5 {variable?.StackPos - info.StackPointer}");
            }

            if (indexVariable != null)
            {
                info.Lines.Add("-- Carregando índice");

                if (indexVariable.FromFunction)
                {
                    info.Lines.Add($"lw t6 {-indexVariable.AddressStackPos - info.StackPointer} sp");
                    info.Lines.Add($"lw t6 0 t6");
                }
                else
                {
                    info.Lines.Add($"lw t6 {indexVariable.StackPos - info.StackPointer} sp");
                }

                info.Lines.Add($"muli t6 t6 {sizePerItem}");
                info.Lines.Add("add t5 t5 t6");
            }
            else
            {
                var calculatedIndex = (index * sizePerItem);
                info.Lines.Add($"addi t5 t5 {calculatedIndex}");
            }

            // Conversão quando retorno é float para int
            if (variable?.Type == TokenType.INTEGER_DECL && expr.GetResult()?.Type == TokenType.DECIMAL_VAL)
                info.Lines.Add("cfi t0 t0");

            // Conversão quando retorno é int pra float
            if (variable?.Type == TokenType.DECIMAL_DECL && expr.GetResult()?.Type == TokenType.INTEGER_VAL)
                info.Lines.Add("cif t0 t0");

            info.Lines.Add($"s{opType} t0 0 t5");
        }
    }
}
