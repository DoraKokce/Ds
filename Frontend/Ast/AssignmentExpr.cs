namespace Ds.Frontend.Ast {
    public class AssignmentExpr : IExpr {
        public NodeType Kind { get {return NodeType.AssignmentExpr; } }
        public IExpr Left { get; }
        public IExpr Right { get; }
        public AssignmentExpr(IExpr left, IExpr right) {
            Left = left;
            Right = right;
        }
    }
}