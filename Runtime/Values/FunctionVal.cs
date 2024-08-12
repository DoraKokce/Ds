using Ds.Frontend.Ast;

namespace Ds.Runtime.Values {
    public class FunctionValue : IRuntimeVal {
        public RuntimeType Type { get {return RuntimeType.FnVal;} }
        public string Name;
        public List<string> Args = new();
        public DSEnvironment Env;
        public BlockStmt Body;
        public FunctionValue(string name, DSEnvironment env, BlockStmt expr) {
            Name = name;
            Env = env;
            Body = expr;
        }
    }
}