using Manco.Lexer.Entities;
using Manco.Semantic.Base;
using Manco.Semantic.Entities;
using Manco.Semantic.Enums;
using Manco.Semantic.Resolver;

namespace Manco.Semantic.Tree
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

        public bool IsResultValid(VariableType expectedResult, ExpressionRestriction restriction = ExpressionRestriction.None)
        {
            switch(expectedResult)
            {
                case VariableType.String:
                    return (_result == VariableType.Integer || _result == VariableType.String);
                case VariableType.Integer:
                case VariableType.Decimal:
                    {
                        if(restriction == ExpressionRestriction.SingleReferenceVariable)
                            return _result == expectedResult;

                        return (_result == VariableType.Integer || _result == VariableType.Decimal);
                    }
                default:
                    return _result == expectedResult;
            }
        }

        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            _result = new ExpressionResolver().Evaluate(tokens.Skip(position).ToList(), scopes, _currentVariable, _restriction);
        }
    }
}
