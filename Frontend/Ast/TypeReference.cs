namespace Ds.Frontend.Ast {
    public class TypeReference : ITypeRefernce {
        public NodeType Kind { get {return NodeType.TypeReference;} }
        public string Name { get; }
        public TypeReference(string name) {
            Name = name;
        }
    }
}