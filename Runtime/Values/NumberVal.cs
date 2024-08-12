namespace Ds.Runtime.Values {
    public class NumberVal : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.Number;} }
        public double Value { get; }
        public NumberVal(double val) {
            Value = val;
        }
    }
}