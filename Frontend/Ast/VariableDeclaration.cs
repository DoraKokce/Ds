namespace Ds.Frontend.Ast {
    public class VarDeclaration : IStmt {
        public NodeType Kind { get {return NodeType.VarDeclaration; } }
        public List<string> Identifiers = new();
        public List<IExpr> Vals = new();
        public bool Constant = false;
    }
}