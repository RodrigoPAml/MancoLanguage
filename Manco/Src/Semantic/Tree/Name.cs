using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Semantic.Base;
using Language.Semantic.Entities;
using Language.Semantic.Exceptions;

namespace Language.Semantic.Tree
{
    /// <summary>
    /// Valida semantica quando invoca um indentificador
    /// </summary>
    public class Name : SemanticTree
    {
        /// <summary>
        /// Se é atribuição/chamada de função ou declaração de variavel
        /// </summary>
        private readonly bool _isAttribution = false;

        public Name(bool isAttribution = false)
        {
            _isAttribution = isAttribution;
        }

        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            // Cai aqui quando se esta atribuindo a uma variavel existente ou chamada de função
            if(_isAttribution)
            {
                switch (tokens[position].Type)
                {
                    // Valida Chamada de função
                    case TokenType.OPEN:
                        ValidateFunctionsExists(position, tokens, scopes); // Função existe ?
                        new FunctionCall().Validate(position, tokens, scopes);
                        break;
                    // Atribuição de variavel
                    case TokenType.ASSIGN:
                        ValidateVariableExists(position, tokens, scopes); // Variavel existe ?
                        new Assign().Validate(position + 1, tokens, scopes);
                        break;
                    // Atribuição por indice
                    case TokenType.OPEN_BRACKET:
                        ValidateVariableExistsArray(position, tokens, scopes); // Array existe ?
                        new ArrayAssign().Validate(position + 1, tokens, scopes);
                        break;
                    default:
                        throw new SemanticException($"Invalid attribution at {tokens[position]}, expecting a function call or assign", tokens[position], ErrorCode.InvalidAssign);
                } 
            }
            // Cai aqui quando é declaração de variavel com ou sem atribuição
            else
            {
                // Aqui trata a declaração sem atribuição
                if (position >= tokens.Count())
                {
                    if (tokens[0].Type == TokenType.STRING_DECL)
                        throw new SemanticException($"Invalid declaration at {tokens[1]}, string needs a initializer", tokens[1], ErrorCode.InvalidDeclaration);

                    ValidateVariableDontExists(position, tokens, scopes, false);
                    return;
                }

                switch (tokens[position].Type)
                {
                    // Declaração de variavel com atribuição
                    case TokenType.ASSIGN:
                        ValidateVariableDontExists(position, tokens, scopes, false);
                        new Assign().Validate(position + 1, tokens, scopes);
                        break;
                    // Declaraçao de array, sem atribuição sempre!
                    case TokenType.LESS:
                        ValidateVariableDontExists(position, tokens, scopes, true);
                        break;
                    default:
                        throw new SemanticException($"Invalid declaration {tokens[position]}, expect an assign", tokens[position], ErrorCode.InvalidDeclaration);
                }
            }
        }

        /// <summary>
        /// Variavel não pode já existir, e não ter o nome da função
        /// </summary>
        private void ValidateVariableDontExists(int position, List<Token> tokens, Stack<Scope> scopes, bool isArray)
        {
            if (scopes.Any(x => x.Variables.Any(y => y.Name == tokens[position - 1].Content && y.Type != TokenType.FUNCTION)))
                throw new SemanticException($"Variable {tokens[position - 1]} already declared", tokens[position - 1], ErrorCode.Identifier);

            string functionName = scopes.ElementAt(scopes.Count()-2).Name;

            if(functionName == tokens[position - 1].Content)
                throw new SemanticException($"Variable {tokens[position - 1]} has the same name of the function", tokens[position - 1], ErrorCode.Identifier);

            scopes.First().Variables.Add(new Variable()
            {
                Name = tokens[position - 1].Content,
                Type = tokens[position - 2].Type,
                IsArray = isArray || tokens[position -2 ].Type == TokenType.STRING_DECL,
            });
        }

        /// <summary>
        /// Valida se variavel e é um array e existe (por causa de assign por indice)
        /// </summary>
        private void ValidateVariableExistsArray(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            var variable = scopes
                 .SelectMany(x => x.Variables)
                 .Where(y => y.Name == tokens[position - 1].Content && y.Type != TokenType.FUNCTION)
                 .FirstOrDefault();

            if (variable == null)
                throw new SemanticException($"Variable {tokens[position - 1]} don't exist", tokens[position - 1], ErrorCode.ArrayDeclaration);

            if (!variable.IsArray)
                throw new SemanticException($"Variable {tokens[position - 1]} is not an array", tokens[position - 1], ErrorCode.ArrayDeclaration);

            var indexToken = tokens[2];

            if(indexToken.Type == TokenType.IDENTIFIER)
            {
                var variableIndex = scopes
                    .SelectMany(x => x.Variables)
                    .Where(y => y.Name == indexToken.Content && y.Type != TokenType.FUNCTION)
                    .FirstOrDefault();

                if (variableIndex == null)
                    throw new SemanticException($"Index variable {indexToken} must exist", indexToken, ErrorCode.ArrayIndexAssign);
                
                if (variableIndex.IsArray)
                    throw new SemanticException($"Index variable {indexToken} cant be an array", indexToken, ErrorCode.ArrayIndexAssign);
                
                if (variableIndex.Type != TokenType.INTEGER_DECL && variableIndex.Type != TokenType.INTEGER_DECL_REF)
                    throw new SemanticException($"Variable {indexToken} must be a integer to be used as a index", indexToken, ErrorCode.ArrayIndexAssign);
            }
        }

        /// <summary>
        /// Valida se variavel existe e não é um array nem string (por que não podem sofrer atriuição direta, so por indice)
        /// </summary>
        private void ValidateVariableExists(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            var variable = scopes
                .SelectMany(x => x.Variables)
                .Where(y => y.Name == tokens[position - 1].Content && y.Type != TokenType.FUNCTION)
                .FirstOrDefault();

            if (variable == null)
                throw new SemanticException($"Variable {tokens[position - 1]} don't exist", tokens[position - 1], ErrorCode.InvalidDeclaration);

            if (variable.IsArray)
                throw new SemanticException($"Variable {tokens[position - 1]} is an array", tokens[position - 1], ErrorCode.InvalidDeclaration);
        }

        /// <summary>
        /// Valida se função existe (chamada de função)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tokens"></param>
        /// <param name="scopes"></param>
        /// <exception cref="SemanticException"></exception>
        private void ValidateFunctionsExists(int position, List<Token> tokens, Stack<Scope> scopes)
        {
            if (!scopes.Any(x => x.Variables.Any(y => y.Name == tokens[position - 1].Content && y.Type == TokenType.FUNCTION)))
                throw new SemanticException($"Function {tokens[position - 1]} don't exist", tokens[position - 1], ErrorCode.FunctionDeclaration);
        }
    }
}
