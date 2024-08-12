namespace Ds.Frontend.Ast {
    public class ArrayLiteralExpr : IExpr {
        public NodeType Kind { get {return NodeType.ArrayLiteralExpr;} }
        public List<IExpr> Vals = new();
        public ArrayLiteralExpr(List<IExpr>? vals = null) {
            if (vals != null) {
                Vals = vals;
            }
        }
    }
}