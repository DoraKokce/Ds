namespace Ds.Runtime.Values {
    public class TypeVal : ITypeVal {
        public RuntimeType Type { get {return RuntimeType.TypeVal;} }
        public RuntimeType TypeName { get; }
        public TypeVal(RuntimeType type) {TypeName = type;}
    }
}