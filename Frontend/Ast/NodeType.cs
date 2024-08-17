namespace Ds.Frontend.Ast {
    public enum NodeType {
        Program,
        FunctionDeclaration,
        TypeReference,
        ReturnStmt,
        VarDeclaration,
        ExprStmt,
        BlockStmt,
        ParenthesizedExpr,
        AssignmentExpr,
        BinExpr,
        ObjectLiteralExpr,
        ArrayLiteralExpr,
        MemberExpr,
        CallExpr,
        NumericLiteral,
        Identifier,
    }
}