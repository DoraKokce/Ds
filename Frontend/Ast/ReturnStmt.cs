namespace Ds.Frontend.Ast {
    public class ReturnStmt : IStmt {
        public NodeType Kind { get {return NodeType.ReturnStmt;} }
        public IExpr Return { get; }
        public ReturnStmt(IExpr ret) {
            Return = ret;
        }
    }
}