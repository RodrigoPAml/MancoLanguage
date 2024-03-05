using Language.Lexer.Enums;

namespace Language.Semantic.Entities
{
    public class Variable
    {
        /// <summary>
        /// The name of variable
        /// </summary>
        public string Name { get; set; }    

        /// <summary>
        /// The token type of the variable
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// If the variable is an array
        /// </summary>
        public bool IsArray { get; set; } = false;

        /// <summary>
        /// The array size, not always provided because if it's a function args its empty
        /// </summary>
        public int ArraySize { get; set; } = 0;

        /// <summary>
        /// If the argument is from the function declaration
        /// </summary>
        public bool FromFunction { get; set; } = false;

        /// <summary>
        /// Se é uma função aqui esta seus argumentos
        /// </summary>
        public List<Variable> ChildVariables { get; set; } = new List<Variable>();
    }
}
