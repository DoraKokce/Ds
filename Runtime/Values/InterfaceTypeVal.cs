namespace Ds.Runtime.Values {
    public class InterfaceTypeVal : ITypeVal {
        public RuntimeType Type { get {return RuntimeType.InterfaceTypeVal;} }
        public Dictionary<string, ITypeVal> Properties = new();
    }
}