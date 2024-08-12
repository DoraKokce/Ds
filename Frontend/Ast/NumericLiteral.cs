namespace Ds.Frontend.Ast {
    public class NumericLiteral : IExpr {
        public NodeType Kind { get { return NodeType.NumericLiteral; }}
        public float Value { get; }
        public NumericLiteral(float num) {
            Value = num;
        }
    }
}