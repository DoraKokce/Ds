namespace Ds.Runtime.Values {
    public class BoolVal : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.Bool;} }
        public bool Val { get; }
        public BoolVal(bool val) {
            Val = val;
        }
    }
}