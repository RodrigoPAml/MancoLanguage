using Manco.Lexer.Entities;
using Manco.Lexer.Enums;
using Manco.Transpiler.Base;
using Manco.Transpiler.Entities;
using Manco.Transpiler.Utils;

namespace Manco.Transpiler.Tree
{
    /// <summary>
    /// Transpila código para chamadas de identificador, atribuições, etc..
    /// </summary>
    public class Name : TranspilerTree
    {
        /// <summary>
        /// Se é atribuição/chamada de função ou declaração de variavel
        /// </summary>
        private readonly bool _isAttribution = false;

        private bool IsAttribution => _isAttribution;
        private bool IsDeclaration => !IsAttribution;

        public Name(bool isAttribution = false)
        {
            _isAttribution = isAttribution;
        }

        public override void Generate(int position, List<Token> tokens, Stack<Scope> scopes, TranspilationInfo info)
        {
            //// Cai aqui quando se esta atribuindo a uma variavel existente ou chamada de função
            if (IsAttribution)
            {
                switch (tokens[position].Type)
                {
                    // Valida Chamada de função
                    case TokenType.OPEN:
                        new FunctionCall().Generate(position, tokens, scopes, info);
                        break;
                    // Atribuição de variavel
                    case TokenType.ASSIGN:
                        {
                            var name = tokens[0].Content;
                            var tokensToAssign = tokens.Skip(2).ToList();
                            var expr = Conversions.BuildExpression(tokensToAssign);

                            info.AddLine($"{name} = {expr};");
                        }
                        break;
                    // Atribuição por indice (array)
                    case TokenType.OPEN_BRACKET:
                        {
                            var name = tokens[0].Content;
                            var index = tokens[2].Content;

                            var tokensToAssign = tokens.Skip(5).ToList();
                            var expr = Conversions.BuildExpression(tokensToAssign);

                            info.AddLine($"{name}[{index}] = {expr};");
                        }
                        break;
                }
            }

            // Cai aqui quando é declaração de variavel com ou sem atribuição
            if(IsDeclaration)
            {
                // Aqui trata a declaração sem atribuição de tipos primitivos
                if (position >= tokens.Count())
                {
                    AddNewVariable(position, tokens, scopes, false);

                    var type = Conversions.ToType(tokens[position - 2].Type);
                    var name = tokens[position - 1].Content;

                    info.AddLine($"{type} {name} = {Conversions.DefaultValue(tokens[position - 2].Type)};");
                
                    return;
                }

                switch (tokens[position].Type)
                {
                    // Declaração de variavel com atribuição
                    case TokenType.ASSIGN:
                        {
                            AddNewVariable(position, tokens, scopes, false);

                            var tokensToAssign = tokens.Skip(position + 1).ToList();

                            var expr = Conversions.BuildExpression(tokensToAssign);
                            var type = Conversions.ToType(tokens[position - 2].Type);
                            var name = tokens[position - 1].Content;

                            // Inicializar string por número
                            if (
                                tokensToAssign.Count() == 1 &&
                                tokens[position - 2].Type == TokenType.STRING_DECL &&
                                tokensToAssign[0].Type == TokenType.INTEGER_VAL)
                            {
                                info.AddLine($"{type} {name}(' ', {expr});");
                            }
                            else
                            {
                                info.AddLine($"{type} {name} = {expr};");
                            }
                        }
                        break;
                    // Declaraçao de array, sem atribuição sempre!
                    case TokenType.LESS:
                        {
                            AddNewVariable(position, tokens, scopes, true);

                            var type = Conversions.ToType(tokens[position - 2].Type);
                            var name = tokens[position - 1].Content;
                            var lenght = tokens[position + 1].Content;

                            info.AddLine($"{type} {name}[{lenght}] = {{{Conversions.DefaultValue(tokens[position - 2].Type)}}};");
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Adiciona nova variavel ao scopo
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tokens"></param>
        /// <param name="scopes"></param>
        /// <param name="isArray"></param>
        public void AddNewVariable(
            int position,
            List<Token> tokens,
            Stack<Scope> scopes,
            bool isArray
        )
        {
            scopes.First().Childrens.Add(new ScopeVariable()
            {
                Name = tokens[position - 1].Content,
                Type = tokens[position - 2].Type,
                IsArray = isArray || tokens[position - 2].Type == TokenType.STRING_DECL,
            });
        }
    }
}
