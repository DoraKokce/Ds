namespace Ds.Frontend.Ast {
    public class BinExpr : IExpr {
        public NodeType Kind { get {return NodeType.BinExpr;} }
        public IExpr Left { get; }
        public string Op { get; }
        public IExpr Right { get; }
        public BinExpr(IExpr left, string op, IExpr right) {
            Left = left;
            Op = op;
            Right = right;
        }
    }
}