namespace Ds.Runtime.Values {
    public class ArrayTypeVal : ITypeVal {
        public RuntimeType Type { get {return RuntimeType.ArrayTypeVal;} }
        public ITypeVal ArrayType;
        public ArrayTypeVal(ITypeVal type) {ArrayType = type;}
    }
}