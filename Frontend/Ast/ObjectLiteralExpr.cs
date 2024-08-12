namespace Ds.Frontend.Ast {
    public class Propery {
        public string Key;
        public IExpr? Value;
        public Propery(string key, IExpr? val = null) {
            Key = key;
            Value = val;
        }
    }

    public class ObjectLiteralExpr : IExpr {
        public NodeType Kind { get {return NodeType.ObjectLiteralExpr;} }
        public List<Propery> Properties = new();
        public ObjectLiteralExpr(List<Propery>? props = null) {
            if (props != null) Properties = props;
        }
    }
}