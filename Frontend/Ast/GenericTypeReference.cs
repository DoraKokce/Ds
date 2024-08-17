namespace Ds.Frontend.Ast {
    public class GenericTypeReference : ITypeRefernce {
        public NodeType Kind { get {return NodeType.TypeReference;} }
        public string Name { get; }
        public List<ITypeRefernce> Args = new();
        public GenericTypeReference (string name) {
            Name = name;
        }
    }
}