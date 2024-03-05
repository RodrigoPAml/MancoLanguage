using Language.Common.Enums;
using Language.Lexer.Entities;
using Language.Lexer.Enums;
using Language.Lexer.Exceptions;
using Language.Lexer.Utils;
using MatchType = Language.Lexer.Enums.MatchType;

namespace Language.Lexer
{
    /// <summary>
    /// Implementação do lexer, detecta tokens da linguagem
    /// Caso algum token seja invalido gera exceção
    /// </summary>
    public class Lexer
    {
        private List<TokenPattern> _patterns = new List<TokenPattern>();

        private List<List<Token>> _result = new List<List<Token>>();

        private List<Token> _lineTokens = new List<Token>();

        public Lexer()
        {
            _patterns.Add(new TokenPattern(MatchType.Exact, "integer&", TokenType.INTEGER_DECL_REF));
            _patterns.Add(new TokenPattern(MatchType.Exact, "decimal&", TokenType.DECIMAL_DECL_REF));
            _patterns.Add(new TokenPattern(MatchType.Exact, "bool&", TokenType.BOOLEAN_DECL_REF));

            _patterns.Add(new TokenPattern(MatchType.Exact, "integer", TokenType.INTEGER_DECL));
            _patterns.Add(new TokenPattern(MatchType.Exact, "decimal", TokenType.DECIMAL_DECL));
            _patterns.Add(new TokenPattern(MatchType.Exact, "bool", TokenType.BOOLEAN_DECL));
            _patterns.Add(new TokenPattern(MatchType.Exact, "string", TokenType.STRING_DECL));

            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "--", TokenType.COMMENT));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, ">=", TokenType.GREATER_EQ));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "<=", TokenType.LESS_EQ));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, ">", TokenType.GREATER));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "<", TokenType.LESS));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "-", TokenType.MINUS));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\+", TokenType.PLUS));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\/", TokenType.DIVIDE));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\*", TokenType.MULT));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\%", TokenType.PERCENTAGE));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "!=", TokenType.NOT_EQUAL));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "==", TokenType.EQUALS));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "=", TokenType.ASSIGN));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, ":", TokenType.TWO_POINTS));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\(", TokenType.OPEN));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\)", TokenType.CLOSE));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "[,]", TokenType.COMMA));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\.", TokenType.DOT));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\[", TokenType.OPEN_BRACKET));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\]", TokenType.CLOSE_BRACKET));


            _patterns.Add(new TokenPattern(MatchType.Exact, "OR", TokenType.OR));
            _patterns.Add(new TokenPattern(MatchType.Exact, "AND", TokenType.AND));

            _patterns.Add(new TokenPattern(MatchType.Exact, "continue", TokenType.CONTINUE));
            _patterns.Add(new TokenPattern(MatchType.Exact, "break", TokenType.BREAK));
            _patterns.Add(new TokenPattern(MatchType.Exact, "return", TokenType.RETURN));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "function", TokenType.FUNCTION));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "while", TokenType.WHILE));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "elif", TokenType.ELSEIF));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "else", TokenType.ELSE));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "if", TokenType.IF));
            _patterns.Add(new TokenPattern(MatchType.Exact, "end", TokenType.END));

            _patterns.Add(new TokenPattern(MatchType.String, "\"[^\"]*\"", TokenType.STRING_VAL));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "(true|false)", TokenType.BOOL_VAL));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "\\d+\\.\\d+", TokenType.DECIMAL_VAL));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "[0-9][0-9]*", TokenType.INTEGER_VAL));
            _patterns.Add(new TokenPattern(MatchType.ExactWithMore, "[a-zA-Z_][a-zA-Z0-9_]*", TokenType.IDENTIFIER));
        }

        /// <summary>
        /// Retorna resultados por linha
        /// </summary>
        /// <returns></returns>
        public List<List<Token>> GetResult()
        {
            return _result;
        }

        /// <summary>
        /// Printa resultados
        /// </summary>
        public void Print()
        {
            if (_result == null)
                return;

            foreach (var line in _result)
            {
                foreach (var token in line)
                {
                    Console.Write(token.ToString() + " ");
                }
                
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Parse do arquivo de código
        /// </summary>
        /// <param name="filePath"></param>
        public void Parse(string filePath)
        {
            try
            {
                _lineTokens = null;
                _result = new List<List<Token>>();

                var lines = File.ReadAllLines(filePath);
                int lineIndex = 0;

                foreach (var line in lines)
                {
                    lineIndex++;

                    if (line.Trim().Count() == 0)
                        continue;

                    _lineTokens = new List<Token>();
                    StringParser _parser = new StringParser(line);

                    foreach (var (tokenStr, start) in _parser.GetStrings())
                    {
                        Work(tokenStr, lineIndex, start);
                    }

                    if (_lineTokens.Count() > 0)
                        _result.Add(_lineTokens);
                }
            }
            catch
            {
                _result = null;
                throw;
            }
            finally
            {
                _lineTokens = null;
            }
        }

        private bool Work(string token, int lineIndex, int desloc)
        {
            if (token.Count() == 0)
                return true;

            foreach (TokenPattern tokenizer in _patterns)
            {
                var resultToken = tokenizer.Find(token, lineIndex, desloc);

                if (resultToken != null)
                {
                    _lineTokens.Add(resultToken);

                    if (resultToken.MoreOnLeft)
                        return Work(token.Substring(resultToken.Size), lineIndex, desloc+resultToken.Size);

                    return true;
                }
            }

            throw new LexerException(
                $"Token ({token}) not allowed",
                new Token()     
                {
                    Content = token,
                    Line = lineIndex,
                    Start = desloc,
                    End = desloc + token.Count(),
                    MoreOnLeft = false,
                    Type = TokenType.ANY
                },
                ErrorCode.InvalidToken
            );
        }
    }
}
