namespace Ds.Runtime.Values {
    public delegate IRuntimeVal NativeFnCall(List<IRuntimeVal> args, DSEnvironment env);

    public class NativeFnVal : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.NativeFn;} }
        public NativeFnCall Call { get; }
        public NativeFnVal(NativeFnCall call) {
            Call = call;
        }
    }
}