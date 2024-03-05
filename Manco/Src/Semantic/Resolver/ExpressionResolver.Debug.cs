using Language.Lexer.Entities;
using Language.Semantic.Entities;

namespace Language.Semantic.Resolver
{
    public partial class ExpressionResolver
    {
        /// <summary>
        /// Debug initial expression
        /// </summary>
        /// <param name="initial"></param>
        public void Print(List<ReducedToken> initial)
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
        /// Debug expression solving
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        public void Print(List<ReducedToken> before, List<ReducedToken> after)
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
