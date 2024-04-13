using Language.Common.Enums;
using Language.Lexer.Exceptions;

namespace Language.Lexer.Utils
{
    /// <summary>
    /// Faz parser ignorando tabulação ou espaço multiplos (splitting)
    /// </summary>
    public class StringParser
    {
        private string _line;

        private static char[] specialChars = { ' ', '\t' };   

        public StringParser(string line)
        {
            _line = line;
        }

        public IEnumerable<(string, int)> GetStrings()
        {
            int index = 0;

            _line = _line.Replace("\\n", "\n");

            while (index < _line.Length)
            {
                // Percorre até achar espaço
                while (index < _line.Length && specialChars.Contains(_line[index]))
                    index++;

                if (index >= _line.Length)
                    yield break;

                // Entrada de string
                if (_line[index] == '"')
                {
                    var stringStart = index;
                    index++;

                    while (index < _line.Length)
                    {
                        if (_line[index] == '"' && stringStart != index)
                            break;

                        index++;
                    }

                    if (index >= _line.Length)
                        throw new LexerException("Unterminated string", null, ErrorCode.InvalidToken);

                    if (_line[index] != '"')
                        throw new LexerException("Unterminated string", null, ErrorCode.InvalidToken);

                    yield return (_line.Substring(stringStart, index - stringStart + 1), stringStart);
                    index++;
                    continue;
                }

                var start = index;

                while (index < _line.Length && !specialChars.Contains(_line[index]))
                {
                    if (_line[index] == '"')
                    {
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }

                yield return (_line.Substring(start, index - start), start);
            }
        }
    }
}
