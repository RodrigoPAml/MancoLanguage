using Manco.Semantic.Entities;

namespace Manco.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Mostra expressão inicial
        /// </summary>
        /// <param name="initial"></param>
        public void Print(List<SemanticToken> initial)
        {
            Console.WriteLine();
            Console.Write("Initial by value: ");
            foreach (var token in initial)
            {
                Console.Write($"{token.Content} ");
            }
            Console.WriteLine();

            Console.Write("Initial by type: ");
            foreach (var token in initial)
            {
                Console.Write($"{token.Type} ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Mostra expressão resolvida
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        public void Print(List<SemanticToken> before, List<SemanticToken> after)
        {
            Console.Write("Resolving step: ");
            foreach (var token in before)
            {
                Console.Write($"{token.Content} ");
            }
            Console.Write("= ");
            foreach (var token in after)
            {
                Console.Write($"{token.Content} ");
            }
            Console.WriteLine();
        }
    }
}
