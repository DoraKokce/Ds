namespace Ds.Runtime.Values {
    public class ArrayVal : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.Array;} }
        public List<IRuntimeVal> Vals = new();
    }
}