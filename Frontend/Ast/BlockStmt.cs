namespace Ds.Frontend.Ast {
    public class BlockStmt : IStmt {

        public NodeType Kind { get {return NodeType.BlockStmt;} }

        public List<IStmt> Stmts = new();
    }
}