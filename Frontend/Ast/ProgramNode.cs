namespace Ds.Frontend.Ast {
    public class ProgramNode : IStmt {
        public int Cols { get {return 0;} }
        public int Rows { get {return 0;} }
        public NodeType Kind { get { return NodeType.Program; } }
        public List<IStmt> Body { get; }
        public ProgramNode() {
            Body = new List<IStmt>();
        }
    }
}