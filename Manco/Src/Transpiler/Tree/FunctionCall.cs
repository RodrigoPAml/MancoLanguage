using System.Text;
using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Utils;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila chamada de função
    /// </summary>
    public class FunctionCall : TranspilerTree
    {
        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            var currentTokens = tokens
                .Skip(position)
                .ToList();

            var functionName = tokens[position - 1].Content;

            // Variaveis da sua função (argumentos)
            var functionVar = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Name == functionName && x.Type == TokenType.FUNCTION)
                .FirstOrDefault();

            // Todas variaveis
            var variables = scopes
                .SelectMany(x => x.Childrens)
                .Where(x => x.Type != TokenType.FUNCTION)
                .ToList();

            // Função sem argumentos 
            if (currentTokens.Count() == 2 && functionVar?.FunctionArguments.Count == 0)
            {
                info.AddLine($"{functionName}();");
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
            var callBuilder = new StringBuilder();
            
            if (functionName != "print")
                callBuilder.Append($"{functionName}(");
            else
                callBuilder.Append($"std::cout << ");

            foreach (var group in groups)
            {
                var functionVariable = functionVar?.FunctionArguments[indexVariable];
                var expr = Conversions.BuildExpression(group);

                // Função especial do sistema, o print
                if (functionName == "print")
                {
                    callBuilder.Append(expr);
                }
                else
                {
                    callBuilder.Append($"{expr}, ");
                }

                indexVariable++;
            }

            var result = callBuilder.ToString();

            if(result.EndsWith(", "))
                result = result.Remove(result.Count()-2, 2);

            if(functionName == "print")
                info.AddLine(result + ";");
            else
                info.AddLine(result + ");");
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
