namespace Ds.Runtime.Values {
    public class ObjectVal : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.Object;} }
        public Dictionary<string,IRuntimeVal> properties = new();
    }
}