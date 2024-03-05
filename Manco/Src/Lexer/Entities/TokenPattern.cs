using Language.Lexer.Enums;
using System.Text.RegularExpressions;
using MatchType = Language.Lexer.Enums.MatchType;

namespace Language.Lexer.Entities
{
    /// <summary>
    /// Classe que reconhece um token
    /// </summary>
    public class TokenPattern
    {
        private TokenType _tokenType;

        private MatchType _type;

        private Regex _match;

        public TokenPattern(MatchType type, string match, TokenType tokenType)
        {
            _type = type;
            _tokenType = tokenType;

            switch (_type)
            {
                case MatchType.Exact:
                    _match = new Regex($"^{match}$");
                    break;
                case MatchType.ExactWithMore:
                    _match = new Regex($"^{match}");
                    break;
                case MatchType.String:
                    _match = new Regex($"^{match}");
                    break;
            }
        }

        public Token? Find(string token, int line, int desloc)
        {
            var match = _match.Match(token);

            if (match.Success)
            {
                string result = match.Groups[0].Value;

                if (_type == MatchType.String)
                    result = result.Substring(1, result.Length - 2); 

                return new Token()
                {
                    Content = result,
                    Type = _tokenType,
                    Line = line,
                    Start = desloc,
                    End = desloc + result.Count(),
                    MoreOnLeft = match.Groups[0].Value.Count() != token.Count()
                };
            }

            return null;
        }
    }
}
