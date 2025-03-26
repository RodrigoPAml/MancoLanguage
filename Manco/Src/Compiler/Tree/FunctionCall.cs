using Manco.Common.Enums;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Compiler.Base;
using Manco.Compiler.Entities;
using Manco.Compiler.Enums;
using Manco.Compiler.Exceptions;
using Manco.Compiler.Utils;

namespace Manco.Compiler.Tree
{
    /// <summary>
    /// Compila chamada de função
    /// </summary>
    public class FunctionCall : CompilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            var currentTokens = tokens
                .Skip(position)
                .ToList();

            var functionName = tokens[position - 1].Content;

            var functionVar = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Name == functionName && x.Type == TokenType.FUNCTION)
                .FirstOrDefault();

            var variables = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Type != TokenType.FUNCTION)
                .ToList();

            // Função sem argumentos 
            if (currentTokens.Count() == 2 && functionVar?.FunctionArguments.Count == 0)
            {
                if (functionName != "main" && functionName != "print")
                    info.Lines.Add($"jal {functionName}");
                return;
            }

            // Grupos de argumentos separados por tokens, cada grupo é um argumento
            var groups = SplitTokens(
                currentTokens
                    .Skip(1)
                    .Take(currentTokens.Count() - 2)
                    .ToList()
            );

            var indexVariable = 0;
            var results = new List<Tuple<int, bool>>();

            foreach (var group in groups)
            {
                var functionVariable = functionVar?.FunctionArguments[indexVariable];
                bool isReference = false;

                var referenceTypes = new List<TokenType>()
                {
                    TokenType.BOOLEAN_DECL_REF,
                    TokenType.DECIMAL_DECL_REF,
                    TokenType.INTEGER_DECL_REF,
                };

                // Por referencia
                if (referenceTypes.Contains(functionVariable!.Type))
                    isReference = true;

                // Quando é array é referencia sempre
                if (functionVariable.IsArray)
                    isReference = true;

                // Função especial do sistema, o print
                if (functionName == "print")
                {
                    var expr = new Expression();

                    // Valida expressão do print
                    expr.Generate(0, group, scopes, info);
                    var result = expr.GetResult();

                    if(result?.Type == TokenType.INTEGER_VAL || result?.Type == TokenType.BOOL_VAL)
                    {
                        info.Lines.Add("jal #print_int");
                        info.UsePrintInt = true;
                    }
                    else if(result?.Type == TokenType.DECIMAL_VAL)
                    {
                        info.Lines.Add("jal #print_decimal");
                        info.UsePrintFloat = true;
                    }
                    else if (result?.Type == TokenType.STRING_VAL)
                    {
                        info.UsePrintString = true;

                        if (result.StackSize == -1)
                            throw new CompilerException($"Function {functionName} can't determine string size to print it, only scoped declared strings are allowed", tokens[position], ErrorCode.FunctionCall);

                        info.Lines.Add($"lir t6 {result.StackSize}");
                        info.Lines.Add("jal #print_string");
                    }
                }
                else
                {
                    if (isReference)
                    {
                        var variableRef = variables.Find(x => x.Name == group[0].Content);

                        if (variableRef != null && variableRef.FromFunction)
                            results.Add(new Tuple<int, bool>(-groups.Count()/(indexVariable+1)* 4, true)); // -4, -8, -12 (reference variables passed to function)
                        else
                            results.Add(new Tuple<int, bool>(variableRef?.StackPos ?? 0, false));
                    }
                    else
                    {
                        info.Lines.Add(string.Empty);
                        info.Lines.Add($"-- Criando valor para enviar em nome de '{functionVariable.Name}'");
                       
                        var expectedResult = TypeConverter.ExpectedResult(functionVariable.Type, tokens[position]);
                        var expr = new Expression(ExpressionRestriction.None);
                        expr.Generate(0, group, scopes, info);

                        // Conversão quando retorno é float para int
                        if (functionVariable?.Type == TokenType.INTEGER_DECL && expr.GetResult()?.Type == TokenType.DECIMAL_VAL)
                        {
                            info.Lines.Add("cfi t0 t0");
                            info.Lines.Add("sw t0 -4 sp");
                        }

                        // Conversão quando retorno é int pra float
                        if (functionVariable?.Type == TokenType.DECIMAL_DECL && expr.GetResult()?.Type == TokenType.INTEGER_VAL)
                        {
                            info.Lines.Add("cif t0 t0");
                            info.Lines.Add("sw t0 -4 sp");
                        }

                        results.Add(new Tuple<int, bool>(expr.GetResult()?.StackPos ?? 0, false));
                    }
                }

                indexVariable++;
            }

            // Passa variaveis para chamada da função (endereço somente)
            indexVariable = 0;
            foreach (var result in results)
            {
                var functionVariable = functionVar?.FunctionArguments[indexVariable];

                if (functionName != "print")
                {
                    info.Lines.Add(string.Empty);
                    info.Lines.Add($"-- Guardando endereço para variavel '{functionVariable?.Name}' para chamada de função");
                    info.Lines.Add("move t6 sp");
                    info.Lines.Add($"addi t6 t6 {result.Item1 - info.StackPointer}");
                    
                    if(result.Item2)
                        info.Lines.Add($"lw t6 0 t6");

                    info.Lines.Add($"sw t6 0 sp");
                    info.Lines.Add($"addi sp sp 4");
                    info.StackPointer += 4;
                }

                indexVariable++;
            }

            if (functionName != "main" && functionName != "print")
                info.Lines.Add($"jal {functionName}");
        }

        /// <summary>
        /// Conteudos das chamadas da função retornados aqui
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static List<List<Token>> SplitTokens(List<Token> tokens)
        {
            var tokenGroups = new List<List<Token>>();
            var currentGroup = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.COMMA)
                {
                    tokenGroups.Add(currentGroup);
                    currentGroup = new List<Token>();   
                }
                else
                {
                    currentGroup.Add(token);
                }
            }

            tokenGroups.Add(currentGroup);

            return tokenGroups
                .Where(x => x.Any())
                .Select(x => x)
                .ToList();
        }
    }
}
