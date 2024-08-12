namespace Ds.Frontend.Ast {
    public class ParenthesizedExpr : IExpr {
        public NodeType Kind { get {return NodeType.ParenthesizedExpr;} }
        public IExpr Expr;
        public ParenthesizedExpr(IExpr expr) {
            Expr = expr;
        }
    }
}