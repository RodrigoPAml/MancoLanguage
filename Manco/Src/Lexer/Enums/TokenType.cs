namespace Manco.Lexer.Enums
{
    /// <summary>
    /// Tipo do token
    /// </summary>
    public enum TokenType
    {
        ANY,

        INTEGER_DECL,
        STRING_DECL,
        BOOLEAN_DECL,
        DECIMAL_DECL,

        INTEGER_DECL_REF,
        BOOLEAN_DECL_REF,
        DECIMAL_DECL_REF,

        COMMENT,

        IDENTIFIER,
        
        IF,
        ELSEIF,
        ELSE,
        
        OPEN,
        CLOSE,
        OPEN_BRACKET,
        CLOSE_BRACKET,

        INTEGER_VAL,
        DECIMAL_VAL,
        BOOL_VAL,
        STRING_VAL,

        RETURN,
        FUNCTION,
        BREAK,
        CONTINUE,
        WHILE,
        END,

        ASSIGN,
        EQUALS,

        OR,
        AND,
    
        PERCENTAGE,
        GREATER,
        GREATER_EQ,
        LESS,
        LESS_EQ,
        NOT_EQUAL,
        PLUS,
        MINUS,
        MULT,
        DIVIDE,

        DOT,
        COMMA,
        TWO_POINTS,

        // Só existe apartir da fase semântica

        // Acessos por índice
        ARR_INDEX_BOOL, 
        ARR_INDEX_INTEGER, 
        ARR_INDEX_STRING,
        ARR_INDEX_DECIMAL,
        
        // Identificadores sem índice
        STR_VAR,
        BOOL_VAR,
        DECIMAL_VAR,
        INTEGER_VAR
    }
}
