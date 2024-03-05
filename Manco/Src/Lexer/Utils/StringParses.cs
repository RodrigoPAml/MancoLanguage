namespace Language.Lexer.Utils
{
    /// <summary>
    /// Faz parser ignorando tabulação ou espaço multiplos (splitting)
    /// </summary>
    public class StringParser
    {
        private readonly string _line;

        private static char[] specialChars = { ' ', '\t' };   

        public StringParser(string line)
        {
            _line = line;
        }

        public IEnumerable<(string, int)> GetStrings()
        {
            int index = 0;

            while (index < _line.Length)
            {
                while (index < _line.Length && specialChars.Contains(_line[index]))
                    index++;

                if (index >= _line.Length)
                    yield break;

                int start = index;

                while (index < _line.Length && !specialChars.Contains(_line[index]))
                    index++;

                yield return (_line.Substring(start, index - start), start);
            }
        }
    }
}
