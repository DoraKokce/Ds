namespace Ds.Frontend.Ast {
    public class ExprStmt : IStmt {

        public NodeType Kind { get { return NodeType.ExprStmt; }}
        public IExpr Expr;
        public ExprStmt(IExpr expr) {
            Expr = expr;
        }
    }
}