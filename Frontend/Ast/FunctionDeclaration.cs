namespace Ds.Frontend.Ast {
    public class FunctionDeclaration : IStmt {

        public NodeType Kind { get {return NodeType.FunctionDeclaration;} }
        public List<string> Args = new();
        public string Name;
        public BlockStmt Block = new();
        public FunctionDeclaration(string name) {
            Name = name;
        }
    }
}