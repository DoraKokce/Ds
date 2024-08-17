namespace Ds.Frontend.Ast {
    public class ArrayTypeReference : ITypeRefernce {
        public NodeType Kind { get {return NodeType.TypeReference;} }
        public ITypeRefernce Type { get; }
        public ArrayTypeReference(ITypeRefernce type) {
            Type = type;
        }
    }
}