using Language.Lexer.Entities;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Enums;
using Language.Semantic.Resolver;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida se expressão é valida semanticamente
    /// </summary>
    public class Expression : SemanticTree
    {
        /// <summary>
        /// Resultado esperado
        /// </summary>
        private VariableType? _result = null;

        /// <summary>
        /// Variavel atual, deve ser desconsiderado seu uso na expressão
        /// </summary>
        private readonly string _currentVariable;

        /// <summary>
        /// Restrição de formação da expressão
        /// </summary>
        private readonly ExpressionRestriction _restriction;

        public Expression(ExpressionRestriction restriction = ExpressionRestriction.None, string currentVariable = "")
        {
            _currentVariable = currentVariable;
            _restriction = restriction;
        }

        /// <summary>
        /// Retorna resultado
        /// </summary>
        /// <returns></returns>
        public VariableType? GetResult()
        { 
            return _result; 
        }    

        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            _result = new ExpressionResolver().Evaluate(tokens.Skip(position).ToList(), scopes, _currentVariable, _restriction);
        }
    }
}
