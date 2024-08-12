namespace Ds.Frontend.Ast {
    public class CallExpr : IExpr {
        public NodeType Kind { get {return NodeType.CallExpr;} }
        public IExpr Caller { get; }
        public List<IExpr> Args { get; }
        public CallExpr(IExpr caller, List<IExpr> args) {
            Caller = caller;
            Args = args;
        }
    }
}