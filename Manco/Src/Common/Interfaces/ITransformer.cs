using Manco.Lexer.Entities;

namespace Manco.Common.Interfaces
{
    /// <summary>
    /// Classe base para compilador e transpilador
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Transforma tokens em código
        /// Assume que já passou por etapas de validação (lexica, sintatica e semantica)
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public List<string> Execute(List<List<Token>> tokens);
    }
}
