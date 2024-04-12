namespace Language.Lexer.Enums
{
    /// <summary>
    /// Tipo de match do regex
    /// </summary>
    public enum MatchType
    {
        // Match exato, sem nada junto 
        Exact,
        // Match exato mas pode ter mais outro token em conjunto
        ExactWithMore,
    }
}
