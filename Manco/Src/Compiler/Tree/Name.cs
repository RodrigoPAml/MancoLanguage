using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Compiler.Base;
using Language.Compiler.Entities;

namespace Language.Compiler.Tree
{
    /// <summary>
    /// Gera código para chamadas de identificador, atribuições, etc..
    /// </summary>
    public class Name : CompilerTree
    {
        /// <summary>
        /// Se é atribuição/chamada de função ou declaração de variavel
        /// </summary>
        private readonly bool _isAttribution = false;

        public Name(bool isAttribution = false)
        {
            _isAttribution = isAttribution;
        }

        public override void Validate(int position, List<Token> tokens, Stack<Scope> scopes, CompilationInfo info)
        {
            // Cai aqui quando se esta atribuindo a uma variavel existente ou chamada de função
            if(_isAttribution)
            {
                info.Lines.Add(string.Empty);
                info.Lines.Add($"-- Instrução {string.Join(' ', tokens.Select(x => x.Content.Replace("\n", "\\n")))}");

                switch (tokens[position].Type)
                {
                    // Valida Chamada de função
                    case TokenType.OPEN:
                        new FunctionCall().Validate(position, tokens, scopes, info);
                        break;
                    // Atribuição de variavel
                    case TokenType.ASSIGN:
                        {
                            var assign = new Assign();
                            assign.Validate(position + 1, tokens, scopes, info);
                            ChangeVariable(info, scopes, tokens, assign?.GetResult()?.Type);
                        }
                        break;
                    // Atribuição por indice (array)
                    case TokenType.OPEN_BRACKET:
                        new ArrayAssign().Validate(position + 1, tokens, scopes, info);
                        break;
                } 
            }
            // Cai aqui quando é declaração de variavel com ou sem atribuição
            else
            {
                // Aqui trata a declaração sem atribuição de tipos primitivos
                if (position >= tokens.Count())
                {
                    AddNewVariable(position, tokens, scopes, false);

                    var sizeItem = tokens[0].Type == TokenType.STRING_DECL
                               ? 1
                               : 4;

                    AddEmptyStack(sizeItem, sizeItem, info);
                    SetEmptyStackPos(position, tokens, scopes, info, sizeItem);
                    return;
                }

                switch (tokens[position].Type)
                {
                    // Declaração de variavel com atribuição
                    case TokenType.ASSIGN:
                        {
                            AddNewVariable(position, tokens, scopes, false);

                            var assign = new Assign();
                            assign.Validate(position + 1, tokens, scopes, info);

                            var result = assign.GetResult();
                            SetVariableStackPos(position, tokens, scopes, info, result!);
                        }
                        break;
                    // Declaraçao de array, sem atribuição sempre!
                    case TokenType.LESS:
                        {
                            AddNewVariable(position, tokens, scopes, true);

                            var sizeItem = tokens[3].Type == TokenType.STRING_DECL 
                                ? 1 
                                : 4;
                            var size = int.Parse(tokens[3].Content) * sizeItem;

                            // Cria vazio
                            AddEmptyStack(size, sizeItem, info);

                            // Seta posição do array
                            SetEmptyStackPos(position, tokens, scopes, info, size);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Muda valor de variavel
        /// </summary>
        /// <param name="info"></param>
        /// <param name="scopes"></param>
        /// <param name="tokens"></param>
        /// <param name="returnedType"></param>
        public void ChangeVariable(CompilationInfo info, Stack<Scope> scopes, List<Token> tokens, TokenType? returnedType)
        {
            var variables = scopes
                .SelectMany(x => x.Variables)
                .ToList();
        
            var variable = variables
                .Where(x => x.Name == tokens[0].Content && x.Type != TokenType.FUNCTION)
                .FirstOrDefault();

            // Conversão quando retorno é float para int
            if (variable?.Type == TokenType.INTEGER_DECL && returnedType == TokenType.DECIMAL_VAL)
                info.Lines.Add("cfi t0 t0");

            // Conversão quando retorno é int pra float
            if (variable?.Type == TokenType.DECIMAL_DECL && returnedType == TokenType.INTEGER_VAL)
                info.Lines.Add("cif t0 t0");

            // Quando passado o endereço da variave
            if(variable?.AddressStackPos != -1)
            {
                info.Lines.Add($"lw t5 {-variable!.AddressStackPos - info.StackPointer} sp");
                info.Lines.Add($"sw t0 0 t5");
            }
            else 
                info.Lines.Add($"sw t0 {variable!.StackPos-info.StackPointer} sp");
        }

        /// <summary>
        /// Adiciona espaço na stack vazio para variaveis declaradas sem valor, ou arrays de primitivos.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sizeItem"></param>
        /// <param name="info"></param>
        public void AddEmptyStack(int size, int sizeItem, CompilationInfo info)
        {
            info.Lines.Add("lir t0 0");

            foreach (var _ in Enumerable.Range(0, size/sizeItem))
            {
                if(sizeItem == 4)
                {
                    info.Lines.Add("sw t0 0 sp");
                    info.Lines.Add("addi sp sp 4");
                }
                else
                {
                    info.Lines.Add("sb t0 0 sp");
                    info.Lines.Add("addi sp sp 1");
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
            scopes.First().Variables.Add(new Variable()
            {
                Name = tokens[position - 1].Content,
                Type = tokens[position - 2].Type,
                IsArray = isArray || tokens[position -2 ].Type == TokenType.STRING_DECL,
            });
        }

        /// <summary>
        /// Seta posição de uma stack de uma variavel apartir de tamanho
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tokens"></param>
        /// <param name="scopes"></param>
        /// <param name="info"></param>
        /// <param name="size"></param>
        public void SetEmptyStackPos(
            int position,
            List<Token> tokens,
            Stack<Scope> scopes,
            CompilationInfo info,
            int size
        )
        {
            // Seta no array
            var variable = scopes.First().Variables.LastOrDefault();

            variable!.StackPos = info.StackPointer;
            variable!.Size = size;

            info.Lines.Add($"-- Variavel vazia '{tokens[position - 1].Content}' atribuida em {info.StackPointer} e tamanho {size}");
            info.StackPointer += size;
        }

        /// <summary>
        /// Seta posição na stack de uma variavel apartir do token da expressão calculada
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tokens"></param>
        /// <param name="scopes"></param>
        /// <param name="info"></param>
        /// <param name="token"></param>
        public void SetVariableStackPos(
            int position,
            List<Token> tokens,
            Stack<Scope> scopes,
            CompilationInfo info,
            CompilerToken token
        )
        {
            var variable = scopes.First().Variables.Last();

            if (token != null)
            {
                variable!.StackPos = token.StackPos;
                variable!.Size = token.StackSize;
            }
            
            // Conversão quando retorno é float para int
            if(variable.Type == TokenType.INTEGER_DECL && token?.Type == TokenType.DECIMAL_VAL)
            {
                info.Lines.Add("cfi t0 t0");
                info.Lines.Add("sw t0 -4 sp");
            }

            // Conversão quando retorno é int pra float
            if (variable.Type == TokenType.DECIMAL_DECL && token?.Type == TokenType.INTEGER_VAL)
            {
                info.Lines.Add("cif t0 t0");
                info.Lines.Add("sw t0 -4 sp");
            }

            info.Lines.Add($"-- Variavel '{tokens[position - 1].Content}' atribuida em {variable.StackPos} e tamanho {variable.Size}");
        }
    }
}
