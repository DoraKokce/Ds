namespace Ds.Frontend.Ast {
    public class Identifier : IExpr {
        public NodeType Kind { get {return NodeType.Identifier;}}
        public string Value { get; }
        public Identifier(string val) {
            Value = val;
        }
    }
}