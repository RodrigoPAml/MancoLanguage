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
        private VariableType? _result = null;

        private readonly string _currentVariable;

        private readonly ExpressionRestriction _restriction;

        public Expression(ExpressionRestriction restriction = ExpressionRestriction.None, string currentVariable = "")
        {
            _currentVariable = currentVariable;
            _restriction = restriction;
        }

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
