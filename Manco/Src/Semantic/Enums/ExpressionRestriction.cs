namespace Manco.Semantic.Enums
{
    /// <summary>
    /// Restrições de validação de expressão
    /// </summary>
    public enum ExpressionRestriction
    {
        None,
        StringArrayIndex, // Quando seta caracter em assign de string por índice
        StringDeclaration, // Declaração de string nova tem que ter tamanho pre determinado
        SingleReferenceVariable, // Chamada de função que espera uma variavel por referencia
        ArrayReferenceVariable, // Chamada de função que espera um array
    }
}
