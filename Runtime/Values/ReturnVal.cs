namespace Ds.Runtime.Values {
    public class ReturnVal : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.Return;} }
        public IRuntimeVal Ret;
        public ReturnVal(IRuntimeVal ret) {
            Ret = ret;
        }
    }
}