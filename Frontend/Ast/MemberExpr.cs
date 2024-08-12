namespace Ds.Frontend.Ast {
    public class MemberExpr : IExpr {
        public NodeType Kind { get {return NodeType.MemberExpr;} }
        public IExpr Obj { get; }
        public IExpr Prop { get; }
        public bool Computed { get; }
        public MemberExpr(IExpr obj, IExpr prop, bool computed) {
            Obj = obj;
            Prop = prop;
            Computed = computed;
        }
    }
}